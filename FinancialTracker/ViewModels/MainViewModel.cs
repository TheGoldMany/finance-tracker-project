using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using FinancialTracker.Models;
using FinancialTracker.Services;
using FinancialTracker.Views;

namespace FinancialTracker.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<Income> _incomes;
        private ObservableCollection<Expense> _expenses;
        private ICollectionView _filteredExpenses;
        private ICollectionView _filteredIncomes;
        private ExpenseCategory? _selectedCategory;
        private DateTime? _selectedDate;
        private Saving _saving;
        private BudgetDistributor _budgetDistributor;
        private Dictionary<ExpenseCategory, decimal> _categoryExpenses;
        private readonly DataService _dataService = new DataService();
        private CategorySettings[] _categorySettings;
        private DispatcherTimer _propertyChangedTimer;

        // Rendezés tulajdonságok
        private ListSortDirection _expensesSortDirection = ListSortDirection.Descending;
        private string _expensesSortProperty = "Date";
        private ListSortDirection _incomesSortDirection = ListSortDirection.Descending;
        private string _incomesSortProperty = "Date";

        public MainViewModel()
        {
            // Inicializáljuk a _categoryExpenses-t egy üres Dictionary-vel
            _categoryExpenses = new Dictionary<ExpenseCategory, decimal>();

            // Inicializáljuk az Expenses és Incomes gyűjteményeket üres gyűjteményként
            Expenses = new ObservableCollection<Expense>();
            Incomes = new ObservableCollection<Income>();

            // Kategória beállítások betöltése
            _categorySettings = _dataService.LoadCategorySettings();

            // Most már be tudjuk állítani a szűrőket
            _filteredExpenses = CollectionViewSource.GetDefaultView(Expenses);
            _filteredExpenses.Filter = ExpenseFilter;

            // Beállítjuk az alapértelmezett rendezést (dátum szerint, csökkenő sorrendben)
            _filteredExpenses.SortDescriptions.Add(new SortDescription(_expensesSortProperty, _expensesSortDirection));

            // Bevételek szűrésének beállítása
            _filteredIncomes = CollectionViewSource.GetDefaultView(Incomes);
            _filteredIncomes.Filter = IncomeFilter;

            // Beállítjuk az alapértelmezett rendezést (dátum szerint, csökkenő sorrendben)
            _filteredIncomes.SortDescriptions.Add(new SortDescription(_incomesSortProperty, _incomesSortDirection));

            // Költségvetés elosztó inicializálása
            _budgetDistributor = new BudgetDistributor();

            // Megtakarítási objektum inicializálása nullára
            _saving = new Saving { Amount = 0 };

            // Parancsok inicializálása
            AddIncomeCommand = new RelayCommand(param => AddIncome());
            AddExpenseCommand = new RelayCommand(param => AddExpense());
            NavigateToSavingsCommand = new RelayCommand(param => NavigateToSavings());
            ClearFiltersCommand = new RelayCommand(param => ClearFilters());
            EditCategorySettingsCommand = new RelayCommand(param => EditCategorySettings());

            // Rendezési parancsok
            SortExpensesCommand = new RelayCommand<string>(property => SortExpenses(property));
            SortIncomesCommand = new RelayCommand<string>(property => SortIncomes(property));

            // Szerkesztés és törlés parancsok
            EditExpenseCommand = new RelayCommand<Expense>(expense => EditExpense(expense));
            DeleteExpenseCommand = new RelayCommand<Expense>(expense => DeleteExpense(expense));
            EditIncomeCommand = new RelayCommand<Income>(income => EditIncome(income));
            DeleteIncomeCommand = new RelayCommand<Income>(income => DeleteIncome(income));

            // Súgó parancs inicializálása
            OpenHelpCommand = new RelayCommand(param => OpenHelp());


            // Alkalmazás bezárásakor adatok mentése
            Application.Current.Exit += (s, e) => SaveAllData();

            // Adatok betöltése - utolsó lépésként, miután már minden inicializálva van
            LoadDataAsync();

            // Adatok betöltése utáni inicializálás
            _propertyChangedTimer = new DispatcherTimer();
            _propertyChangedTimer.Interval = TimeSpan.FromMilliseconds(100);
            _propertyChangedTimer.Tick += (s, e) => {
                _propertyChangedTimer.Stop();
                // Újraszámoljuk a költségvetési elosztást
                CalculateCategoryExpenses();
                OnPropertyChanged(nameof(CategoryExpenses));
            };
        }

        public ObservableCollection<Income> Incomes
        {
            get => _incomes;
            set => SetProperty(ref _incomes, value);
        }

        public ObservableCollection<Expense> Expenses
        {
            get => _expenses;
            set => SetProperty(ref _expenses, value);
        }

        public ICollectionView FilteredExpenses => _filteredExpenses;

        public ICollectionView FilteredIncomes => _filteredIncomes;

        public ExpenseCategory? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (SetProperty(ref _selectedCategory, value))
                {
                    _filteredExpenses.Refresh();
                }
            }
        }

        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (SetProperty(ref _selectedDate, value))
                {
                    _filteredExpenses.Refresh();
                    _filteredIncomes.Refresh();
                }
            }
        }

        public Dictionary<ExpenseCategory, decimal> CategoryExpenses
        {
            get => _categoryExpenses;
            set => SetProperty(ref _categoryExpenses, value);
        }

        // Jelenlegi egyenleg (kizárólag a nem megtakarításból fizetett összegek)
        public decimal Balance
        {
            get
            {
                decimal incomes = Incomes
                    .Where(i => i.Type != IncomeType.Megtakarítás)
                    .Sum(i => i.Amount);

                decimal expenses = Expenses
                    .Where(e => !e.FromSavings)
                    .Sum(e => e.Amount);

                // Megtakarításba helyezett összegek is levonódnak az egyenlegből
                decimal savingsTransfers = Incomes
                    .Where(i => i.Type == IncomeType.Megtakarítás)
                    .Sum(i => i.Amount);

                return incomes - expenses - savingsTransfers;
            }
        }

        // Megtakarítás összege
        public decimal SavingsAmount
        {
            get
            {
                decimal savingsIncomes = Incomes
                    .Where(i => i.Type == IncomeType.Megtakarítás)
                    .Sum(i => i.Amount);

                decimal savingsExpenses = Expenses
                    .Where(e => e.FromSavings)
                    .Sum(e => e.Amount);

                return _saving.Amount + savingsIncomes - savingsExpenses;
            }
        }

        // Parancsok
        public ICommand AddIncomeCommand { get; }
        public ICommand AddExpenseCommand { get; }
        public ICommand NavigateToSavingsCommand { get; }
        public ICommand ClearFiltersCommand { get; }
        public ICommand SortExpensesCommand { get; }
        public ICommand SortIncomesCommand { get; }
        public ICommand EditExpenseCommand { get; }
        public ICommand DeleteExpenseCommand { get; }
        public ICommand EditIncomeCommand { get; }
        public ICommand DeleteIncomeCommand { get; }
        public ICommand EditCategorySettingsCommand { get; }
        public ICommand OpenHelpCommand { get; }

        private bool ExpenseFilter(object item)
        {
            if (!(item is Expense expense))
                return false;

            bool categoryMatch = true;
            if (_selectedCategory != null)
            {
                categoryMatch = expense.Category == _selectedCategory;
            }

            bool dateMatch = true;
            if (_selectedDate != null)
            {
                dateMatch = expense.Date.Date == _selectedDate.Value.Date;
            }

            return categoryMatch && dateMatch;
        }

        private bool IncomeFilter(object item)
        {
            if (!(item is Income income))
                return false;

            if (_selectedDate != null)
            {
                return income.Date.Date == _selectedDate.Value.Date;
            }

            return true;
        }

        private void OpenHelp()
        {
            var helpWindow = new HelpWindow();
            helpWindow.ShowDialog();
        }
        private void SortExpenses(string property)
        {
            if (_expensesSortProperty == property)
            {
                // Irány váltása, ha ugyanaz a tulajdonság
                _expensesSortDirection = _expensesSortDirection == ListSortDirection.Ascending
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending;
            }
            else
            {
                // Új tulajdonság esetén alapértelmezetten csökkenő sorrend
                _expensesSortProperty = property;
                _expensesSortDirection = ListSortDirection.Descending;
            }

            // Rendezés beállítása
            _filteredExpenses.SortDescriptions.Clear();
            _filteredExpenses.SortDescriptions.Add(new SortDescription(_expensesSortProperty, _expensesSortDirection));
            _filteredExpenses.Refresh();
        }

        private void SortIncomes(string property)
        {
            if (_incomesSortProperty == property)
            {
                // Irány váltása, ha ugyanaz a tulajdonság
                _incomesSortDirection = _incomesSortDirection == ListSortDirection.Ascending
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending;
            }
            else
            {
                // Új tulajdonság esetén alapértelmezetten csökkenő sorrend
                _incomesSortProperty = property;
                _incomesSortDirection = ListSortDirection.Descending;
            }

            // Rendezés beállítása
            _filteredIncomes.SortDescriptions.Clear();
            _filteredIncomes.SortDescriptions.Add(new SortDescription(_incomesSortProperty, _incomesSortDirection));
            _filteredIncomes.Refresh();
        }

        private void AddIncome()
        {
            var addIncomeWindow = new AddIncomeWindow();
            var viewModel = addIncomeWindow.DataContext as AddIncomeViewModel;

            if (addIncomeWindow.ShowDialog() == true)
            {
                // Új bevétel hozzáadása
                Incomes.Add(viewModel.NewIncome);

                // Automatikus megtakarítás hozzáadása, ha be van állítva
                if (viewModel.SavingsPercentage > 0 && viewModel.Type != IncomeType.Megtakarítás)
                {
                    decimal savingsAmount = viewModel.Amount * (viewModel.SavingsPercentage / 100m);

                    // Hozzáadunk egy új megtakarítás típusú bevételt
                    Incomes.Add(new Income
                    {
                        Date = viewModel.Date,
                        Amount = savingsAmount,
                        Description = "Automatikus megtakarítás",
                        Type = IncomeType.Megtakarítás,
                        SavingsPercentage = 0
                    });
                }

                // Egyenleg és megtakarítások frissítése
                OnPropertyChanged(nameof(Balance));
                OnPropertyChanged(nameof(SavingsAmount));

                // Aktuális költések frissítése
                CalculateCurrentSpending();

                // Kiadási keretösszegek újraszámolása
                CalculateCategoryExpenses();

                // Frissítjük a szűrt listákat
                if (_filteredIncomes != null)
                {
                    _filteredIncomes.Refresh();
                }

                // Adatok mentése
                SaveAllData();
            }
        }

        private void AddExpense()
        {
            var addExpenseWindow = new AddExpenseWindow();
            var viewModel = addExpenseWindow.DataContext as AddExpenseViewModel;

            if (addExpenseWindow.ShowDialog() == true)
            {
                // Ellenőrzés, hogy van-e elegendő pénz
                if (viewModel.SourceIndex == 1) // FromSavings
                {
                    if (SavingsAmount < viewModel.Amount)
                    {
                        MessageBox.Show("Nincs elegendő pénz a megtakarítási számládon!",
                            "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    // Egyenleg ellenőrzése
                    if (Balance < viewModel.Amount)
                    {
                        MessageBox.Show("Nincs elegendő pénz az egyenleg számládon!",
                            "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Kategória költségvetési korlát ellenőrzése
                    var categorySettings = _categorySettings.FirstOrDefault(s => s.Category == viewModel.Category);
                    if (categorySettings != null && categorySettings.WouldExceedBudget(viewModel.Amount))
                    {
                        MessageBox.Show($"A(z) {viewModel.Category} kategóriára beállított költségvetési keretet túllépnéd ezzel a kiadással!\n" +
                                       $"Maximum: {categorySettings.MaxAmount:N0} Ft\n" +
                                       $"Már elköltöttél: {categorySettings.CurrentSpent:N0} Ft\n" +
                                       $"Ez a kiadás: {viewModel.Amount:N0} Ft",
                                       "Költségvetési korlát túllépése", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                // Új kiadás hozzáadása
                Expenses.Add(viewModel.NewExpense);

                // Egyenleg és megtakarítások frissítése
                OnPropertyChanged(nameof(Balance));
                OnPropertyChanged(nameof(SavingsAmount));

                // Aktuális költések frissítése
                CalculateCurrentSpending();

                // Kiadási keretösszegek újraszámolása
                CalculateCategoryExpenses();

                // Frissítjük a szűrt listákat
                _filteredExpenses.Refresh();

                // Adatok mentése
                SaveAllData();
            }
        }

        private void EditExpense(Expense expense)
        {
            // Létrehozunk egy másolatot, hogy ha a felhasználó megszakítja a szerkesztést, az eredeti adatok ne változzanak
            var expenseCopy = new Expense
            {
                Id = expense.Id,
                Amount = expense.Amount,
                Description = expense.Description,
                Category = expense.Category,
                CustomCategory = expense.CustomCategory,
                Date = expense.Date,
                FromSavings = expense.FromSavings
            };

            var addExpenseWindow = new AddExpenseWindow();
            var viewModel = addExpenseWindow.DataContext as AddExpenseViewModel;
            viewModel.EditMode = true;
            viewModel.Amount = expenseCopy.Amount;
            viewModel.Description = expenseCopy.Description;
            viewModel.Category = expenseCopy.Category;
            viewModel.CustomCategory = expenseCopy.CustomCategory;
            viewModel.Date = expenseCopy.Date;
            viewModel.SourceIndex = expenseCopy.FromSavings ? 1 : 0;

            if (addExpenseWindow.ShowDialog() == true)
            {
                // Itt ellenőrizzük, hogy van-e elég pénz, ha megváltozik a forrás
                bool sourceChanged = expense.FromSavings != (viewModel.SourceIndex == 1);

                if (sourceChanged)
                {
                    if (viewModel.SourceIndex == 1 && Balance < viewModel.Amount)
                    {
                        MessageBox.Show("Nincs elegendő pénz az egyenleg számládon a változtatáshoz!",
                            "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else if (viewModel.SourceIndex == 0 && SavingsAmount < viewModel.Amount)
                    {
                        MessageBox.Show("Nincs elegendő pénz a megtakarítási számládon a változtatáshoz!",
                            "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Megerősítés kérése
                    MessageBoxResult result = MessageBox.Show(
                        $"A kiadás forrása megváltozott. Ez hatással lesz az {(viewModel.SourceIndex == 1 ? "egyenlegre" : "megtakarításra")}. Folytatja?",
                        "Megerősítés",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }

                // Ha új forrás nem megtakarítás, ellenőrizzük a kategória költségvetést
                if (viewModel.SourceIndex == 0 && expense.Category != viewModel.Category)
                {
                    var categorySettings = _categorySettings.FirstOrDefault(s => s.Category == viewModel.Category);
                    if (categorySettings != null && categorySettings.WouldExceedBudget(viewModel.Amount))
                    {
                        MessageBox.Show($"A(z) {viewModel.Category} kategóriára beállított költségvetési keretet túllépnéd ezzel a kiadással!\n" +
                                       $"Maximum: {categorySettings.MaxAmount:N0} Ft\n" +
                                       $"Már elköltöttél: {categorySettings.CurrentSpent:N0} Ft\n" +
                                       $"Ez a kiadás: {viewModel.Amount:N0} Ft",
                                       "Költségvetési korlát túllépése", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                // Módosítjuk az eredeti kiadást
                expense.Amount = viewModel.Amount;
                expense.Description = viewModel.Description;
                expense.Category = viewModel.Category;
                expense.CustomCategory = viewModel.CustomCategory;
                expense.Date = viewModel.Date;
                expense.FromSavings = (viewModel.SourceIndex == 1);

                // Egyenleg és megtakarítások frissítése
                OnPropertyChanged(nameof(Balance));
                OnPropertyChanged(nameof(SavingsAmount));

                // Aktuális költések frissítése
                CalculateCurrentSpending();

                // Kiadási keretösszegek újraszámolása
                CalculateCategoryExpenses();

                // Frissítjük a szűrt listákat
                _filteredExpenses.Refresh();

                // Adatok mentése
                SaveAllData();
            }
        }

        private void DeleteExpense(Expense expense)
        {
            // Megerősítés kérése
            MessageBoxResult result = MessageBox.Show(
                $"Biztosan törölni szeretné ezt a kiadást: {expense.Description} ({expense.Amount:N0} Ft)?",
                "Megerősítés",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Töröljük a kiadást
                Expenses.Remove(expense);

                // Egyenleg és megtakarítások frissítése
                OnPropertyChanged(nameof(Balance));
                OnPropertyChanged(nameof(SavingsAmount));

                // Aktuális költések frissítése
                CalculateCurrentSpending();

                // Kiadási keretösszegek újraszámolása
                CalculateCategoryExpenses();

                // Frissítjük a szűrt listákat
                _filteredExpenses.Refresh();

                // Adatok mentése
                SaveAllData();
            }
        }

        private void EditIncome(Income income)
        {
            // Létrehozunk egy másolatot, hogy ha a felhasználó megszakítja a szerkesztést, az eredeti adatok ne változzanak
            var incomeCopy = new Income
            {
                Amount = income.Amount,
                Description = income.Description,
                Date = income.Date,
                Type = income.Type,
                CustomType = income.CustomType,
                SavingsPercentage = income.SavingsPercentage
            };

            var addIncomeWindow = new AddIncomeWindow();
            var viewModel = addIncomeWindow.DataContext as AddIncomeViewModel;
            viewModel.EditMode = true;
            viewModel.Amount = incomeCopy.Amount;
            viewModel.Description = incomeCopy.Description;
            viewModel.Date = incomeCopy.Date;
            viewModel.Type = incomeCopy.Type;
            viewModel.CustomType = incomeCopy.CustomType;
            viewModel.SavingsPercentage = incomeCopy.SavingsPercentage;

            if (addIncomeWindow.ShowDialog() == true)
            {
                // Itt ellenőrizzük, hogy van-e elég pénz, ha megváltozik a típus
                bool typeChanged = income.Type != viewModel.Type;

                if (typeChanged && viewModel.Type == IncomeType.Megtakarítás)
                {
                    if (Balance < viewModel.Amount)
                    {
                        MessageBox.Show("Nincs elegendő pénz az egyenlegen a megtakarításhoz!",
                            "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Megerősítés kérése
                    MessageBoxResult result = MessageBox.Show(
                        "Biztos az egyenlegből szeretnél átvezetni a megtakarításba?",
                        "Megerősítés",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }

                // Módosítjuk az eredeti bevételt
                income.Amount = viewModel.Amount;
                income.Description = viewModel.Description;
                income.Date = viewModel.Date;
                income.Type = viewModel.Type;
                income.CustomType = viewModel.CustomType;
                income.SavingsPercentage = viewModel.SavingsPercentage;

                // Egyenleg és megtakarítások frissítése
                OnPropertyChanged(nameof(Balance));
                OnPropertyChanged(nameof(SavingsAmount));

                // Kiadási keretösszegek újraszámolása
                CalculateCategoryExpenses();

                // Aktuális költések frissítése
                CalculateCurrentSpending();

                // Frissítjük a szűrt listákat
                _filteredIncomes.Refresh();

                // Adatok mentése
                SaveAllData();
            }
        }

        private void DeleteIncome(Income income)
        {
            // Megerősítés kérése
            MessageBoxResult result = MessageBox.Show(
                $"Biztosan törölni szeretné ezt a bevételt: {income.Description} ({income.Amount:N0} Ft)?",
                "Megerősítés",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Töröljük a bevételt
                Incomes.Remove(income);

                // Egyenleg és megtakarítások frissítése
                OnPropertyChanged(nameof(Balance));
                OnPropertyChanged(nameof(SavingsAmount));

                // Aktuális költések frissítése
                CalculateCurrentSpending();

                // Kiadási keretösszegek újraszámolása
                CalculateCategoryExpenses();

                // Frissítjük a szűrt listákat
                _filteredIncomes.Refresh();

                // Adatok mentése
                SaveAllData();
            }
        }

        private void NavigateToSavings()
        {
            // Navigáció a megtakarítási oldalra
            var savingsWindow = new SavingsPage
            {
                DataContext = new SavingsViewModel(_saving, SavingsAmount)
            };
            savingsWindow.ShowDialog();
        }

        private void ClearFilters()
        {
            // Töröljük a kategória és dátum szűrőket
            SelectedCategory = null;
            SelectedDate = null;

            // Frissítjük a listákat
            _filteredExpenses.Refresh();
            _filteredIncomes.Refresh();
        }

        private void SaveAllData()
        {
            try
            {
                _dataService.SaveExpenses(Expenses);
                _dataService.SaveIncomes(Incomes);
                _dataService.SaveSaving(_saving);
                _dataService.SaveCategorySettings(_categorySettings);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt az adatok mentésekor: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalculateCurrentSpending()
        {
            // Aktuális hónap
            DateTime now = DateTime.Now;
            DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // Minden kategória költésének nullázása
            foreach (var setting in _categorySettings)
            {
                setting.CurrentSpent = 0;
            }

            // Aktuális költések összeszámolása
            foreach (var expense in Expenses)
            {
                if (expense.Date >= firstDayOfMonth && expense.Date <= lastDayOfMonth && !expense.FromSavings)
                {
                    var setting = _categorySettings.FirstOrDefault(s => s.Category == expense.Category);
                    if (setting != null)
                    {
                        setting.CurrentSpent += expense.Amount;
                    }
                }
            }

            // Maximum összegek kiszámítása
            decimal totalBudget = Balance;
            foreach (var setting in _categorySettings)
            {
                setting.MaxAmount = totalBudget * (setting.MaxPercentage / 100);
            }
        }

        private void EditCategorySettings()
        {
            var settingsWindow = new CategorySettingsWindow();
            var viewModel = new CategorySettingsViewModel(_categorySettings);
            settingsWindow.DataContext = viewModel;

            if (settingsWindow.ShowDialog() == true)
            {
                // Régi beállítások mentése (debug céljából)
                var oldSettings = _categorySettings;

                // Új beállítások lekérése
                _categorySettings = viewModel.GetSettings();

                // Debug információk a változásokról
                System.Diagnostics.Debug.WriteLine("Kategória beállítások frissítve:");
                for (int i = 0; i < _categorySettings.Length; i++)
                {
                    string changeInfo = $"{_categorySettings[i].Category}: " +
                                       $"{(oldSettings.Length > i ? oldSettings[i].MaxPercentage : 0)}% → {_categorySettings[i].MaxPercentage}%";
                    System.Diagnostics.Debug.WriteLine(changeInfo);
                }

                // Újraszámoljuk a kategória költéseket
                CalculateCurrentSpending();

                // Újraszámoljuk a költségvetési elosztást
                CalculateCategoryExpenses();

                // Frissítjük a felületet a változásokról
                OnPropertyChanged(nameof(CategoryExpenses));

                // Mentjük a beállításokat
                SaveAllData();
            }
        }

        private void CalculateCategoryExpenses()
        {
            try
            {
                // Ellenőrizzük, hogy a CategorySettings létezik-e
                if (_categorySettings == null || _categorySettings.Length == 0)
                {
                    // Ha nincs CategorySettings, akkor használjuk a BudgetDistributor-t
                    if (_budgetDistributor == null)
                    {
                        _budgetDistributor = new BudgetDistributor();
                    }

                    var distribution = _budgetDistributor.DistributeBudget(Balance);
                    if (distribution != null)
                    {
                        CategoryExpenses = distribution;
                    }
                    else
                    {
                        CategoryExpenses = new Dictionary<ExpenseCategory, decimal>();
                    }
                }
                else
                {
                    // Ha van CategorySettings, akkor annak alapján osztjuk el a költségvetést
                    var newCategoryExpenses = new Dictionary<ExpenseCategory, decimal>();
                    decimal totalBudget = Balance;

                    foreach (var setting in _categorySettings)
                    {
                        // A kategória maximum összegét a kategória százalékából számoljuk
                        decimal maxAmount = totalBudget * (setting.MaxPercentage / 100);
                        newCategoryExpenses[setting.Category] = maxAmount;
                    }

                    CategoryExpenses = newCategoryExpenses;
                }
            }
            catch (Exception ex)
            {
                // Hibakezelés, esetleg naplózás
                System.Diagnostics.Debug.WriteLine($"Hiba a kategória költségvetés számításakor: {ex.Message}");
                CategoryExpenses = new Dictionary<ExpenseCategory, decimal>();
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                Expenses = await _dataService.LoadExpensesAsync();
                Incomes = await _dataService.LoadIncomesAsync();
                _saving = await _dataService.LoadSavingAsync();
                _categorySettings = await _dataService.LoadCategorySettingsAsync();

                // Ha a megtakarítás nincs definiálva, inicializáljuk nullára
                if (_saving == null)
                {
                    _saving = new Saving { Amount = 0 };
                }

                // Szűrők visszaállítása
                _filteredExpenses = CollectionViewSource.GetDefaultView(Expenses);
                _filteredExpenses.Filter = ExpenseFilter;
                _filteredExpenses.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));

                _filteredIncomes = CollectionViewSource.GetDefaultView(Incomes);
                _filteredIncomes.Filter = IncomeFilter;
                _filteredIncomes.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));

                // Aktuális költések kiszámítása
                CalculateCurrentSpending();

                // Frissítsük az egyenleget és a megtakarításokat
                OnPropertyChanged(nameof(Balance));
                OnPropertyChanged(nameof(SavingsAmount));
                OnPropertyChanged(nameof(FilteredExpenses));
                OnPropertyChanged(nameof(FilteredIncomes));

                // Kategória kiadások számítása
                CalculateCategoryExpenses();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt az adatok betöltésekor: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
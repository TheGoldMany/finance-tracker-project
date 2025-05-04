using FinancialTracker.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace FinancialTracker.ViewModels
{
    public class AddExpenseViewModel : ViewModelBase
    {
        private decimal _amount;
        private string _description;
        private ExpenseCategory _category;
        private string _customCategory; // Egyedi kategória
        private DateTime _date;
        private int _sourceIndex = 0;  // 0 = Egyenleg, 1 = Megtakarítás
        private bool _editMode = false; // Szerkesztési mód

        public AddExpenseViewModel()
        {
            // Alapértékek beállítása
            Date = DateTime.Now;
            Category = ExpenseCategory.Élelmiszer; // Alapértelmezett kategória
            SourceIndex = 0; // Alapértelmezetten egyenlegből (0=Egyenleg, 1=Megtakarítás)

            // Parancsok inicializálása
            SaveCommand = new RelayCommand(param => SaveWithConfirmation(param), param => CanSave());
            CancelCommand = new RelayCommand(param => Cancel(param));
        }

        public decimal Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public ExpenseCategory Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        public string CustomCategory
        {
            get => _customCategory;
            set => SetProperty(ref _customCategory, value);
        }

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public int SourceIndex
        {
            get => _sourceIndex;
            set => SetProperty(ref _sourceIndex, value);
        }

        public bool EditMode
        {
            get => _editMode;
            set => SetProperty(ref _editMode, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Expense NewExpense
        {
            get
            {
                return new Expense
                {
                    Amount = Amount,
                    Description = Description,
                    Category = Category,
                    CustomCategory = Category == ExpenseCategory.Egyéb ? CustomCategory : null,
                    Date = Date,
                    FromSavings = (SourceIndex == 1)  // Konverzió SourceIndex-ből
                };
            }
        }

        private bool CanSave()
        {
            return Amount > 0 && !string.IsNullOrWhiteSpace(Description);
        }

        private void SaveWithConfirmation(object window)
        {
            // Ha megtakarításból vonunk le, akkor megerősítés kérése
            if (SourceIndex == 1)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Biztos a megtakarításodból szeretnéd fizetni ezt a kiadást?",
                    "Megerősítés",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    return; // Nem mentjük el, ha nem erősíti meg
                }
            }

            Save(window);
        }

        private void Save(object window)
        {
            if (window is Window win)
            {
                win.DialogResult = true;
                win.Close();
            }
        }

        private void Cancel(object window)
        {
            if (window is Window win)
            {
                win.DialogResult = false;
                win.Close();
            }
        }
    }
}
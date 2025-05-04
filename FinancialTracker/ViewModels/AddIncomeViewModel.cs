using FinancialTracker.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace FinancialTracker.ViewModels
{
    public class AddIncomeViewModel : ViewModelBase
    {
        private decimal _amount;
        private string _description;
        private DateTime _date;
        private IncomeType _type;
        private string _customType; // Egyedi típus
        private decimal _savingsPercentage = 0; // Megtakarítási százalék
        private bool _editMode = false; // Szerkesztési mód

        public AddIncomeViewModel()
        {
            // Alapértékek beállítása
            Date = DateTime.Now;
            Type = IncomeType.Munkabér; // Alapértelmezett típus

            // Parancsok inicializálása
            SaveCommand = new RelayCommand(param => Save(param), param => CanSave());
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

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public IncomeType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public string CustomType
        {
            get => _customType;
            set => SetProperty(ref _customType, value);
        }

        public decimal SavingsPercentage
        {
            get => _savingsPercentage;
            set => SetProperty(ref _savingsPercentage, value);
        }

        public bool EditMode
        {
            get => _editMode;
            set => SetProperty(ref _editMode, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Income NewIncome
        {
            get
            {
                return new Income
                {
                    Amount = Amount,
                    Description = Description,
                    Date = Date,
                    Type = Type,
                    CustomType = Type == IncomeType.Egyéb ? CustomType : null,
                    SavingsPercentage = SavingsPercentage
                };
            }
        }

        private bool CanSave()
        {
            return Amount > 0 && !string.IsNullOrWhiteSpace(Description);
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
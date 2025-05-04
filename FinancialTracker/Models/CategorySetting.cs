using FinancialTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Models
{
    // Kategória prioritások és limitek
    public class CategorySettings
    {
        public ExpenseCategory Category { get; set; }
        public int Priority { get; set; } // Alacsonyabb szám = magasabb prioritás
        public decimal MaxPercentage { get; set; } // Max. százalék az elkölthető összegből
        public decimal CurrentSpent { get; set; } // Aktuális költés ebben a kategóriában
        public decimal MaxAmount { get; set; } // Maximum elkölthető összeg (számolt érték)

        // Ellenőrzi, hogy túlléptük-e a keretet
        public bool IsOverBudget => CurrentSpent >= MaxAmount;

        // Ellenőrzi, hogy egy kiadás túllépné-e a keretet
        public bool WouldExceedBudget(decimal amount)
        {
            return CurrentSpent + amount > MaxAmount;
        }

        public static CategorySettings[] DefaultSettings = new CategorySettings[]
        {
            new CategorySettings { Category = ExpenseCategory.Élelmiszer, Priority = 1, MaxPercentage = 40 },
            new CategorySettings { Category = ExpenseCategory.Háztartás, Priority = 2, MaxPercentage = 25 },
            new CategorySettings { Category = ExpenseCategory.Közlekedés, Priority = 3, MaxPercentage = 15 },
            new CategorySettings { Category = ExpenseCategory.Egészség, Priority = 4, MaxPercentage = 10 },
            new CategorySettings { Category = ExpenseCategory.Szórakozás, Priority = 5, MaxPercentage = 5 },
            new CategorySettings { Category = ExpenseCategory.Egyéb, Priority = 6, MaxPercentage = 5 }
        };
    }


    public class CategorySettingItem : INotifyPropertyChanged
    {
        private ExpenseCategory _category;
        private int _priority;
        private decimal _maxPercentage;

        public event PropertyChangedEventHandler PropertyChanged;

        public ExpenseCategory Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Priority
        {
            get => _priority;
            set
            {
                if (_priority != value)
                {
                    _priority = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal MaxPercentage
        {
            get => _maxPercentage;
            set
            {
                // Csak egész számokat fogadunk el
                decimal roundedValue = decimal.Round(value);

                if (_maxPercentage != roundedValue)
                {
                    _maxPercentage = roundedValue;
                    OnPropertyChanged();
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


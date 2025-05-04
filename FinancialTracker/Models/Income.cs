using System;

namespace FinancialTracker.Models
{
    public class Income
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public IncomeType Type { get; set; } // Bevétel típusa
        public string CustomType { get; set; } // Egyedi típus, ha Type == IncomeType.Egyéb
        public decimal SavingsPercentage { get; set; } // Bevétel megtakarítási százaléka
    }

    public enum IncomeType
    {
        Munkabér,
        Megtakarítás,
        Egyéb
    }
}
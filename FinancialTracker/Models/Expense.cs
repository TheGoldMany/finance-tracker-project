using System;

namespace FinancialTracker.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public ExpenseCategory Category { get; set; }
        public string CustomCategory { get; set; }
        public DateTime Date { get; set; }
        public bool FromSavings { get; set; } // Megtakarításból fizetve?
    }

    public enum ExpenseCategory
    {
        Élelmiszer,
        Háztartás,
        Közlekedés,
        Egészség,
        Szórakozás,
        Egyéb
    }

}
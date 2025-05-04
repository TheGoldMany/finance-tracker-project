namespace FinancialTracker.Models
{
    public class StockInfo
    {
        public string Symbol { get; set; } = "NVDA";
        public string Name { get; set; } = "NVIDIA Corporation";
        public decimal CurrentPrice { get; set; } = 1156.32m; // Mock ár (USD)
        public decimal HufExchangeRate { get; set; } = 361.5m; // Mock árfolyam (HUF/USD)

        public decimal GetPriceInHuf()
        {
            return CurrentPrice * HufExchangeRate;
        }

        public decimal CalculateSharesCanBuy(decimal savingsAmount)
        {
            if (GetPriceInHuf() <= 0) return 0;
            return savingsAmount / GetPriceInHuf(); // Eltávolítottuk az (int) kasztolást
        }

        
    }
}
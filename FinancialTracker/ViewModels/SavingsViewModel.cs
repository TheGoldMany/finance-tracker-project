using System.Windows;
using System.Windows.Input;
using FinancialTracker.Models;

namespace FinancialTracker.ViewModels
{
    public class SavingsViewModel : ViewModelBase
    {
        private readonly Saving _saving;
        private readonly StockInfo _stockInfo;
        private readonly decimal _currentSavingsAmount;

        // Írható-olvasható tulajdonságok az adatkötéshez
        private string _stockSymbol;
        private string _stockName;
        private decimal _stockPriceUsd;
        private decimal _stockPriceHuf;
        private decimal _sharesCanBuy;

        public SavingsViewModel(Saving saving, decimal currentSavingsAmount)
        {
            _saving = saving;
            _currentSavingsAmount = currentSavingsAmount;
            _stockInfo = new StockInfo(); // Valós alkalmazásban API vagy service-ből jönne

            // Inicializáljuk a tulajdonságokat - csillagok nélkül
            _stockSymbol = _stockInfo.Symbol;
            _stockName = _stockInfo.Name;
            _stockPriceUsd = _stockInfo.CurrentPrice;
            _stockPriceHuf = _stockInfo.GetPriceInHuf();
            _sharesCanBuy = _stockInfo.CalculateSharesCanBuy(_currentSavingsAmount);

            // Window típus helyesen megadva
            BackCommand = new RelayCommand<Window>(param => CloseWindow(param));
        }

        public decimal SavingsAmount => _currentSavingsAmount;

        public string StockSymbol
        {
            get => _stockSymbol;
            set => SetProperty(ref _stockSymbol, value);
        }

        public string StockName
        {
            get => _stockName;
            set => SetProperty(ref _stockName, value);
        }

        public decimal StockPriceUsd
        {
            get => _stockPriceUsd;
            set => SetProperty(ref _stockPriceUsd, value);
        }

        public decimal StockPriceHuf
        {
            get => _stockPriceHuf;
            set => SetProperty(ref _stockPriceHuf, value);
        }

        public decimal SharesCanBuy
        {
            get => _sharesCanBuy;
            set => SetProperty(ref _sharesCanBuy, value);
        }

        // Window típusú paramétert vár
        public ICommand BackCommand { get; }

        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
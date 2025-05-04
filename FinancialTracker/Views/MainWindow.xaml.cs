using FinancialTracker.ViewModels;
using System.Windows;

namespace FinancialTracker.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
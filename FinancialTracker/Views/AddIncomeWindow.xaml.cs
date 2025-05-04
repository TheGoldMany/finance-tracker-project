using FinancialTracker.ViewModels;
using System.Windows;

namespace FinancialTracker.Views
{
    public partial class AddIncomeWindow : Window
    {
        public AddIncomeWindow()
        {
            InitializeComponent();
            DataContext = new AddIncomeViewModel();
        }
    }
}
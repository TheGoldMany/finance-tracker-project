using FinancialTracker.ViewModels;
using System.Windows;

namespace FinancialTracker.Views
{
    public partial class AddExpenseWindow : Window
    {
        public AddExpenseWindow()
        {
            InitializeComponent();
            DataContext = new AddExpenseViewModel();
        }
    }
}
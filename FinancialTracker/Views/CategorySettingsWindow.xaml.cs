using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinancialTracker.Views
{
    public partial class CategorySettingsWindow : Window
    {
        private static readonly Regex _numericRegex = new Regex("[^0-9]+");

        public CategorySettingsWindow()
        {
            InitializeComponent();

            // Eseménykezelő hozzáadása dinamikusan a létrehozott TextBox-okhoz
            Loaded += (s, e) =>
            {
                foreach (var textBox in FindVisualChildren<TextBox>(this))
                {
                    textBox.PreviewTextInput += TextBox_PreviewTextInput;
                    textBox.PreviewKeyDown += TextBox_PreviewKeyDown;

                    // DataObject.AddPastingHandler és DataObject.AddCopyingHandler is hozzáadható
                    // további validációhoz a vágólapról való beillesztés esetén
                }
            };
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Csak számokat fogadunk el
            e.Handled = _numericRegex.IsMatch(e.Text);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // A szóköz lenyomását is letiltjuk
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        // Segédmetódus az összes TextBox megtalálásához a vizuális fában
        private static System.Collections.Generic.IEnumerable<T> FindVisualChildren<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject != null)
            {
                for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
                {
                    DependencyObject child = System.Windows.Media.VisualTreeHelper.GetChild(dependencyObject, i);
                    if (child != null && child is T t)
                    {
                        yield return t;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
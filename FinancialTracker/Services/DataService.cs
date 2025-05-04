using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using FinancialTracker.Models;

namespace FinancialTracker.Services
{
    public class DataService
    {
        private readonly string _expensesFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FinancialTracker", "expenses.json");

        private readonly string _incomesFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FinancialTracker", "incomes.json");

        private readonly string _savingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FinancialTracker", "savings.json");

        private readonly string _categorySettingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FinancialTracker", "categorysettings.json");

        public DataService()
        {
            // Győződjünk meg arról, hogy a könyvtár létezik
            Directory.CreateDirectory(Path.GetDirectoryName(_expensesFilePath));
        }

        // Szinkron mentési metódusok
        public void SaveExpenses(ObservableCollection<Expense> expenses)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(expenses, options);
            File.WriteAllText(_expensesFilePath, jsonString);
        }

        public void SaveIncomes(ObservableCollection<Income> incomes)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(incomes, options);
            File.WriteAllText(_incomesFilePath, jsonString);
        }

        public void SaveSaving(Saving saving)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(saving, options);
            File.WriteAllText(_savingsFilePath, jsonString);
        }

        public void SaveCategorySettings(CategorySettings[] settings)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(settings, options);
            File.WriteAllText(_categorySettingsFilePath, jsonString);
        }

        // Aszinkron mentési metódusok
        public async Task SaveExpensesAsync(ObservableCollection<Expense> expenses)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(expenses, options);
            await File.WriteAllTextAsync(_expensesFilePath, jsonString);
        }

        public async Task<ObservableCollection<Expense>> LoadExpensesAsync()
        {
            if (!File.Exists(_expensesFilePath))
                return new ObservableCollection<Expense>();

            string jsonString = await File.ReadAllTextAsync(_expensesFilePath);
            if (string.IsNullOrEmpty(jsonString))
                return new ObservableCollection<Expense>();

            var expenses = JsonSerializer.Deserialize<ObservableCollection<Expense>>(jsonString);
            return expenses ?? new ObservableCollection<Expense>();
        }

        public async Task SaveIncomesAsync(ObservableCollection<Income> incomes)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(incomes, options);
            await File.WriteAllTextAsync(_incomesFilePath, jsonString);
        }

        public async Task<ObservableCollection<Income>> LoadIncomesAsync()
        {
            if (!File.Exists(_incomesFilePath))
                return new ObservableCollection<Income>();

            string jsonString = await File.ReadAllTextAsync(_incomesFilePath);
            if (string.IsNullOrEmpty(jsonString))
                return new ObservableCollection<Income>();

            var incomes = JsonSerializer.Deserialize<ObservableCollection<Income>>(jsonString);
            return incomes ?? new ObservableCollection<Income>();
        }

        public async Task SaveSavingAsync(Saving saving)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(saving, options);
            await File.WriteAllTextAsync(_savingsFilePath, jsonString);
        }

        public async Task<Saving> LoadSavingAsync()
        {
            if (!File.Exists(_savingsFilePath))
                return new Saving { Amount = 0 };

            string jsonString = await File.ReadAllTextAsync(_savingsFilePath);
            if (string.IsNullOrEmpty(jsonString))
                return new Saving { Amount = 0 };

            var saving = JsonSerializer.Deserialize<Saving>(jsonString);
            return saving ?? new Saving { Amount = 0 };
        }

        public async Task SaveCategorySettingsAsync(CategorySettings[] settings)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(settings, options);
            await File.WriteAllTextAsync(_categorySettingsFilePath, jsonString);
        }

        public async Task<CategorySettings[]> LoadCategorySettingsAsync()
        {
            if (!File.Exists(_categorySettingsFilePath))
                return CategorySettings.DefaultSettings;

            string jsonString = await File.ReadAllTextAsync(_categorySettingsFilePath);
            if (string.IsNullOrEmpty(jsonString))
                return CategorySettings.DefaultSettings;

            var settings = JsonSerializer.Deserialize<CategorySettings[]>(jsonString);
            return settings ?? CategorySettings.DefaultSettings;
        }

        // Szinkron betöltési metódusok
        public ObservableCollection<Expense> LoadExpenses()
        {
            if (!File.Exists(_expensesFilePath))
                return new ObservableCollection<Expense>();

            string jsonString = File.ReadAllText(_expensesFilePath);
            if (string.IsNullOrEmpty(jsonString))
                return new ObservableCollection<Expense>();

            var expenses = JsonSerializer.Deserialize<ObservableCollection<Expense>>(jsonString);
            return expenses ?? new ObservableCollection<Expense>();
        }

        public ObservableCollection<Income> LoadIncomes()
        {
            if (!File.Exists(_incomesFilePath))
                return new ObservableCollection<Income>();

            string jsonString = File.ReadAllText(_incomesFilePath);
            if (string.IsNullOrEmpty(jsonString))
                return new ObservableCollection<Income>();

            var incomes = JsonSerializer.Deserialize<ObservableCollection<Income>>(jsonString);
            return incomes ?? new ObservableCollection<Income>();
        }

        public Saving LoadSaving()
        {
            if (!File.Exists(_savingsFilePath))
                return new Saving { Amount = 0 };

            string jsonString = File.ReadAllText(_savingsFilePath);
            if (string.IsNullOrEmpty(jsonString))
                return new Saving { Amount = 0 };

            var saving = JsonSerializer.Deserialize<Saving>(jsonString);
            return saving ?? new Saving { Amount = 0 };
        }

        public CategorySettings[] LoadCategorySettings()
        {
            if (!File.Exists(_categorySettingsFilePath))
                return CategorySettings.DefaultSettings;

            string jsonString = File.ReadAllText(_categorySettingsFilePath);
            if (string.IsNullOrEmpty(jsonString))
                return CategorySettings.DefaultSettings;

            var settings = JsonSerializer.Deserialize<CategorySettings[]>(jsonString);
            return settings ?? CategorySettings.DefaultSettings;
        }
    }
}
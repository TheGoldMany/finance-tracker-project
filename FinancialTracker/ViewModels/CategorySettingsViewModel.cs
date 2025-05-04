using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using FinancialTracker.Models;

namespace FinancialTracker.ViewModels
{
    public class CategorySettingsViewModel : ViewModelBase
    {
        private ObservableCollection<CategorySettingItem> _settings;

        public CategorySettingsViewModel(CategorySettings[] settings)
        {
            // Beállítások inicializálása
            Settings = new ObservableCollection<CategorySettingItem>(
                settings.Select(s => new CategorySettingItem
                {
                    Category = s.Category,
                    Priority = s.Priority,
                    MaxPercentage = s.MaxPercentage
                }));

            // PropertyChanged esemény hozzáadása az elemekhez, hogy frissüljön a TotalPercentage
            foreach (var setting in Settings)
            {
                setting.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(CategorySettingItem.MaxPercentage))
                    {
                        OnPropertyChanged(nameof(TotalPercentage));
                        OnPropertyChanged(nameof(CanSave));
                    }
                };
            }

            // Parancsok inicializálása
            SaveCommand = new RelayCommand<Window>(param => Save(param), _ => CanSave);
            CancelCommand = new RelayCommand<Window>(param => Cancel(param));

            // Jelezzük a TotalPercentage változásait
            OnPropertyChanged(nameof(TotalPercentage));
            OnPropertyChanged(nameof(CanSave));
        }

        public ObservableCollection<CategorySettingItem> Settings
        {
            get => _settings;
            set
            {
                if (SetProperty(ref _settings, value))
                {
                    OnPropertyChanged(nameof(TotalPercentage));
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        // Az összesített százalék (csak olvasható)
        public decimal TotalPercentage => Settings?.Sum(s => s.MaxPercentage) ?? 0;

        // Csak akkor menthetünk, ha az összeg pontosan 100%
        public bool CanSave => Math.Abs(TotalPercentage - 100) < 0.01m;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        // Visszaadja a beállításokat a MainViewModel számára
        public CategorySettings[] GetSettings()
        {
            return Settings.Select(s => new CategorySettings
            {
                Category = s.Category,
                Priority = s.Priority,
                MaxPercentage = s.MaxPercentage
            }).ToArray();
        }

        // Mentés parancsa
        private void Save(Window window)
        {
            if (window != null)
            {
                window.DialogResult = true;
                window.Close();
            }
        }

        // Mégsem parancsa
        private void Cancel(Window window)
        {
            if (window != null)
            {
                window.DialogResult = false;
                window.Close();
            }
        }
    }
}
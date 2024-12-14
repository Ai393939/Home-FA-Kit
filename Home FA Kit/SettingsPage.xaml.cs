using Microsoft.Maui.Controls;
using BusinessLayer;
using DataLayer;

namespace Home_FA_Kit
{
    public partial class SettingsPage : ContentPage
    {
        private Settings _settings = new Settings();
        private PharmacyApp _pharmacyApp;

        public SettingsPage(PharmacyApp pharmacyApp)
        {
            InitializeComponent();
            _pharmacyApp = pharmacyApp;
            _settings = _pharmacyApp.AppSettings;
            LoadSettings();
        }

        private void LoadSettings()
        {
            LanguagePicker.SelectedIndex = _settings.Language == "ru" ? 0 : 1;
            ThemePicker.SelectedIndex = _settings.Theme == "Light" ? 0 : 1;
        }

        private void OnLanguageSelected(object sender, EventArgs e)
        {
            var selectedLanguage = LanguagePicker.SelectedIndex == 0 ? "ru" : "en";
            _settings.Language = selectedLanguage;

            _pharmacyApp.Categories = CategoryLoader.LoadCategories(selectedLanguage);

            LocalizeMedicineForms(selectedLanguage);
            UpdateMedicineCategories();
        }

        private void OnThemeSelected(object sender, EventArgs e)
        {
            var selectedTheme = ThemePicker.SelectedIndex == 0 ? "Light" : "Dark";
            _settings.Theme = selectedTheme;
        }

        private async void OnSaveSettingsClicked(object sender, EventArgs e)
        {
            PharmacyAppSaver.SaveToFile(_pharmacyApp, "pharmacyApp.json");

            if (_pharmacyApp.AppSettings.Language == "en")
            {
                DisplayAlert("Saved", "Settings are saved\n(if you updated the language please reset the app)", "OK");
            }
            else
            {
                DisplayAlert("Сохранено", "Настройки сохранены\n(если вы обновили язык, пожалуйста, перезапустите приложение)", "OK");
            }

            await Navigation.PopAsync();
            ApplyTheme(_settings.Theme);
        }

        private void ApplyTheme(string theme)
        {
            if (theme == "Light")
            {
                Application.Current.UserAppTheme = AppTheme.Light;
            }
            else if (theme == "Dark")
            {
                Application.Current.UserAppTheme = AppTheme.Dark;
            }
        }

        private void LocalizeMedicineForms(string culture)
        {
            foreach (var pharmacy in _pharmacyApp.Pharmacies)
            {
                foreach (var medicine in pharmacy.Medicines)
                {
                    medicine.Form = MedicineFormLocalization.GetLocalizedForm(medicine.FormIndex, culture);
                }
            }
        }

        private void UpdateMedicineCategories()
        {
            foreach (var pharmacy in _pharmacyApp.Pharmacies)
            {
                foreach (var medicine in pharmacy.Medicines)
                {
                    var category = _pharmacyApp.Categories.FirstOrDefault(c => c.Id == medicine.Category?.Id);
                    if (category != null)
                    {
                        medicine.Category.Name = category.Name;

                        if (medicine.Category.Subcategories != null && medicine.Category.Subcategories.Any())
                        {
                            var subcategory = category.Subcategories.FirstOrDefault(s => s.Id == medicine.Category.Subcategories[0].Id);
                            if (subcategory != null)
                            {
                                medicine.Category.Subcategories[0].Name = subcategory.Name;
                            }
                        }
                    }
                }
            }
        }
        private async void OnPharmaciesClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnMedicationClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MedicationPage(_pharmacyApp));
        }

        private async void OnStatisticsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StatisticsPage(_pharmacyApp));
        }

        private async void OnSettingsClicked(object sender, EventArgs e) { }
    }
}
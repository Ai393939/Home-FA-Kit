using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using BusinessLayer;
using DataLayer;

namespace Home_FA_Kit
{
    public partial class StatisticsPage : ContentPage
    {
        private PharmacyApp _pharmacyApp;

        public StatisticsPage(PharmacyApp pharmacyApp)
        {
            InitializeComponent();
            _pharmacyApp = pharmacyApp;
            BindingContext = _pharmacyApp;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _pharmacyApp.GetPharmacyStatistics();
        }

        private async void OnDeleteMedicineClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var medicine = button.BindingContext as Medicine;

            var result = await DisplayAlert("Подтверждение", "Хотите удалить это лекарство?", "Да", "Нет");
            if (result)
            {
                var pharmacyStats = _pharmacyApp.PharmacyStatistics.PharmacyStats
                    .FirstOrDefault(ps => ps.Pharmacy.Medicines.Contains(medicine));

                if (pharmacyStats != null)
                {
                    pharmacyStats.Pharmacy.RemoveMedicine(medicine);
                    pharmacyStats.ExpiredMedicines.Remove(medicine);

                    _pharmacyApp.GetPharmacyStatistics();
                    PharmacyAppSaver.SaveToFile(_pharmacyApp, "pharmacyApp.json");
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

        private async void OnStatisticsClicked(object sender, EventArgs e) { }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage(_pharmacyApp));
        }
    }
}
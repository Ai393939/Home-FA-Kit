using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using BusinessLayer;

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

        private async void OnPharmacyTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is PharmacyStatistics pharmacyStats)
            {
                var action = await DisplayActionSheet("Статистика", "Отмена", null, "Общее количество лекарств", "Лекарства с истекшим сроком", "Категории");

                switch (action)
                {
                    case "Общее количество лекарств":
                        await DisplayAlert("Статистика", $"Общее количество лекарств в аптечке '{pharmacyStats.Pharmacy.Name}': {pharmacyStats.TotalMedicines}", "OK");
                        break;
                    case "Лекарства с истекшим сроком":
                        await Navigation.PushAsync(new ExpiredMedicinesPage(pharmacyStats.ExpiredMedicines, _pharmacyApp, pharmacyStats.Pharmacy, pharmacyStats));
                        break;
                    case "Категории":
                        await DisplayAlert("Статистика", $"Количество категорий: {pharmacyStats.CategoryCount}\nКатегории в аптечке '{pharmacyStats.Pharmacy.Name}': {string.Join(", ", pharmacyStats.Categories)}", "OK");
                        break;
                }
            }
        }

        private async void OnStatisticTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is string selectedStatistic)
            {
                var pharmacyStats = (PharmacyStatistics)((ListView)sender).BindingContext;

                switch (selectedStatistic)
                {
                    case "Общее количество лекарств":
                        await DisplayAlert("Статистика", $"Общее количество лекарств в аптечке '{pharmacyStats.Pharmacy.Name}': {pharmacyStats.TotalMedicines}", "OK");
                        break;
                    case "Лекарства с истекшим сроком":
                        await Navigation.PushAsync(new ExpiredMedicinesPage(pharmacyStats.ExpiredMedicines, _pharmacyApp, pharmacyStats.Pharmacy, pharmacyStats));
                        break;
                    case "Категории":
                        await DisplayAlert("Статистика", $"Количество категорий: {pharmacyStats.CategoryCount}\nКатегории в аптечке '{pharmacyStats.Pharmacy.Name}': {string.Join(", ", pharmacyStats.Categories)}", "OK");
                        break;
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
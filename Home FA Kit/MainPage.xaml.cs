using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using BusinessLayer;
using DataLayer;
using Home_FA_Kit.Resources.Strings;
using System.Globalization;

namespace Home_FA_Kit
{
    public partial class MainPage : ContentPage
    {
        private PharmacyApp _pharmacyApp;
        private bool _isAscendingSort = true;

        public MainPage()
        {
            _pharmacyApp = PharmacyAppLoader.LoadFromFile("pharmacyApp.json") ?? new PharmacyApp();
            if (_pharmacyApp.AppSettings.Language == "en") AppResources.Culture = new CultureInfo("en");
            _pharmacyApp.Categories = CategoryLoader.LoadCategories(_pharmacyApp.AppSettings.Language);
            if (_pharmacyApp.AppSettings.Theme == "Dark") Application.Current.UserAppTheme = AppTheme.Dark;

            InitializeComponent();

            pharmaciesListView.ItemsSource = _pharmacyApp.Pharmacies;
            BindingContext = _pharmacyApp;
        }

        public void AddPharmacy(FirstAidKit pharmacy)
        {
            _pharmacyApp.AddPharmacy(pharmacy);
            UpdatePharmacy();
        }

        public void RemovePharmacy(FirstAidKit pharmacy)
        {
            _pharmacyApp.RemovePharmacy(pharmacy);
            UpdatePharmacy();
        }

        public void UpdatePharmacy()
        {
            PharmacyAppSaver.SaveToFile(_pharmacyApp, "pharmacyApp.json");
        }

        private async void OnPharmacyTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is FirstAidKit selectedPharmacy)
            {
                await Navigation.PushAsync(new MedicinesPage(selectedPharmacy, _pharmacyApp));
            }
        }

        private async void OnCreatePharmacyClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreatePharmacyPage(this));
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is FirstAidKit selectedPharmacy)
            {
                await Navigation.PushAsync(new EditPharmacyPage(this, selectedPharmacy));
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is FirstAidKit selectedPharmacy)
            {
                var result = await DisplayAlert("Подтверждение", "Вы уверены, что хотите удалить эту аптечку?", "Да", "Нет");
                if (result)
                {
                    RemovePharmacy(selectedPharmacy);
                }
            }
        }

        private void OnSortClicked(object sender, EventArgs e)
        {
            if (_isAscendingSort)
            {
                pharmaciesListView.ItemsSource = new ObservableCollection<FirstAidKit>(_pharmacyApp.Pharmacies.OrderBy(p => p.Name));
            }
            else
            {
                pharmaciesListView.ItemsSource = new ObservableCollection<FirstAidKit>(_pharmacyApp.Pharmacies.OrderByDescending(p => p.Name));
            }

            _isAscendingSort = !_isAscendingSort;
        }

        private void OnPharmaciesClicked(object sender, EventArgs e) { }

        private async void OnMedicationClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MedicationPage(_pharmacyApp));
        }

        private async void OnStatisticsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StatisticsPage(_pharmacyApp));
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage(_pharmacyApp));
        }
    }
}
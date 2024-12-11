using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using BusinessLayer;
using DataLayer;

namespace Home_FA_Kit
{
    public partial class ExpiredMedicinesPage : ContentPage
    {
        private ObservableCollection<Medicine> _expiredMedicines;
        private PharmacyApp _pharmacyApp;
        private FirstAidKit _pharmacy;
        private PharmacyStatistics _pharmacyStats;

        public ExpiredMedicinesPage(ObservableCollection<Medicine> expiredMedicines, PharmacyApp pharmacyApp, FirstAidKit pharmacy, PharmacyStatistics pharmacyStats)
        {
            InitializeComponent();
            _expiredMedicines = expiredMedicines;
            _pharmacyApp = pharmacyApp;
            _pharmacy = pharmacy;
            _pharmacyStats = pharmacyStats;
            BindingContext = this;
            medicinesListView.ItemsSource = _expiredMedicines;
        }

        private async void OnMedicineTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Medicine selectedMedicine)
            {
                var result = await DisplayAlert("Подтверждение", "Хотите удалить это лекарство?", "Да", "Нет");
                if (result)
                {
                    _expiredMedicines.Remove(selectedMedicine);
                    _pharmacy.RemoveMedicine(selectedMedicine);
                    _pharmacyApp.GetPharmacyStatistics();
                    PharmacyAppSaver.SaveToFile(_pharmacyApp, "pharmacyApp.json");
                }
            }
        }
    }
}
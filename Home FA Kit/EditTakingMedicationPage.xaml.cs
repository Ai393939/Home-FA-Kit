using System;
using Microsoft.Maui.Controls;
using BusinessLayer;
using DataLayer;

namespace Home_FA_Kit
{
    public partial class EditTakingMedicationPage : ContentPage
    {
        private PharmacyApp _pharmacyApp;
        private MedicationEvent _currentEvent;
        private MedicineTakenStatus _selectedMedicine;

        public EditTakingMedicationPage(PharmacyApp pharmacyApp, MedicationEvent currentEvent, MedicineTakenStatus selectedMedicine)
        {
            InitializeComponent();
            _pharmacyApp = pharmacyApp;
            _currentEvent = currentEvent;
            _selectedMedicine = selectedMedicine;

            medicineNameLabel.Text = selectedMedicine.Medicine.Name;
            timePicker.Time = selectedMedicine.Time.ToTimeSpan();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var newTime = TimeOnly.FromTimeSpan(timePicker.Time);
            _currentEvent.EditMedicineTime(_selectedMedicine.Medicine, newTime);
            PharmacyAppSaver.SaveToFile(_pharmacyApp, "pharmacyApp.json");

            await Navigation.PopAsync();
        }
    }
}
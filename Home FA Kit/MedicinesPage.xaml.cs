using System;
using System.Collections.ObjectModel;
using System.Linq;
using BusinessLayer;
using Microsoft.Maui.Controls;
using DataLayer;

namespace Home_FA_Kit
{
    public partial class MedicinesPage : ContentPage
    {
        private FirstAidKit _pharmacy;
        private PharmacyApp _pharmacyApp;
        private bool _isAscendingSort = true;
        private string _currentSortType = "По названию";

        public MedicinesPage(FirstAidKit pharmacy, PharmacyApp pharmacyApp)
        {
            InitializeComponent();

            _pharmacy = pharmacy;
            _pharmacyApp = pharmacyApp;
            medicinesListView.ItemsSource = _pharmacy.Medicines;

            foreach (var category in _pharmacyApp.Categories)
            {
                categoryPicker.Items.Add(category.Name);
            }

            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            medicinesListView.ItemsSource = _pharmacy.Medicines;
        }

        private void OnCategorySelected(object sender, EventArgs e)
        {
            var selectedCategory = categoryPicker.SelectedItem as string;

            if (categoryPicker.SelectedIndex == 0)
            {
                medicinesListView.ItemsSource = _pharmacy.Medicines;
            }
            else
            {
                var filteredMedicines = new ObservableCollection<Medicine>(_pharmacy.Medicines.Where(m => m.Category.Name == selectedCategory));
                medicinesListView.ItemsSource = filteredMedicines;
            }
        }

        public void AddMedicine(Medicine medicine)
        {
            _pharmacy.AddMedicine(medicine);
            UpdateMedicine();
        }

        public void RemoveMedicine(Medicine medicine)
        {
            _pharmacy.RemoveMedicine(medicine);
            UpdateMedicine();
        }

        public void UpdateMedicine()
        {
            PharmacyAppSaver.SaveToFile(_pharmacyApp, "pharmacyApp.json");
            medicinesListView.ItemsSource = _pharmacy.Medicines;
        }

        private async void OnCreateMedicineClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateMedicinePage(this, _pharmacyApp, _pharmacyApp.AppSettings.Language, _pharmacyApp.Categories));
        }

        private async void OnMedicineTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Medicine selectedMedicine)
            {
                await Navigation.PushAsync(new OneMedicinePage(_pharmacyApp, this, selectedMedicine, _pharmacyApp.AppSettings.Language, _pharmacyApp.Categories));
            }
        }

        private async void OnEllipsisClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Medicine selectedMedicine)
            {
                var result = await DisplayAlert("Подтверждение", "Вы уверены, что хотите удалить это лекарство?", "Да", "Нет");
                if (result)
                {
                    RemoveMedicine(selectedMedicine);
                }
            }
        }

        private async void OnSortClicked(object sender, EventArgs e)
        {
            var sortType = await DisplayActionSheet("Выберите тип сортировки", "Отмена", null, "По названию", "По цене", "По дате истечения", "По активному веществу", "По производителю", "По стране", "По категории");

            if (sortType == "Отмена")
                return;

            _currentSortType = sortType;

            IComparer<Medicine> comparer = null;

            switch (sortType)
            {
                case "По названию":
                    comparer = new CompareMedicineByName();
                    break;
                case "По цене":
                    comparer = new CompareMedicineByCost();
                    break;
                case "По дате истечения":
                    comparer = new CompareMedicineByExpirationDate();
                    break;
                case "По активному веществу":
                    comparer = new CompareMedicineByActiveIngredient();
                    break;
                case "По производителю":
                    comparer = new CompareMedicineByManufacturer();
                    break;
                case "По стране":
                    comparer = new CompareMedicineByCountry();
                    break;
                case "По категории":
                    comparer = new CompareMedicineByCategory();
                    break;
            }

            if (comparer != null)
            {
                if (_isAscendingSort)
                {
                    medicinesListView.ItemsSource = new ObservableCollection<Medicine>(_pharmacy.Medicines.OrderBy(m => m, comparer));
                }
                else
                {
                    medicinesListView.ItemsSource = new ObservableCollection<Medicine>(_pharmacy.Medicines.OrderByDescending(m => m, comparer));
                }

                _isAscendingSort = !_isAscendingSort;
            }
        }

        private void OnSortDirectionClicked(object sender, EventArgs e)
        {
            _isAscendingSort = !_isAscendingSort;

            IComparer<Medicine> comparer = null;

            switch (_currentSortType)
            {
                case "По названию":
                    comparer = new CompareMedicineByName();
                    break;
                case "По цене":
                    comparer = new CompareMedicineByCost();
                    break;
                case "По дате истечения":
                    comparer = new CompareMedicineByExpirationDate();
                    break;
                case "По активному веществу":
                    comparer = new CompareMedicineByActiveIngredient();
                    break;
                case "По производителю":
                    comparer = new CompareMedicineByManufacturer();
                    break;
                case "По стране":
                    comparer = new CompareMedicineByCountry();
                    break;
                case "По категории":
                    comparer = new CompareMedicineByCategory();
                    break;
            }

            if (comparer != null)
            {
                if (_isAscendingSort)
                {
                    medicinesListView.ItemsSource = new ObservableCollection<Medicine>(_pharmacy.Medicines.OrderBy(m => m, comparer));
                }
                else
                {
                    medicinesListView.ItemsSource = new ObservableCollection<Medicine>(_pharmacy.Medicines.OrderByDescending(m => m, comparer));
                }
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = e.NewTextValue;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                medicinesListView.ItemsSource = _pharmacy.Medicines;
                return;
            }

            searchText = searchText.ToLower();

            var filteredMedicines = new ObservableCollection<Medicine>(_pharmacy.Medicines.Where(m => m.ToString().ToLower().Contains(searchText)));

            medicinesListView.ItemsSource = filteredMedicines;
        }

        private void OnResetFilterClicked(object sender, EventArgs e)
        {
            searchEntry.Text = string.Empty;
            medicinesListView.ItemsSource = _pharmacy.Medicines;
        }

        private void OnDecreaseQuantityClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Medicine medicine)
            {
                if (medicine.Quantity > 0)
                {
                    medicine.Quantity--;
                    UpdateMedicine();
                }
            }
        }

        private void OnIncreaseQuantityClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Medicine medicine)
            {
                medicine.Quantity++;
                UpdateMedicine();
            }
        }
    }
}
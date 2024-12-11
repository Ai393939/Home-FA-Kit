using System;
using System.Collections.ObjectModel;
using System.Linq;
using BusinessLayer;
using Microsoft.Maui.Controls;
using DataLayer;
using System.Globalization;

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

            BindingContext = this;
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

        private async void OnMedicineTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Medicine selectedMedicine)
            {
                await Navigation.PushAsync(new OneMedicinePage(_pharmacyApp, this, selectedMedicine));
            }
        }

        private async void OnCreateMedicineClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateMedicinePage(this, _pharmacyApp));
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

        private async void OnSearchClicked(object sender, EventArgs e)
        {
            var searchText = searchEntry.Text;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                medicinesListView.ItemsSource = _pharmacy.Medicines;
                return;
            }

            searchText = searchEntry.Text.ToLower();

            var searchType = await DisplayActionSheet("Выберите тип поиска", "Отмена", null, "По названию", "По цене", "По дате истечения", "По активному веществу", "По производителю", "По стране", "По категории");

            ObservableCollection<Medicine> filteredMedicines;

            switch (searchType)
            {
                case "По названию":
                    filteredMedicines = new ObservableCollection<Medicine>(_pharmacy.Medicines.Where(m => m.Name.ToLower().Contains(searchText)));
                    break;
                case "По цене":
                    if (int.TryParse(searchText, out int cost))
                    {
                        filteredMedicines = new ObservableCollection<Medicine>(_pharmacy.Medicines.Where(m => m.Cost == cost));
                    }
                    else
                    {
                        await DisplayAlert("Ошибка", "Введите корректную цену", "OK");
                        return;
                    }
                    break;
                case "По дате истечения":
                    if (DateTime.TryParse(searchText, out DateTime expirationDate))
                    {
                        filteredMedicines = new ObservableCollection<Medicine>(_pharmacy.Medicines.Where(m => m.ExpirationDate == expirationDate));
                    }
                    else
                    {
                        await DisplayAlert("Ошибка", "Введите корректную дату истечения", "OK");
                        return;
                    }
                    break;
                case "По активному веществу":
                    filteredMedicines = new ObservableCollection<Medicine>(_pharmacy.Medicines.Where(m => m.ActiveIngredient.ToLower().Contains(searchText)));
                    break;
                case "По производителю":
                    filteredMedicines = new ObservableCollection<Medicine>(_pharmacy.Medicines.Where(m => m.Manufacturer.ToLower().Contains(searchText)));
                    break;
                case "По стране":
                    filteredMedicines = new ObservableCollection<Medicine>(_pharmacy.Medicines.Where(m => m.Country.ToLower().Contains(searchText)));
                    break;
                case "По категории":
                    filteredMedicines = new ObservableCollection<Medicine>(_pharmacy.Medicines.Where(m => m.Category?.Name.ToLower().Contains(searchText) == true));
                    break;
                default:
                    return;
            }

            medicinesListView.ItemsSource = filteredMedicines;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                medicinesListView.ItemsSource = _pharmacy.Medicines;
            }
        }

        private void OnResetFilterClicked(object sender, EventArgs e)
        {
            searchEntry.Text = string.Empty;
            medicinesListView.ItemsSource = _pharmacy.Medicines;
        }

        private Color GetExpirationDateColor(DateTime expirationDate)
        {
            return expirationDate < DateTime.Now ? Colors.Red : Colors.Transparent;
        }
    }
}
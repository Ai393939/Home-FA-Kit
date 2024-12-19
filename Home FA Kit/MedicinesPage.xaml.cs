using System;
using System.Collections.ObjectModel;
using System.Linq;
using BusinessLayer;
using Microsoft.Maui.Controls;
using DataLayer;
using Home_FA_Kit.Resources.Strings;

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

        public FirstAidKit Pharmacy => _pharmacy;

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
                var filteredMedicines = new ObservableCollection<Medicine>(_pharmacy.Medicines
                    .Where(m => m.Category != null && m.Category.Name == selectedCategory));
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
                await Navigation.PushAsync(new EditMedicinePage(_pharmacyApp, this, selectedMedicine, _pharmacyApp.AppSettings.Language, _pharmacyApp.Categories));
            }
        }

        private async void OnEllipsisClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Medicine selectedMedicine)
            {
                if (AppResources.Culture != null && AppResources.Culture.Name == "en")
                {
                    var result = await DisplayAlert("Confirmation", "Are you sure you want to delete this medicine?", "Yes", "No");
                    if (result)
                    {
                        RemoveMedicine(selectedMedicine);
                    }
                }
                else
                {
                    var result = await DisplayAlert("Подтверждение", "Вы уверены, что хотите удалить это лекарство?", "Да", "Нет");
                    if (result)
                    {
                        RemoveMedicine(selectedMedicine);
                    }
                }
            }
        }

        private async void OnSortClicked(object sender, EventArgs e)
        {
            string sortType = null;

            if (AppResources.Culture != null && AppResources.Culture.Name == "en")
            {
                sortType = await DisplayActionSheet("Select sorting type", "Cancel", null, "By name", "By price", "By expiration date", "By active ingredient", "By manufacturer", "By country", "By category");
            }
            else
            {
                sortType = await DisplayActionSheet("Выберите тип сортировки", "Отмена", null, "По названию", "По цене", "По дате истечения", "По активному веществу", "По производителю", "По стране", "По категории");
            }

            if (sortType == "Cancel")
                return;

            _currentSortType = sortType;

            IComparer<Medicine> comparer = null;

            switch (sortType)
            {
                case "By name":
                case "По названию":
                    comparer = new CompareMedicineByName();
                    break;
                case "By price":
                case "По цене":
                    comparer = new CompareMedicineByCost();
                    break;
                case "By expiration date":
                case "По дате истечения":
                    comparer = new CompareMedicineByExpirationDate();
                    break;
                case "By active ingredient":
                case "По активному веществу":
                    comparer = new CompareMedicineByActiveIngredient();
                    break;
                case "By manufacturer":
                case "По производителю":
                    comparer = new CompareMedicineByManufacturer();
                    break;
                case "By country":
                case "По стране":
                    comparer = new CompareMedicineByCountry();
                    break;
                case "By category":
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

        private async void OnSortDirectionClicked(object sender, EventArgs e)
        {
            string sortDirection = null;

            if (AppResources.Culture != null && AppResources.Culture.Name == "en")
            {
                sortDirection = await DisplayActionSheet("Select sorting direction", "Cancel", null, "Ascending", "Descending");
            }
            else
            {
                sortDirection = await DisplayActionSheet("Выберите направление сортировки", "Отмена", null, "По возрастанию", "По убыванию");
            }

            if (sortDirection == "Cancel")
                return;

            _isAscendingSort = sortDirection == "Ascending" || sortDirection == "По возрастанию";

            IComparer<Medicine> comparer = null;

            switch (_currentSortType)
            {
                case "By name":
                case "По названию":
                    comparer = new CompareMedicineByName();
                    break;
                case "By price":
                case "По цене":
                    comparer = new CompareMedicineByCost();
                    break;
                case "By expiration date":
                case "По дате истечения":
                    comparer = new CompareMedicineByExpirationDate();
                    break;
                case "By active ingredient":
                case "По активному веществу":
                    comparer = new CompareMedicineByActiveIngredient();
                    break;
                case "By manufacturer":
                case "По производителю":
                    comparer = new CompareMedicineByManufacturer();
                    break;
                case "By country":
                case "По стране":
                    comparer = new CompareMedicineByCountry();
                    break;
                case "By category":
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

        private async void OnResetFilterClicked(object sender, EventArgs e)
        {
            string resetConfirmation = null;

            if (AppResources.Culture != null && AppResources.Culture.Name == "en")
            {
                resetConfirmation = await DisplayActionSheet("Reset filter", "Cancel", null, "Reset");
            }
            else
            {
                resetConfirmation = await DisplayActionSheet("Сбросить фильтр", "Отмена", null, "Сбросить");
            }

            if (resetConfirmation == "Reset" || resetConfirmation == "Сбросить")
            {
                searchEntry.Text = string.Empty;
                medicinesListView.ItemsSource = _pharmacy.Medicines;
            }
        }

        private async void OnDecreaseQuantityClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Medicine medicine)
            {
                if (medicine.Quantity > 0)
                {
                    string confirmation = null;

                    if (AppResources.Culture != null && AppResources.Culture.Name == "en")
                    {
                        confirmation = await DisplayActionSheet("Decrease quantity", "Cancel", null, "Decrease");
                    }
                    else
                    {
                        confirmation = await DisplayActionSheet("Уменьшить количество", "Отмена", null, "Уменьшить");
                    }

                    if (confirmation == "Decrease" || confirmation == "Уменьшить")
                    {
                        medicine.Quantity--;
                        UpdateMedicine();
                    }
                }
            }
        }

        private async void OnIncreaseQuantityClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Medicine medicine)
            {
                string confirmation = null;

                if (AppResources.Culture != null && AppResources.Culture.Name == "en")
                {
                    confirmation = await DisplayActionSheet("Increase quantity", "Cancel", null, "Increase");
                }
                else
                {
                    confirmation = await DisplayActionSheet("Увеличить количество", "Отмена", null, "Увеличить");
                }

                if (confirmation == "Increase" || confirmation == "Увеличить")
                {
                    medicine.Quantity++;
                    UpdateMedicine();
                }
            }
        }
    }
}
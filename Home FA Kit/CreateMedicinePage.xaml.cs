using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using BusinessLayer;
using DataLayer;

namespace Home_FA_Kit
{
    public partial class CreateMedicinePage : ContentPage
    {
        private MedicinesPage _medicinesPage;
        private PharmacyApp _pharmacyApp;
        private bool _isMedicineSaved = false;
        private List<Category> _categories;

        public CreateMedicinePage(MedicinesPage medicinesPage, PharmacyApp pharmacyApp)
        {
            InitializeComponent();
            _medicinesPage = medicinesPage;
            _pharmacyApp = pharmacyApp;

            if (_pharmacyApp.AppSettings.Language == "ru")
            {
                _categories = CategoryLoader.LoadCategories("categories.json");
            } else
            {
                _categories = CategoryLoader.LoadCategories("categoriesEn.json");
            }

            foreach (var category in _categories)
            {
                categoryPicker.Items.Add(category.Name);
            }
        }

        private void OnCategorySelected(object sender, EventArgs e)
        {
            subCategoryPicker.Items.Clear();
            if (categoryPicker.SelectedIndex != -1)
            {
                var selectedCategory = _categories[categoryPicker.SelectedIndex];
                foreach (var subCategory in selectedCategory.Subcategories)
                {
                    subCategoryPicker.Items.Add(subCategory.Name);
                }
            }
        }

        private async void OnSaveMedicineClicked(object sender, EventArgs e)
        {
            var medicineName = medicineNameEntry.Text;
            if (!string.IsNullOrEmpty(medicineName))
            {
                var newMedicine = new Medicine
                {
                    Name = medicineName,
                    Description = medicineDescriptionEntry.Text,
                    Cost = int.TryParse(medicineCostEntry.Text, out int cost) ? cost : 0,
                    Quantity = int.TryParse(medicineQuantityEntry.Text, out int quantity) ? quantity : 0,
                    ExpirationDate = DateTime.TryParse(medicineExpirationDateEntry.Text, out DateTime expirationDate) ? expirationDate : DateTime.Now,
                    ActiveIngredient = medicineActiveIngredientEntry.Text,
                    Manufacturer = medicineManufacturerEntry.Text,
                    Country = medicineCountryEntry.Text,
                    PharmacologicalEffect = medicinePharmacologicalEffectEntry.Text,
                    Form = medicineFormEntry.Text,
                    Note = medicineNoteEntry.Text,
                    Category = new Category
                    {
                        Name = categoryPicker.SelectedItem.ToString(),
                        Subcategories = new List<Subcategory>
                        {
                            new Subcategory { Name = subCategoryPicker.SelectedItem.ToString() }
                        }
                    }
                };

                _medicinesPage.AddMedicine(newMedicine);
                _isMedicineSaved = true;
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Ошибка", "Название лекарства не может быть пустым", "OK");
            }
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            if (!_isMedicineSaved && !string.IsNullOrEmpty(medicineNameEntry.Text))
            {
                var result = await DisplayAlert("Предупреждение", "Вы хотите удалить изменения?", "Да", "Нет");
                if (result)
                {
                    await Navigation.PushAsync(this);
                }
            }
        }
    }
}
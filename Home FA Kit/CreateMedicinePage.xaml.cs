using System;
using System.Collections.Generic;
using BusinessLayer;
using DataLayer;
using Home_FA_Kit.Resources.Strings;
using Microsoft.Maui.Controls;

namespace Home_FA_Kit
{
    public partial class CreateMedicinePage : ContentPage
    {
        private MedicinesPage _medicinesPage;
        private PharmacyApp _pharmacyApp;
        private bool _isMedicineSaved = false;
        private List<Category> _categories;
        private string _currentLanguage;
        private List<string> _forms;

        public CreateMedicinePage(MedicinesPage medicinesPage, PharmacyApp pharmacyApp, string currentLanguage, List<Category> categories)
        {
            InitializeComponent();
            _medicinesPage = medicinesPage;
            _pharmacyApp = pharmacyApp;
            _currentLanguage = currentLanguage;
            _categories = categories;

            foreach (var category in _categories)
            {
                categoryPicker.Items.Add(category.Name);
            }

            _forms = MedicineFormLocalization.GetAllForms(_currentLanguage);
            foreach (var form in _forms)
            {
                medicineFormPicker.Items.Add(form);
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
            var expirationDateText = medicineExpirationDateEntry.Text;
            var quantityText = medicineQuantityEntry.Text;
            var selectedForm = medicineFormPicker.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(medicineName))
            {
                if (AppResources.Culture != null && AppResources.Culture.Name == "en")
                {
                    await DisplayAlert("Error", "Medicine name cannot be empty", "OK");
                }
                else
                {
                    await DisplayAlert("Ошибка", "Название лекарства не может быть пустым", "OK");
                }
                return;
            }

            if (string.IsNullOrEmpty(expirationDateText) || !DateTime.TryParse(expirationDateText, out _))
            {
                if (AppResources.Culture != null && AppResources.Culture.Name == "en")
                {
                    await DisplayAlert("Error", "Expiration date must be specified and in the correct format (e.g., 01.01.2023)", "OK");
                }
                else
                {
                    await DisplayAlert("Ошибка", "Срок годности должен быть указан и иметь правильный формат (например, 01.01.2023)", "OK");
                }
                return;
            }

            if (string.IsNullOrEmpty(quantityText) || !int.TryParse(quantityText, out _))
            {
                if (AppResources.Culture != null && AppResources.Culture.Name == "en")
                {
                    await DisplayAlert("Error", "Quantity must be specified and be an integer", "OK");
                }
                else
                {
                    await DisplayAlert("Ошибка", "Количество должно быть указано и быть целым числом", "OK");
                }
                return;
            }

            if (string.IsNullOrEmpty(selectedForm))
            {
                if (AppResources.Culture != null && AppResources.Culture.Name == "en")
                {
                    await DisplayAlert("Error", "Medicine form must be selected", "OK");
                }
                else
                {
                    await DisplayAlert("Ошибка", "Форма лекарства должна быть выбрана", "OK");
                }
                return;
            }

            var formIndex = MedicineFormLocalization.GetFormIndex(selectedForm, _currentLanguage);

            var selectedCategoryIndex = categoryPicker.SelectedIndex;
            var selectedSubCategoryIndex = subCategoryPicker.SelectedIndex;

            if (selectedCategoryIndex != -1 && selectedSubCategoryIndex == -1)
            {
                if (AppResources.Culture != null && AppResources.Culture.Name == "en")
                {
                    await DisplayAlert("Error", "Medicine subcategory must be selected", "OK");
                }
                else
                {
                    await DisplayAlert("Ошибка", "Подкатегория лекарства должна быть выбрана", "OK");
                }
                return;
            }

            Category selectedCategory = null;
            Subcategory selectedSubCategory = null;

            if (selectedCategoryIndex != -1 && selectedSubCategoryIndex != -1)
            {
                selectedCategory = _categories[selectedCategoryIndex];
                selectedSubCategory = selectedCategory.Subcategories[selectedSubCategoryIndex];
            }

            Category category = null;

            if (selectedCategory != null)
            {
                category = new Category
                {
                    Id = selectedCategory.Id,
                    Name = selectedCategory.Name,
                    Subcategories = new List<Subcategory>
                    {
                        new Subcategory { Id = selectedSubCategory.Id, Name = selectedSubCategory.Name }
                    }
                };
            }

            var newMedicine = new Medicine
            {
                Name = medicineName,
                Description = medicineDescriptionEntry.Text,
                Cost = float.TryParse(medicineCostEntry.Text, out float cost) ? cost : 0,
                Quantity = int.TryParse(medicineQuantityEntry.Text, out int quantity) ? quantity : 0,
                ExpirationDate = DateTime.TryParse(medicineExpirationDateEntry.Text, out DateTime expirationDate) ? expirationDate : DateTime.Now,
                ActiveIngredient = medicineActiveIngredientEntry.Text,
                Manufacturer = medicineManufacturerEntry.Text,
                Country = medicineCountryEntry.Text,
                PharmacologicalEffect = medicinePharmacologicalEffectEntry.Text,
                FormIndex = formIndex,
                Form = selectedForm,
                Note = medicineNoteEntry.Text,
                Category = category
            };

            _medicinesPage.AddMedicine(newMedicine);
            _isMedicineSaved = true;
            await Navigation.PopAsync();
        }
    }
}
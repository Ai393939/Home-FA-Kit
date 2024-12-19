using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer;
using DataLayer;
using Home_FA_Kit.Resources.Strings;
using Microsoft.Maui.Controls;

namespace Home_FA_Kit
{
    public partial class EditMedicinePage : ContentPage
    {
        private PharmacyApp _pharmacyApp;
        private MedicinesPage _medicinesPage;
        private Medicine _originalMedicine;
        private bool _isMedicineSaved = false;
        private List<Category> _categories;
        private List<string> _forms;
        private string _currentLanguage;

        public EditMedicinePage(PharmacyApp pharmacyApp, MedicinesPage medicinesPage, Medicine originalMedicine, string currentLanguage, List<Category> categories)
        {
            InitializeComponent();
            _medicinesPage = medicinesPage;
            _originalMedicine = originalMedicine;
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

            medicineNameEntry.Text = originalMedicine.Name;
            medicineDescriptionEntry.Text = originalMedicine.Description;
            medicineCostEntry.Text = originalMedicine.Cost.ToString();
            medicineQuantityEntry.Text = originalMedicine.Quantity.ToString();
            medicineExpirationDateEntry.Text = originalMedicine.ExpirationDate.ToString("dd.MM.yyyy");
            medicineActiveIngredientEntry.Text = originalMedicine.ActiveIngredient;
            medicineManufacturerEntry.Text = originalMedicine.Manufacturer;
            medicineCountryEntry.Text = originalMedicine.Country;
            medicinePharmacologicalEffectEntry.Text = originalMedicine.PharmacologicalEffect;
            medicineNoteEntry.Text = originalMedicine.Note;

            medicineFormPicker.SelectedItem = originalMedicine.Form;

            categoryPicker.SelectedItem = originalMedicine.Category?.Name;
            OnCategorySelected(null, null);
            subCategoryPicker.SelectedItem = originalMedicine.Category?.Subcategories?.FirstOrDefault()?.Name;
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
                subCategoryPicker.SelectedItem = _originalMedicine.Category?.Subcategories?.FirstOrDefault()?.Name;
            }
        }

        private async void OnSaveMedicineClicked(object sender, EventArgs e)
        {
            var newMedicineName = medicineNameEntry.Text;
            var expirationDateText = medicineExpirationDateEntry.Text;
            var quantityText = medicineQuantityEntry.Text;
            var selectedForm = medicineFormPicker.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(newMedicineName))
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

            _originalMedicine.Name = newMedicineName;
            _originalMedicine.Description = medicineDescriptionEntry.Text;
            _originalMedicine.Cost = float.TryParse(medicineCostEntry.Text, out float cost) ? cost : 0;
            _originalMedicine.Quantity = int.TryParse(medicineQuantityEntry.Text, out int quantity) ? quantity : 0;
            _originalMedicine.ExpirationDate = DateTime.TryParse(medicineExpirationDateEntry.Text, out DateTime expirationDate) ? expirationDate : DateTime.Now;
            _originalMedicine.ActiveIngredient = medicineActiveIngredientEntry.Text;
            _originalMedicine.Manufacturer = medicineManufacturerEntry.Text;
            _originalMedicine.Country = medicineCountryEntry.Text;
            _originalMedicine.PharmacologicalEffect = medicinePharmacologicalEffectEntry.Text;
            _originalMedicine.FormIndex = formIndex;
            _originalMedicine.Form = selectedForm;
            _originalMedicine.Note = medicineNoteEntry.Text;

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

            if (selectedCategoryIndex != -1 && selectedSubCategoryIndex != -1)
            {
                var selectedCategory = _categories[selectedCategoryIndex];
                var selectedSubCategory = selectedCategory.Subcategories[selectedSubCategoryIndex];

                _originalMedicine.Category = new Category
                {
                    Id = selectedCategory.Id,
                    Name = selectedCategory.Name,
                    Subcategories = new List<Subcategory>
                    {
                        new Subcategory { Id = selectedSubCategory.Id, Name = selectedSubCategory.Name }
                    }
                };
            }
            else
            {
                _originalMedicine.Category = null;
            }

            _medicinesPage.UpdateMedicine();

            _isMedicineSaved = true;
            await Navigation.PopAsync();
        }
    }
}
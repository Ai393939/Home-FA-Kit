using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer;
using DataLayer;
using Microsoft.Maui.Controls;

namespace Home_FA_Kit
{
    public partial class OneMedicinePage : ContentPage
    {
        private PharmacyApp _pharmacyApp;
        private MedicinesPage _medicinesPage;
        private Medicine _originalMedicine;
        private bool _isMedicineSaved = false;
        private List<Category> _categories;
        private List<string> _forms;
        private string _currentLanguage;

        public OneMedicinePage(PharmacyApp pharmacyApp, MedicinesPage medicinesPage, Medicine originalMedicine, string currentLanguage, List<Category> categories)
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

            // Проверка на пустое название
            if (string.IsNullOrEmpty(newMedicineName))
            {
                await DisplayAlert("Ошибка", "Название лекарства не может быть пустым", "OK");
                return;
            }

            // Проверка на пустой срок годности или некорректный формат
            if (string.IsNullOrEmpty(expirationDateText) || !DateTime.TryParse(expirationDateText, out _))
            {
                await DisplayAlert("Ошибка", "Срок годности должен быть указан и иметь правильный формат (например, 01.01.2023)", "OK");
                return;
            }

            // Проверка на пустое количество или некорректное значение
            if (string.IsNullOrEmpty(quantityText) || !int.TryParse(quantityText, out _))
            {
                await DisplayAlert("Ошибка", "Количество должно быть указано и быть целым числом", "OK");
                return;
            }

            // Проверка на пустую форму
            if (string.IsNullOrEmpty(selectedForm))
            {
                await DisplayAlert("Ошибка", "Форма лекарства должна быть выбрана", "OK");
                return;
            }

            // Получаем индекс формы
            var formIndex = MedicineFormLocalization.GetFormIndex(selectedForm, _currentLanguage);

            // Обновляем свойства оригинального лекарства
            _originalMedicine.Name = newMedicineName;
            _originalMedicine.Description = medicineDescriptionEntry.Text;
            _originalMedicine.Cost = int.TryParse(medicineCostEntry.Text, out int cost) ? cost : 0;
            _originalMedicine.Quantity = int.TryParse(medicineQuantityEntry.Text, out int quantity) ? quantity : 0;
            _originalMedicine.ExpirationDate = DateTime.TryParse(medicineExpirationDateEntry.Text, out DateTime expirationDate) ? expirationDate : DateTime.Now;
            _originalMedicine.ActiveIngredient = medicineActiveIngredientEntry.Text;
            _originalMedicine.Manufacturer = medicineManufacturerEntry.Text;
            _originalMedicine.Country = medicineCountryEntry.Text;
            _originalMedicine.PharmacologicalEffect = medicinePharmacologicalEffectEntry.Text;
            _originalMedicine.FormIndex = formIndex;
            _originalMedicine.Form = selectedForm;
            _originalMedicine.Note = medicineNoteEntry.Text;

            // Получаем выбранную категорию и подкатегорию
            var selectedCategoryIndex = categoryPicker.SelectedIndex;
            var selectedSubCategoryIndex = subCategoryPicker.SelectedIndex;

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

            // Обновляем лекарство на странице MedicinesPage
            _medicinesPage.UpdateMedicine();

            _isMedicineSaved = true;
            await Navigation.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            if (!_isMedicineSaved && !string.IsNullOrEmpty(medicineNameEntry.Text) && medicineNameEntry.Text != _originalMedicine.Name)
            {
                ShowUnsavedChangesAlert();
                return true;
            }
            return base.OnBackButtonPressed();
        }

        private async void ShowUnsavedChangesAlert()
        {
            var result = await DisplayAlert("Предупреждение", "У вас остались несохранённые изменения. Всё равно выйти?", "Да", "Нет");
            if (result)
            {
                await Navigation.PopAsync();
            }
        }
    }
}
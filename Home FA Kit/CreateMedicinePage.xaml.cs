using System;
using System.Collections.Generic;
using BusinessLayer;
using DataLayer;
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
            _categories = categories; // Принимаем категории из MedicinesPage

            // Загрузка категорий
            foreach (var category in _categories)
            {
                categoryPicker.Items.Add(category.Name);
            }

            // Загрузка форм в Picker с локализацией
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

            // Проверка на пустое название
            if (string.IsNullOrEmpty(medicineName))
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

            // Получаем выбранную категорию и подкатегорию
            var selectedCategoryIndex = categoryPicker.SelectedIndex;
            var selectedSubCategoryIndex = subCategoryPicker.SelectedIndex;

            Category selectedCategory = null;
            Subcategory selectedSubCategory = null;

            if (selectedCategoryIndex != -1 && selectedSubCategoryIndex != -1)
            {
                selectedCategory = _categories[selectedCategoryIndex];
                selectedSubCategory = selectedCategory.Subcategories[selectedSubCategoryIndex];
            }

            // Создаем новое лекарство
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
                FormIndex = formIndex, // Сохраняем индекс формы
                Form = selectedForm, // Сохраняем локализованное значение формы
                Note = medicineNoteEntry.Text,
                Category = selectedCategory != null ? new Category
                {
                    Id = selectedCategory.Id, // Сохраняем идентификатор категории
                    Name = selectedCategory.Name,
                    Subcategories = new List<Subcategory>
                        {
                            new Subcategory { Id = selectedSubCategory.Id, Name = selectedSubCategory.Name } 
                        }
                    } : null
                };

            // Добавляем лекарство на страницу MedicinesPage
            _medicinesPage.AddMedicine(newMedicine);
            _isMedicineSaved = true;
            await Navigation.PopAsync();
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
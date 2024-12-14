using System.Collections.Generic;

namespace DataLayer
{
    public static class MedicineFormLocalization
    {
        // Список русских значений форм
        private static readonly List<string> _russianForms = new List<string>
        {
            "шт",
            "уп",
            "пакет",
            "табл",
            "г",
            "мг",
            "мл",
            "амп",
            "капс"
        };

        // Список английских значений форм
        private static readonly List<string> _englishForms = new List<string>
        {
            "pcs",
            "pack",
            "package",
            "tablet",
            "g",
            "mg",
            "ml",
            "ampoule",
            "capsule"
        };

        public static string GetLocalizedForm(int formIndex, string culture)
        {
            if (formIndex < 0 || formIndex >= _russianForms.Count)
                return string.Empty;

            return culture == "ru" ? _russianForms[formIndex] : _englishForms[formIndex];
        }

        public static int GetFormIndex(string localizedForm, string culture)
        {
            if (culture == "ru")
            {
                return _russianForms.IndexOf(localizedForm);
            }
            else
            {
                return _englishForms.IndexOf(localizedForm);
            }
        }

        public static List<string> GetAllForms(string culture)
        {
            return culture == "ru" ? _russianForms : _englishForms;
        }
    }
}
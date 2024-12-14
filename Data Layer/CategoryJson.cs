using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BusinessLayer;

namespace DataLayer
{
    public static class CategoryLoader
    {
        public static List<Category> LoadCategories(string language)
        {
            string filePath = GetCategoryFilePath(language);

            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<Category>>(jsonString);
            }

            return new List<Category>();
        }

        private static string GetCategoryFilePath(string language)
        {
            return language == "ru" ? "categories.json" : "categoriesEn.json";
        }
    }
}
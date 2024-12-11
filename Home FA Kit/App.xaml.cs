using BusinessLayer;
using DataLayer;
using Home_FA_Kit.Resources.Strings;
using System.Globalization;

namespace Home_FA_Kit
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
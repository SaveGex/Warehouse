using Warehouse.Auxiliary;

namespace Warehouse
{
    public partial class App : Application
    {
        public App(MainPage mainPage)
        {
            InitializeComponent();
            
            MainPage = mainPage;

            MainPage = new AppShell();
        }
    }
}

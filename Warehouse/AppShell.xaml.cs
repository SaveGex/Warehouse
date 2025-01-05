
namespace Warehouse
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("AddElement", typeof(AddElement));
            Routing.RegisterRoute("MainPage", typeof(MainPage));


        }

        private async void OpenModalAdd(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("AddElement");
        }
    }
}

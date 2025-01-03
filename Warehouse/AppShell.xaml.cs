
namespace Warehouse
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("ModalPageAdd", typeof(AddElement));
        }

        private async void OpenModalAdd(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("ModalPageAdd");
        }
    }
}

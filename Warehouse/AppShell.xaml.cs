
using Warehouse.Views;

namespace Warehouse
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddIndefinedElement), typeof(AddIndefinedElement));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(ChooseTemplate), typeof(ChooseTemplate));
            Routing.RegisterRoute(nameof(ArbitraryElement), typeof(ArbitraryElement));
        }
    }
}

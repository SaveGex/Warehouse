using Warehouse.Auxiliary;

namespace Warehouse
{
    public partial class App : Application
    {
        private readonly MainPage _mainPage;

        public App(MainPage mainPage)
        {
            InitializeComponent();
            //_mainPage = mainPage;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        public override void ActivateWindow(Window window)
        {
            //MainPage = _mainPage;

            //MainPage = new AppShell();

            base.ActivateWindow(window);
        }
    }
}

﻿
namespace Warehouse
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("AddIndefinedElement", typeof(AddIndefinedElement));
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("ChooseTemplate", typeof(ChooseTemplate));
        }
    }
}

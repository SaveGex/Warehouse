using baseElement = Warehouse.Resources.Auxiliary.baseElement;

namespace Warehouse
{
    public partial class MainPage : ContentPage
    {
        Grid views = new Grid();

        List<baseElement> elements;

        int rows = DeviceInfo.Platform == DevicePlatform.WinUI ? 5 : DeviceInfo.Platform == DevicePlatform.Android ? 2 : 3;
        int cols = DeviceInfo.Platform == DevicePlatform.WinUI ? 4 : DeviceInfo.Platform == DevicePlatform.Android ? 10 : 9;
        public int totNumber { get; set; }
        

        public MainPage()
        {
            InitializeComponent();

            totNumber = rows * cols;

            CreateMarkUp();
        }

        private void AddElement(object sender, EventArgs e)
        {
            elements.Append(new baseElement());
        }

        private void CreateMarkUp()
        {
            for (int i = 0; i < rows; i++)
            {
                views.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            for (int i = 0; i < cols; i++)
            {
                views.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
        }
    }

}

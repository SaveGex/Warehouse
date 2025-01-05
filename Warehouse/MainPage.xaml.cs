using System.Reflection.Metadata;
using System.Text.Json;
using BaseElement = Warehouse.Resources.Auxiliary.BaseElement;

namespace Warehouse
{
    //[QueryProperty(nameof(someObject), "someObject")]
    public partial class MainPage : ContentPage
    {
        Grid views = new Grid();

        List<BaseElement> _elements;

        public Command OpenModalCommand { get; }

        int _rows = DeviceInfo.Platform == DevicePlatform.WinUI ? 5 : DeviceInfo.Platform == DevicePlatform.Android ? 2 : 3;
        int _cols = DeviceInfo.Platform == DevicePlatform.WinUI ? 4 : DeviceInfo.Platform == DevicePlatform.Android ? 10 : 9;
        public int totNumber { get; set; }
        private BaseElement? _baseElement { get; set; }
        //public string someObject 
        //{ 
        //    set
        //    {
        //        if (!string.IsNullOrEmpty(value)) {
        //            _baseElement = JsonSerializer.Deserialize<BaseElement>(value);

        //        }
        //    }
        //}
        public MainPage()
        {
            InitializeComponent();
            OpenModalCommand = new Command(OpenModal);
            
            CreateMarkUp();

            _elements = new List<BaseElement>();
            _baseElement = NavigationData.CurrentBaseElement;

            if (_baseElement != null)
                UpdateViewElements();

            totNumber = _rows * _cols;
            BindingContext = this;

        }
        private async void OpenModal()
        {
            await Shell.Current.GoToAsync("AddElement");
        }

        private void UpdateViewElements()
        {
            _elements.Add(_baseElement);
        }

        private void CreateMarkUp()
        {
            for (int i = 0; i < _rows; i++)
            {
                views.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            for (int i = 0; i < _cols; i++)
            {
                views.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            scrollView.Content = views;
            if(_elements == null)
                return;
            
            foreach (BaseElement element in _elements)
            {
                for (int i = 0; i < _rows; i++)
                {
                    for (int j = 0; j < _cols; j++)
                    {
                        var card = CreateCard(element);

                        views.Children.Add(card);
                        Grid.SetRow(card, i);
                        Grid.SetColumn(card, j);
                    }
                }
            }
        }

        private Frame CreateCard(BaseElement element)
        {
            if (element.image == null)
                return null;
            var image = new Image
            {
                Source = BaseElement.ConvertBytesToImageSource(element.image),
                HeightRequest = 100, 
                WidthRequest = 100,
                Aspect = Aspect.AspectFit,
            };

            var title = new Label
            {
                Text = element.description,
                FontAttributes = FontAttributes.Italic,
                FontSize = 12,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 5, 0, 5),
            };

            var button = new Button
            {
                Text = "Details",
                BackgroundColor = Colors.LightBlue,
                TextColor = Colors.White,
                Command = new Command(() => ShowDetails(element)),
            };

            var buttonDelImg = new Image
            {
                Source = "red_trash_canon_icon.png",
                WidthRequest=30, HeightRequest=30,
            };

            var tapGestureRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() => ShowDetails(element)),
            };

            buttonDelImg.GestureRecognizers.Add(tapGestureRecognizer);

            var stackLayout = new StackLayout
            {
                Padding = new Thickness(10),
                Children = {image, title, button, buttonDelImg}
            };

            var frame = new Frame
            {
                CornerRadius = 10,
                BorderColor = Colors.Gray,
                BackgroundColor = Colors.White,
                Content = stackLayout,
                Margin = new Thickness(5),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
            };
            return frame;
        }

        private async void ShowDetails(BaseElement element)
        {
            await Shell.Current.GoToAsync("somePage");
        }

    }
}

﻿using Microsoft.IdentityModel.Tokens;
using Warehouse.Auxiliary;
using Warehouse.Auxiliary.Patterns;
using Warehouse.Auxiliary.Patterns.Interfaces;
using Warehouse.DataBase;
using BaseElement = Warehouse.DataBase.Models.BaseElement;

namespace Warehouse
{
    [QueryProperty(nameof(AfRows), "afRows")]
    public partial class MainPage : ContentPage, IObserver
    {
        //insane spot for such pattern as Observer ! I am happy
        List<ISubscriber> Subscribers;
        Grid views = new Grid();
        public int AfRows { get; set; }
        List<BaseElement> _elements;
        public Command OpenModalCommand { get; }
        int _rows = DeviceInfo.Platform == DevicePlatform.WinUI ? 5 : DeviceInfo.Platform == DevicePlatform.Android ? 2 : 3;
        int _cols = DeviceInfo.Platform == DevicePlatform.WinUI ? 4 : DeviceInfo.Platform == DevicePlatform.Android ? 10 : 9;
        public int totNumber { get; set; }
        private BaseElement? _baseElement { get; set; }


        public MainPage()
        {
            InitializeComponent();

            //initializing
            OpenModalCommand = new Command(OpenModal);
            _elements        = new List<BaseElement>();
            Subscribers      = new List<ISubscriber>();

            //Add subscribers
            Subscribers.Add(new DBManagerSubscriber());

            //binding data
            totNumber = _rows * _cols;
            BindingContext = this; // for binding context. It's mean all xaml code of this page will be looking for each BINDING data in xaml - will be looking for in that class

            LoadXXElements();
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            _baseElement = NavigationData.CurrentBaseElement;

            await DisplayAlert("Attention", $"Affected rows: {AfRows}", "Ok");

            AddElement();

            CreateMarkUp();
        }

        //Roman numerals "X = 10" Load_XX_ == Load20...()
        private async void LoadXXElements()
        {
            List<Dictionary<string, object>>? queryList = await WarehouseStaticContext.ExecuteQueryAsync("SELECT TOP 20 [Id], [Name], [Image], [Description] FROM Undefined;");
            
            if(queryList != null && queryList.Count > 0)
            {

                for (int i = 0; i < queryList.Count; i++)
                {
                    BaseElement tempElement = new BaseElement();
                    foreach (var item in queryList[i])
                    {
                        if (item.Key == "Id") 
                            tempElement.Id = (int)item.Value;
                        else if (item.Key == "Name")
                            tempElement.name = item.Value as string;
                        else if (item.Key == "Image")
                            tempElement.image = item.Value as byte[];
                        else if(item.Key == "Description")
                            tempElement.description = item.Value as string;
                    }
                    if (tempElement.notNull())
                    {
                        tempElement.objectIndex = _elements.Count; // Initialize objectIndex
                        _elements.Add(tempElement);
                    }
                }
                CreateMarkUp();
            }
        }


        public async void OpenModal()
        {
            await Shell.Current.GoToAsync("ChooseTemplate");
        }


        private void AddElement()
        {
            NotifySubscribers("ADD", _baseElement);

            if(_baseElement != null)
                _elements.Add(_baseElement);
        }


        private void CreateMarkUp()
        {

            views.Children.Clear();
            views.RowDefinitions.Clear();
            views.ColumnDefinitions.Clear();

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


            if (_elements.Count > 0)
            {
                int index = 0;
                for (int i = 0; i < _rows && index < _elements.Count; i++)
                {
                    for (int j = 0; j < _cols && index < _elements.Count; j++)
                    {
                        var card = CreateCard(_elements[index]);

                        if (card == null)
                            throw new NullReferenceException();
                        
                        views.Children.Add(card);
                        Grid.SetRow(card, i);
                        Grid.SetColumn(card, j);
                        index++;
                    }
                }
            }
        }


        private Frame? CreateCard(BaseElement element)
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
                TextColor = Colors.Black,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 2, 0, 2),
            };

            var id = new Label
            {
                Text = element.objectIndex.ToString(),
                IsVisible = false,
            };

            var button = new Button
            {
                Text = "Details",
                BackgroundColor = Colors.LightBlue,
                WidthRequest = 80,
                FontSize = 12,
                TextColor = Colors.FloralWhite,
                Command = new Command(() => ShowDetails(element)),
            };

            var buttonDelImg = new ImageButton
            {
                Source = "red_trash_canon_icon.png",
                WidthRequest = 30,
                HeightRequest = 30,
                BackgroundColor = Colors.Transparent 
            };
            var tapGestureRecognizer = new TapGestureRecognizer();

            tapGestureRecognizer.Command = new Command(parameter =>
            {
                var tappedImg = (ImageButton)tapGestureRecognizer.Parent;

                var parentHSL = tappedImg.Parent as HorizontalStackLayout;
                if(parentHSL != null)
                {
                    var some_card = parentHSL.Parent as StackLayout;
                    if (some_card != null)
                    {
                        var indexOfSomeCard = some_card.Children.OfType<Label>().FirstOrDefault(lbl => lbl.IsVisible == false);

                        if(indexOfSomeCard != null)
                        {
                            var index = indexOfSomeCard.Text;
                            if(index!=null)
                                deleteElementAndReMarkUp(Convert.ToInt32(index));
                        }
                    }
                }
                
            });

            buttonDelImg.GestureRecognizers.Add(tapGestureRecognizer);
            
            var horizontalLayout = new HorizontalStackLayout
            {
                Children = {button, buttonDelImg}
            };

            var stackLayout = new StackLayout
            {
                Padding = new Thickness(5, 2, 5, 2),
                Children = {image, title, horizontalLayout, id}
            };

            var frame = new Frame
            {
                CornerRadius = 10,
                BorderColor = Colors.White,
                BackgroundColor = Colors.LightGray,
                Content = stackLayout,
                Margin = new Thickness(5),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
            };
            return frame;
        }
        

void deleteElementAndReMarkUp(int index)
        {
            NotifySubscribers("DELETE", _elements[index]);

            _elements.RemoveAt(index);
            for (int i = 0; i < _elements.Count; i++)
            {
                _elements[i].objectIndex = i; // Reassign objectIndex to maintain consistency
            }
            CreateMarkUp();
        }


        private async void ShowDetails(BaseElement element)
        {
            //await Shell.Current.GoToAsync("somePage");
        }


        public bool AddSubscriber(ISubscriber? subscriber)
        {
            if(subscriber == null)
                return false;
            Subscribers.Add(subscriber);
            return true;
        }


        public bool RemoveSubscriber(ISubscriber? subscriber)
        {
            if (subscriber == null)
                return false;
            Subscribers.Remove(subscriber);
            return true;
        }


        public void NotifySubscribers(string? args = null, object? obj = null)
        {
            foreach (ISubscriber subscriber in Subscribers)
            {
                subscriber.Update(args, obj);
            }
        }
    }
}

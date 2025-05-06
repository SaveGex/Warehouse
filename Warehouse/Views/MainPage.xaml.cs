using Warehouse.Auxiliary.Patterns.Interfaces;

namespace Warehouse
{
    public partial class MainPage : ContentPage, IObserver, IQueryAttributable
    {
        private List<ISubscriber> _subscribers = new();
        public MainPage()
        {
            InitializeComponent();
            if (BindingContext is ISubscriber vmAsSubscriber)
            {
                AddSubscriber(vmAsSubscriber);
            }
        }

        public void OnAppearing()
        {
            base.OnAppearing();
            NotifySubscribers("LOAD", null);
        }

        public bool AddSubscriber(ISubscriber? subscriber)
        {
            if (subscriber == null)
                return false;
            _subscribers.Add(subscriber);
            return true;
        }

        public bool RemoveSubscriber(ISubscriber? subscriber)
        {
            if (subscriber == null)
                return false;
            _subscribers.Remove(subscriber);
            return true;
        }
        public void NotifySubscribers(string args, object obj)
            => _subscribers.ForEach(s => s.Update(args, obj));

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("UpdateElements", out var raw)
                && bool.TryParse(raw?.ToString(), out var update)
                && update)
            {
                NotifySubscribers("RELOAD", null);
            }
        }
    }
}

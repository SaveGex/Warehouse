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

        protected override void OnAppearing()
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
        public void NotifySubscribers(string? args = null, object? obj = null, Dictionary<string, object>? qargs = null)
        {
            foreach(var subscriber in _subscribers)
            {
                if (subscriber is ISubscriber sub)
                {
                    sub.Update(args, obj, qargs);
                }
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            string? args = null;
            Dictionary<string, object>? qargs = new Dictionary<string, object>();

            if (query.TryGetValue("UpdateElements", out var raw)
                && bool.TryParse(raw?.ToString(), out var update)
                && update)
                args = "RELOAD";


            if (query.ContainsKey("ElementId") && int.TryParse(query["ElementId"]?.ToString(), out var elementId))
                qargs["ElementId"] = elementId;
            
            NotifySubscribers(args, null, qargs);
        }
    }
}

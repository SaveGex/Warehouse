using Warehouse.Auxiliary;
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
            foreach (var subscriber in _subscribers)
            {
                if (subscriber is ISubscriber sub)
                {
                    sub.Update(args, obj, qargs);
                }
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("UpdateElements", out var rawUpdateElements)
                && bool.TryParse(rawUpdateElements?.ToString(), out var update)
                && update)
            {
                string? args = null;
                Dictionary<string, object>? qargs = new Dictionary<string, object>();
                args = "RELOAD";
                if (query.ContainsKey("ElementId") && int.TryParse(query["ElementId"]?.ToString(), out var elementId))
                    qargs["ElementId"] = elementId;

                NotifySubscribers(args, null, qargs);
            }
            else if (query.TryGetValue("AffectedRows", out var rawAffectedRows)
                && int.TryParse(rawAffectedRows?.ToString(), out var afRows)
                && afRows > 0)
            {
                NotifySubscribers("ADD", NavigationData.CurrentBaseElement, null);
            }
        }
    }
}

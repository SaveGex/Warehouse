using System.Collections.ObjectModel;
using System.Windows.Input;
using Warehouse.Auxiliary.Patterns.Interfaces;
using Warehouse.DataBase.Models;
using Warehouse.Models;

namespace Warehouse.ViewModels;

class MainPageViewModel : IQueryAttributable, IObserver, ISubscriber
{
    private List<ISubscriber> _subsribers = new();
    private MainPageModel _model = new MainPageModel();
    public ObservableCollection<BaseElement> Elements => _model.Elements;

    public int AfRows { get; set; }

    public ICommand OpenModalCommand => _model.OpenModalCommand;
    public ICommand ShowDetailsCommand => _model.ShowDetailsCommand;
    public ICommand DeleteElementCommand => _model.DeleteElementCommand;


    public MainPageViewModel() 
    {
        _subsribers.Add(_model);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("AffectedRows") && query["AffectedRows"] is int)
            AfRows = (int)query["AffectedRows"];
        else
            AfRows = 0;
    }

    public void Update(string? args = null, object? obj = null, Dictionary<string, object>? qargs = null)
    {
        if (args == "RELOAD")
            NotifySubscribers("RELOAD", obj, qargs);
        else if(args == "ADD")
            NotifySubscribers("ADD", obj, qargs);
    }

    public bool AddSubscriber(ISubscriber? subscriber)
    {
        if (subscriber == null)
            return false;
        _subsribers.Add(subscriber);
        return true;
    }

    public bool RemoveSubscriber(ISubscriber? subscriber)
    {
        if(subscriber == null)
            return false;
        _subsribers.Remove(subscriber);
        return true;
    }

    public void NotifySubscribers(string? args, object? obj, Dictionary<string, object>? qargs = null)
    {
        foreach(var subscriber in _subsribers)
        {
            subscriber.Update(args, obj, qargs);
        }
    }
}
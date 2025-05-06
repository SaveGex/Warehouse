using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Warehouse.Auxiliary;
using Warehouse.Auxiliary.Patterns;
using Warehouse.Auxiliary.Patterns.Interfaces;
using Warehouse.DataBase;
using Warehouse.DataBase.Models;
using Warehouse.Views;

namespace Warehouse.Models;

class MainPageModel : ObservableCollection<BaseElement>, ISubscriber, IObserver
{
    public BaseElement? BaseElement { get; set; }
    private List<ISubscriber> Subscribers;
    public ObservableCollection<BaseElement> Elements { get; set; }
    public int AfRows { get; set; }
    public ICommand OpenModalCommand { get; }
    public ICommand ShowDetailsCommand { get; }
    public ICommand DeleteElementCommand { get; }
    public MainPageModel()
    {
        //initializing
        OpenModalCommand = new AsyncRelayCommand(OpenModal);
        ShowDetailsCommand = new AsyncRelayCommand<BaseElement>(ShowDetails);
        DeleteElementCommand = new AsyncRelayCommand<BaseElement>(RemoveElement);
        Elements = new ObservableCollection<BaseElement>();
        Subscribers = new List<ISubscriber>();

        //Add subscribers
        Subscribers.Add(new DBManagerSubscriber());

        //setup view
        LoadElements(20);
    }

    private async Task RemoveElement(BaseElement? element)
    {
        if (element == null)
            return;
        Elements.Remove(element);
        OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Remove, element));
        NotifySubscribers("DELETE", element);
    }

    public async Task OpenModal()
    {
        await Shell.Current.GoToAsync("ChooseTemplate");
    }

    private async Task ShowDetails(BaseElement element)
    {
        await Shell.Current.GoToAsync(nameof(ArbitraryElement)+$"?Id={element.Id}");
    }

    private void LoadElements(int count)
    {
        DbContextOptionsBuilder<WarehouseContext> optionsBuilder = new DbContextOptionsBuilder<WarehouseContext>();
        optionsBuilder.UseSqlServer(AppConfig.GetConnectionString());

        DbContextOptions<WarehouseContext> options = optionsBuilder.Options;
        using var Db = new WarehouseContext(options);

        List<BaseElement> colection = Db.Indefined.OrderBy(x => x.Id).Take(20).Select(x => new BaseElement(x.image, x.name, x.description, x.Id)).ToList();        

        foreach (var item in colection) 
            Elements.Add((BaseElement)item);
    }

    private void AddElement()
    {
        NotifySubscribers("ADD", BaseElement);

        if (NavigationData.CurrentBaseElement != null)
        {
            BaseElement = NavigationData.CurrentBaseElement;
            Elements.Add(NavigationData.CurrentBaseElement);
            OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Add, NavigationData.CurrentBaseElement));
        }
    }

    public bool AddSubscriber(ISubscriber? subscriber)
    {
        if (subscriber == null)
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

    public void Update(string? args = null, object? obj = null)
    {
        if (args == "ADD")
        {
            AddElement();
        }
        else if (args == "LOAD")
        {
            if (NavigationData.CurrentBaseElement is not null)
                AddElement();
            OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
        }
        else if(args == "RELOAD")
        {
            int minId = Elements.Min(x => x.Id);
            Elements = null;
            OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Remove));

            DbContextOptionsBuilder<WarehouseContext> optionsBuilder = new DbContextOptionsBuilder<WarehouseContext>().UseSqlServer(AppConfig.GetConnectionString());
            using var Db = new WarehouseContext(optionsBuilder.Options);

            List<BaseElement> colection = Db.Indefined.Where(x => x.Id > minId).OrderBy(x => x.Id).Select(x => new BaseElement(x.image, x.name, x.description, x.Id)).ToList();
            OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Add, colection));
        }
    }
}

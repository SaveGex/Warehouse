﻿using CommunityToolkit.Mvvm.Input;
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

class MainPageModel : ISubscriber, IObserver
{
    public BaseElement? Element { get; set; }
    private List<ISubscriber> Subscribers;
    public ObservableCollection<BaseElement> Elements { get; set; }
    public int AfRows { get; set; }
    public ICommand OpenModalCommand { get; }
    public ICommand ShowDetailsCommand { get; }
    public ICommand DeleteElementCommand { get; }

    private readonly int _maxElements = 10; // Maximum number of elements to load at once for viewport
    public MainPageModel()
    {
        //initializing
        OpenModalCommand = new AsyncRelayCommand(OpenModal);
        ShowDetailsCommand = new AsyncRelayCommand<BaseElement?>(ShowDetails);
        DeleteElementCommand = new AsyncRelayCommand<BaseElement?>(RemoveElement);
        Elements = new ObservableCollection<BaseElement>();
        Subscribers = new List<ISubscriber>();

        //Add subscribers
        Subscribers.Add(new DBManagerSubscriber());

        //setup view
        LoadElements(_maxElements);
    }

    private Task RemoveElement(BaseElement? element)
    {
        if (element == null)
            return Task.CompletedTask;
        int index = Elements.IndexOf(element);

        
        NotifySubscribers("DELETE", element);
        
        var optionsBuilder = new DbContextOptionsBuilder<WarehouseContext>().UseSqlServer(AppConfig.GetConnectionString());
        using var Db = new WarehouseContext(optionsBuilder.Options);
        List<int> ids = Elements.Select(x => x.Id).ToList();
        BaseElement? el = Db.Indefined.Where(x =>  !ids.Contains(x.Id) && x.Id < element.Id).OrderByDescending(x => x.Id).FirstOrDefault();

        if (el is null)
        {
            Elements.Remove(element);
            return Task.CompletedTask;
        }
        else
            Elements[index] = el;     

        return Task.CompletedTask;
    }

    public async Task OpenModal()
    {
        await Shell.Current.GoToAsync("ChooseTemplate");
    }

    private async Task ShowDetails(BaseElement? element)
    {
        if (element == null)
            return;
        await Shell.Current.GoToAsync(nameof(ArbitraryElement)+$"?Id={element.Id}");
    }

    private void LoadElements(int count)
    {
        DbContextOptionsBuilder<WarehouseContext> optionsBuilder = new DbContextOptionsBuilder<WarehouseContext>();
        optionsBuilder.UseSqlServer(AppConfig.GetConnectionString());

        DbContextOptions<WarehouseContext> options = optionsBuilder.Options;
        using var Db = new WarehouseContext(options);

        List<BaseElement> colection = Db.Indefined.OrderByDescending(x => x.Id).Take(count).Select(x => new BaseElement(x.Id, x.image, x.name, x.description)).ToList();        

        foreach (var item in colection) 
            Elements.Add((BaseElement)item);
    }

    private void AddElement()
    {
        if (Elements.Count == _maxElements)
            Elements.RemoveAt(Elements.Count - 1);

        NotifySubscribers("ADD", NavigationData.CurrentBaseElement);

        if (NavigationData.CurrentBaseElement != null)
        {
            Element = NavigationData.CurrentBaseElement;
            Elements.Insert(0, NavigationData.CurrentBaseElement);
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

    public void NotifySubscribers(string? args = null, object? obj = null, Dictionary<string, object>? qargs = null)
    {
        foreach (ISubscriber subscriber in Subscribers)
        {
            subscriber.Update(args, obj);
        }
    }

    public void Update(string? args = null, object? obj = null, Dictionary<string, object>? qargs = null)
    {
        if (args == "RELOAD" && qargs is not null)
        {
            int elementId = (int)qargs["ElementId"];
            int index = Elements.IndexOf(Elements.First(x => x.Id == elementId));
            BaseElement oldElement = Elements[index];

            var optionsBuilder = new DbContextOptionsBuilder<WarehouseContext>().UseSqlServer(AppConfig.GetConnectionString());
            WarehouseContext Db = new WarehouseContext(optionsBuilder.Options);

            BaseElement changedElement = Db.Indefined.First(x => x.Id == elementId);

            Elements[index] = changedElement;
        }
        if(args == "ADD")
        {
            AddElement();
        }
    }
}

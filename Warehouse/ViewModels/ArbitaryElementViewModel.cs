using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using Warehouse.DataBase;
using Warehouse.DataBase.Models;
using Warehouse.Models;

namespace Warehouse.ViewModels;

class ArbitaryElementViewModel : ObservableObject, IQueryAttributable
{
    private ArbitaryElementModel _model;
    private BaseElement _prymaryElement;
    public ICommand ChangeInfoCommand { get; }
    public ICommand ChangeImageCommand { get; }

    private BaseElement _element;
    public BaseElement Element
    {
        get => _element;
        set
        {
            if (_element != value)
            {
                _element = value;
                OnPropertyChanged(nameof(Element));
            }
        }
    }
            
    private BaseElement prymaryElement
    {
        get => _prymaryElement;
        set
        {
            if (_prymaryElement is null)
                _prymaryElement = value;
        }
    }


    public ArbitaryElementViewModel()
    {
        _model = new ArbitaryElementModel();
        ChangeInfoCommand = new AsyncRelayCommand(ChangeInfo);
        ChangeImageCommand = new AsyncRelayCommand(ChangeImage);
    }

    private async Task ChangeImage()
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Pick image please",
            FileTypes = FilePickerFileType.Images
        });

        if (result == null)
            return;

        using var stream = await result.OpenReadAsync();

        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);

        Element.image = ms.ToArray();
        OnPropertyChanged(nameof(Element));

        var tmp = Element;
        Element = null;       // (щоб гарантовано спрацювало OnPropertyChanged)
        Element = tmp;
    }


    private async Task ChangeInfo()
    {
        DbContextOptionsBuilder<WarehouseContext> optionsBuilder = new DbContextOptionsBuilder<WarehouseContext>();
        optionsBuilder.UseSqlServer(AppConfig.GetConnectionString());
        DbContextOptions<WarehouseContext> options = optionsBuilder.Options;
        using var Db = new WarehouseContext(options);

        BaseElement query = Db.Indefined.First(x => x.Id == prymaryElement.Id);

        query.name = Element.name;
        query.description = Element.description;
        query.image = Element.image;
        
        Db.Update(query);
        Db.SaveChanges();


        await Shell.Current.GoToAsync($"//MainPage?UpdateElements=True");
    }


    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if(query.ContainsKey("Id"))
        {
            int id = int.Parse((string)query["Id"]);
            DbContextOptionsBuilder<WarehouseContext> optionsBuilder = new DbContextOptionsBuilder<WarehouseContext>();
            optionsBuilder.UseSqlServer(AppConfig.GetConnectionString());

            DbContextOptions<WarehouseContext> options = optionsBuilder.Options;
            using var Db = new WarehouseContext(options);

            Element = Db.Indefined.First(x=>x.Id == id);
            
            //Late initializating
            _model.BindingProperties(Element);
            prymaryElement = Element;
        }
        else
        {
            throw new Exception($"Element is not initialized. Directory: {Path.Join(AppContext.BaseDirectory, "ViewModel", "ArbitaryElementViewModel")}");
        }

    }
}

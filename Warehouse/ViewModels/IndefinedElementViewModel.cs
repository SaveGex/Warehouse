using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using Warehouse.Auxiliary;
using Warehouse.DataBase;
using Warehouse.DataBase.Models;
using Warehouse.Models;

namespace Warehouse.ViewModels;

internal class IndefinedElementViewModel :ObservableObject
{
    private IndefinedElementModel IndefinedElementModel { get; set; } = new IndefinedElementModel(); //for business logic but here have no one business logic
    public BaseElement Element { get; set; } = new BaseElement(); //for view and binding


    public ICommand CloseModalWindow { get; }
    public ICommand PickImage { get; }
    public ICommand Admit { get; }

    public IndefinedElementViewModel()
    {
        CloseModalWindow = new AsyncRelayCommand(OnCloseModalWindow);
        PickImage = new AsyncRelayCommand(OnPickImage);
        Admit = new AsyncRelayCommand(OnAdmit);
    }

    private async Task OnAdmit()
    {
        if (string.IsNullOrWhiteSpace(Element.name) || string.IsNullOrWhiteSpace(Element.description) || Element.image == null)
        {
            string alert = string.Empty;
            if (string.IsNullOrWhiteSpace(Element.name)) alert += "\n\nField name is empty";
            if (Element.image == null) alert += "\n\nYou have to provide an image";
            if (string.IsNullOrWhiteSpace(Element.description)) alert += "\n\nYou have to provide some description about your object";

            await Shell.Current.DisplayAlert("Warning", "You must fill out the form to add an element" + alert, "Ok");
            return;
        }

        NavigationData.CurrentBaseElement = Element;

        // Save to DB
        var optionsBuilder = new DbContextOptionsBuilder<WarehouseContext>().UseSqlServer(AppConfig.GetConnectionString());
        using var db = new WarehouseContext(optionsBuilder.Options);

        db.Indefined.Add(Element);
        int affectedRows = await db.SaveChangesAsync();

        await Shell.Current.GoToAsync($"//MainPage?AffectedRows={affectedRows}");
    }


    private async Task OnCloseModalWindow()
    {
        await Shell.Current.GoToAsync("//MainPage");

    }


    private async Task OnPickImage()
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Pick image please",
            FileTypes = FilePickerFileType.Images
        });

        if (result == null)
        {
            return;
        }

        Element.image = await File.ReadAllBytesAsync(result.FullPath);
        OnPropertyChanged(nameof(Element));
    }

}

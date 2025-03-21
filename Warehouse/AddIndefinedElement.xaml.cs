using Warehouse.Auxiliary;
using Warehouse.DataBase;
using Warehouse.DataBase.Models;

namespace Warehouse;

public partial class AddIndefinedElement : ContentPage
{
	public AddIndefinedElement()
	{
		InitializeComponent();
    }


    private async void OnAdmit(object sender, EventArgs e)
    {
        bool markNameExists = false, markDescriptionExists = false, markImageExists = false;
        byte[]? imageData = null;
        var fileSource = DroppedImage.Source as FileImageSource;

        if (NamePostField.Text != null)
        {
            markNameExists = true;
        }

        if (DescriptionField.Text != null)
        {
            markDescriptionExists = true;
        }

        if (fileSource != null)
        {
            string filePath = fileSource.File; 
            if (File.Exists(filePath))
            {
                imageData = File.ReadAllBytes(filePath);
                markImageExists = true;
            }
        }

        if (!markNameExists || !markImageExists || !markDescriptionExists)
        {
            string allert = !markNameExists ? "\n\nField name is empty" : string.Empty;
            allert = !markImageExists ? allert + "\n\nYou have to provide an image" : allert;
            allert = !markDescriptionExists ? allert + "\n\nYou have to provide an some description about your object" : allert;
            await DisplayAlert("Warning", "You must fill out the form for add element" + allert, "Ok");
        }
        else if(imageData             != null && 
                NamePostField.Text    != null && 
                DescriptionField.Text != null) 
        {
            BaseElement baseElement = new BaseElement(imageData, NamePostField.Text, DescriptionField.Text);
            NavigationData.CurrentBaseElement = baseElement;

            string query = "INSERT INTO Undefined ([Name], [Image], [Description]) VALUES (@Name, @Image, @Description)";

            // Підготовка параметрів
            var parameters = new Dictionary<string, object>
            {
                { "@Name", NamePostField.Text },
                { "@Image", imageData },
                { "@Description", DescriptionField.Text }
            };

            // Виконання запиту
            int afRows = await WarehouseStaticContext.ExecuteNonQueryAsync(query, parameters);

            await Shell.Current.GoToAsync($"//MainPage?afRows={afRows}");
        }
        else
        {
            await DisplayAlert("Error", "Unexpected exception in AddIndefinedElement.xaml.cs", "Ok");
        }
    }


    private async void OnCloseModalWindow(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");

    }


    private async void OnPickImage(object sender, EventArgs e)
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

        DroppedImage.Source = result.FullPath;
    }

    
    private async void OnHoverImage(object sender, EventArgs e)
    {
        var image = sender as Image;
        if(image != null)
        {
            await image.ScaleTo(2, 1050, Easing.CubicInOut);
        }
    }


    private async void OnExitImage(object sender, EventArgs e)
    {
        var image = sender as Image;
        if (image != null)
        {
            await image.ScaleTo(1, 250, Easing.CubicInOut);
        }
    }
}
using System.Xml.Linq;
using Warehouse.Resources.Auxiliary;
using System.Text.Json;

namespace Warehouse;

public partial class AddElement : ContentPage
{
	public AddElement()
	{
		InitializeComponent();
    }


    private async void OnAdmit(object sender, EventArgs e)
    {
        bool markNameExists = false, markDescriptionExists = false, markImageExists = false;
        byte[]? imageData = null;

        if (NamePostField.Text != null)
        {
            markNameExists = true;
        }

        if (DescriptionField.Text != null)
        {
            markDescriptionExists = true;
        }

        var fileSource = DroppedImage.Source as FileImageSource;
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
        else if(imageData != null && markNameExists && markDescriptionExists) 
        {
            BaseElement baseElement = new BaseElement(imageData, NamePostField.Text, DescriptionField.Text);
            NavigationData.CurrentBaseElement = baseElement;
            await Shell.Current.GoToAsync("..");
            //await Navigation.PushAsync(new MainPage(baseElement));
            //await Shell.Current.GoToAsync("MainPage");
        }
        else
        {
            await DisplayAlert("Error", "Unexpected exception in AddElement.xaml.cs", "Ok");
        }
    }

    private async void OnCloseModalWindow(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");

    }
	//private async void OnImageDrop(object sender, DropEventArgs e)
	//{
 //       if (e.Data.Properties.ContainsKey("Image")){
 //           var img = await e.Data.GetImageAsync();
 //           if (img != null)
 //           {
 //               DataBaseContext baseContext = new DataBaseContext();
 //               DroppedImage.Source = img;
 //               //baseContext.baseElements.Add();
 //           }
 //           //DataBaseContext context = new DataBaseContext();
 //           //ToDo: View and addition elements to table. (Figure out how to work with table)
 //           else
 //           {
 //               // Якщо зображення не вдалося отримати
 //               await Application.Current.MainPage.DisplayAlert("Error", "Failed to load image", "OK");
 //           }
 //       }
 //   }
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
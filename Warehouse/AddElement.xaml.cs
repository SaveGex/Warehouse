using Warehouse.Resources.Auxiliary;

namespace Warehouse;

public partial class AddElement : ContentPage
{
	public AddElement()
	{
		InitializeComponent();
	}

    private async void OnCloseModalWindow(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("..");
    }
	private async void OnImageDrop(object sender, DropEventArgs e)
	{
        var img = await e.Data.GetImageAsync();
        if (img != null)
        {
            DroppedImage.Source = img;
        }
        DataBaseContext context = new DataBaseContext();
        //ToDo: View and addition elements to table. (Figure out how to work with table)
    }
}
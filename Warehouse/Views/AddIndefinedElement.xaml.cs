using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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



    
    private async void OnHoverImage(object sender, EventArgs e)
    {
        var image = sender as Image;
        if(image != null)
        {
            image.MaximumWidthRequest = 550;
            image.MaximumHeightRequest = 550;
            await image.ScaleToAsync(2, 1050, Easing.CubicInOut);
        }
    }


    private async void OnExitImage(object sender, EventArgs e)
    {
        var image = sender as Image;
        if (image != null)
        {
            //image.MaximumWidthRequest = 250;
            //image.MaximumHeightRequest = 250;
            await image.ScaleToAsync(1, 250, Easing.CubicInOut);
        }
    }
}
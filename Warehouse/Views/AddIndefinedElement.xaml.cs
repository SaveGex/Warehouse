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
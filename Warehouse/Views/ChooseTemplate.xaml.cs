namespace Warehouse;

public partial class ChooseTemplate : ContentPage
{
	public ChooseTemplate()
	{
		InitializeComponent();

		WrapFramesAndNumerationsNamesOfBorders();
	}


    private void OnFrame(object sender, PointerEventArgs e)
	{
		if (sender is Border border)
		{
			var animation = new Animation(v => border.BackgroundColor = Color.FromRgba(0, 252, 255, v), 0, 1);
			animation.Commit(border, "BorderAppear", length: 500, easing: Easing.Linear);
		}
    }


    private void OutFrame(object sender, PointerEventArgs e)
	{
		if (sender is Border border)
		{
            var animation = new Animation(v => border.BackgroundColor = Color.FromRgba(0, 252, 255, v), 1, 0);
			animation.Commit(border, "BorderFade", length: 500, easing: Easing.Linear);
        }
    }

    private void WrapFramesAndNumerationsNamesOfBorders()
	{
		//The plan
		//GridView -> child -> <Border x:Name = "*string + digit*"> *Child* </Border>
		//By the plan I kind of don't know how many types templates I'll make
		for(int i = 0; i < ShowGrid.Count; i++)
		{
			var child = ShowGrid[i];
			if (child is Border border)
			{
				Border someBorder = new Border()
				{
					StrokeThickness = 3,
					BackgroundColor = Color.FromRgba(255, 255, 255, 1),
					Content = (View)child
                };
			}
		}
	}

    private async void ToIndefinedElement(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("AddIndefinedElement");
	}


	private async void ToAddPizza(object sender, EventArgs e)
	{
		await DisplayAlertAsync("In development...", "This template is in development", "OK");
        await Shell.Current.GoToAsync("..");
	}


}
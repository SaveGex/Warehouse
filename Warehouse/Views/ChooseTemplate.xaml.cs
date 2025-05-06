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
		if (sender is Frame frame)
		{
			var animation = new Animation(v => frame.BorderColor = Color.FromRgba(0, 252, 255, v), 0, 1);
			animation.Commit(frame, "BorderAppear", length: 500, easing: Easing.Linear);
		}
    }


    private void OutFrame(object sender, PointerEventArgs e)
	{
		if (sender is Frame frame)
		{
            var animation = new Animation(v => frame.BorderColor = Color.FromRgba(0, 252, 255, v), 1, 0);
			animation.Commit(frame, "BorderFade", length: 500, easing: Easing.Linear);
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
			if (child is Frame frame)
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
		await DisplayAlert("In development...", "This template is in development", "OK");
        await Shell.Current.GoToAsync("..");
	}


}
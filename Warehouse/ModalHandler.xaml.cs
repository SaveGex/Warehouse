namespace Warehouse;

public partial class ModalHandler : ContentPage
{
	public ModalHandler()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		await Shell.Current.GoToAsync("ModalPageAdd");

        await Shell.Current.GoToAsync("..");
	}
}
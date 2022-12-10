using Home_Automation_Maui.ViewModels;

namespace Home_Automation_Maui.Views;

public partial class MainPage : ContentPage
{
	private MainPageViewModel _ViewModel;

	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_ViewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
		await _ViewModel.OnAppearing();
    }
}


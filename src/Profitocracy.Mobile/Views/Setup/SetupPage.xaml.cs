using ExpencesTracker.Mobile.Abstractions;
using ExpencesTracker.Mobile.ViewModels.Setup;

namespace ExpencesTracker.Mobile.Views.Setup;

public partial class SetupPage : BaseContentPage
{
	private readonly SetupPageViewModel _viewModel;
	
	public SetupPage(SetupPageViewModel viewModel)
	{
		InitializeComponent();
		
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}
	
	protected override bool OnBackButtonPressed()
	{
		return true;
	}
	
	private void Button_OnClicked(object? sender, EventArgs e)
	{
		ProcessAction(async () =>
		{
			await _viewModel.CreateFirstProfile();
			await Navigation.PopModalAsync();
		});
	}
}
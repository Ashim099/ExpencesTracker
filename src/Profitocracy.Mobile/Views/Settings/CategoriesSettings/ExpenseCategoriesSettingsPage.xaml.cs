using ExpencesTracker.Mobile.Abstractions;
using ExpencesTracker.Mobile.Resources.Strings;
using ExpencesTracker.Mobile.ViewModels.Categories;

namespace ExpencesTracker.Mobile.Views.Settings.CategoriesSettings;

public partial class ExpenseCategoriesSettingsPage : BaseContentPage
{
	private readonly ExpenseCategoriesSettingsPageViewModel _viewModel;
	
	public ExpenseCategoriesSettingsPage(ExpenseCategoriesSettingsPageViewModel viewModel)
	{
		_viewModel = viewModel;
		BindingContext = _viewModel;
		
		InitializeComponent();

		CategoriesCollectionView.ItemsSource = _viewModel.Categories;
	}

	private void UpdateCategoriesList(object? sender, EventArgs e)
	{
		ProcessAction(async () =>
		{
			await _viewModel.Initialize();
		});
	}

	private void AddCategoryButton_OnClicked(object? sender, EventArgs e)
	{
		ProcessAction(async () =>
		{
			await OpenAddCategoryPage();
		});
	}

	private void SwipeItem_OnInvoked(object? sender, EventArgs e)
	{
		
	}
	
	private async Task OpenAddCategoryPage()
	{
		var addPage = Handler?.MauiContext?.Services.GetService<AddExpenseCategoryPage>();

		if (addPage is null)
		{
			throw new Exception(AppResources.ErrorAlert_OpenAddCategoryPage);
		}

		await Navigation.PushModalAsync(addPage);
	}
}
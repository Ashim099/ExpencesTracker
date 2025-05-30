using ExpencesTracker.Core.Domain.Model.Transactions.ValueObjects;
using ExpencesTracker.Mobile.Abstractions;
using ExpencesTracker.Mobile.Models.Categories;
using ExpencesTracker.Mobile.Resources.Strings;
using ExpencesTracker.Mobile.ViewModels.Home;
using ExpencesTracker.Mobile.Views.Transactions;

namespace ExpencesTracker.Mobile.Views.Home
{
    public partial class HomePage : BaseContentPage
    {
    	private readonly HomePageViewModel _viewModel;
	
    	public HomePage(HomePageViewModel viewModel)
    	{
    		_viewModel = viewModel;
    		BindingContext = _viewModel;
    		InitializeComponent();

    		CategoriesExpensesCollectionView.ItemsSource = _viewModel.CategoriesExpenses;
    	}

    	private void HomePage_OnNavigated(object? sender, EventArgs e)
    	{
    		ProcessAction(async () =>
    		{
    			await _viewModel.Initialize();
    		});
    	}

    	private void CategoryLayout_OnTapped(object? sender, TappedEventArgs e)
    	{
    		ProcessAction(async () =>
    		{
    			await OpenFilteredTransactionsByCategoryPage(sender, e);	
    		});
    	}
	
    	private void MainSpendingTypeLayout_OnTapped(object? sender, TappedEventArgs e)
    	{
    		SpendingTypeLayout_OnTapped(SpendingType.Main);
    	}

    	private void SecondarySpendingTypeLayout_OnTapped(object? sender, TappedEventArgs e)
    	{
    		SpendingTypeLayout_OnTapped(SpendingType.Secondary);
    	}

    	private void SavedSpendingTypeLayout_OnTapped(object? sender, TappedEventArgs e)
    	{
    		SpendingTypeLayout_OnTapped(SpendingType.Saved);
    	}
	
    	private void SpendingTypeLayout_OnTapped(SpendingType type)
    	{
    		ProcessAction(async () =>
    		{
    			await OpenFilteredTransactionsBySpendingTypePage(type);
    		});
    	}

    	private async Task OpenFilteredTransactionsByCategoryPage(object? sender, TappedEventArgs e)
    	{
    		if ((sender as StackLayout)?.BindingContext is not CategoryExpenseModel category)
    		{
    			throw new Exception(AppResources.ErrorAlert_GetCategoryInfo);
    		}
		
    		var filteredPage = Handler?.MauiContext?.Services.GetService<FilteredTransactionsPage>();

    		if (filteredPage is null)
    		{
    			throw new Exception(AppResources.ErrorAlert_ShowFilteredTransactions);
    		}
		
    		await filteredPage.Initialize(
    			_viewModel.ProfileId,
    			category.Id, 
    			spendingType: null, 
    			dateFrom: DateTime.Parse(_viewModel.DateFrom),
    			dateTo: DateTime.Parse(_viewModel.DateTo));
		
    		await Navigation.PushModalAsync(filteredPage);
    	}
	
    	private async Task OpenFilteredTransactionsBySpendingTypePage(SpendingType type)
    	{
    		var filteredPage = Handler?.MauiContext?.Services.GetService<FilteredTransactionsPage>();
		
    		if (filteredPage is null)
    		{
    			throw new Exception(AppResources.ErrorAlert_ShowFilteredTransactions);
    		}
		
    		await filteredPage.Initialize(
    			_viewModel.ProfileId,
    			categoryId: null, 
    			type,
    			dateFrom: DateTime.Parse(_viewModel.DateFrom),
    			dateTo: DateTime.Parse(_viewModel.DateTo));

    		await Navigation.PushModalAsync(filteredPage);
    	}
    }
}
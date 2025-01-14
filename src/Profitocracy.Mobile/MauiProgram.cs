using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using ExpencesTracker.Core;
using ExpencesTracker.Infrastructure;
using ExpencesTracker.Mobile.ViewModels.Categories;
using ExpencesTracker.Mobile.ViewModels.Home;
using ExpencesTracker.Mobile.ViewModels.Settings;
using ExpencesTracker.Mobile.ViewModels.Setup;
using ExpencesTracker.Mobile.ViewModels.Transactions;
using ExpencesTracker.Mobile.Views.Home;
using ExpencesTracker.Mobile.Views.Settings.CategoriesSettings;
using ExpencesTracker.Mobile.Views.Settings.LanguageSettings;
using ExpencesTracker.Mobile.Views.Settings.ThemeSettings;
using ExpencesTracker.Mobile.Views.Setup;
using ExpencesTracker.Mobile.Views.Transactions;

namespace ExpencesTracker.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.RegisterAppServices()
			.RegisterViewModels()
			.RegisterViews();

		
#if DEBUG
		builder.Logging.AddDebug();
#endif

		var infrastructureConfig = new InfrastructureConfiguration
		{
			AppDirectoryPath = FileSystem.AppDataDirectory 
		};
		
		builder.Services
			.RegisterInfrastructureServices(infrastructureConfig)
			.RegisterCoreServices()
			.AddSingleton<TransactionsPage>();

		return builder.Build();
	}
	
	private static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
	{
		_ = mauiAppBuilder.Services
			.AddSingleton<AppShell>()
			.AddSingleton<AppInit>();

		return mauiAppBuilder;
	}

	private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
	{
		_ = mauiAppBuilder.Services
			.AddTransient<HomePageViewModel>()
			.AddTransient<SetupPageViewModel>()
			.AddTransient<AddTransactionPageViewModel>()
			.AddTransient<FilteredTransactionsPageViewModel>()
			.AddTransient<TransactionsPageViewModel>()
			.AddTransient<ExpenseCategoriesSettingsPageViewModel>()
			.AddTransient<AddExpenseCategoryPageViewModel>()
			.AddTransient<LanguageSettingsViewModel>()
			.AddTransient<ThemeSettingsPageViewModel>();
		
		return mauiAppBuilder;
	}

	private static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
	{
		_ = mauiAppBuilder.Services
			.AddSingleton<HomePage>()
			.AddSingleton<SetupPage>()
			.AddSingleton<TransactionsPage>()
			.AddTransient<FilteredTransactionsPage>()
			.AddTransient<AddTransactionPage>()
			.AddTransient<ExpenseCategoriesSettingsPage>()
			.AddTransient<ThemeSettingsPage>()
			.AddTransient<LanguageSettingsPage>()
			.AddTransient<AddExpenseCategoryPage>();
		
		return mauiAppBuilder;
	}
}
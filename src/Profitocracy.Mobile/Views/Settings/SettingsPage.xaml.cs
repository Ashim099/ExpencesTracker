using ExpencesTracker.Mobile.Abstractions;
using ExpencesTracker.Mobile.Views.Settings.CategoriesSettings;
using ExpencesTracker.Mobile.Views.Settings.LanguageSettings;
using ExpencesTracker.Mobile.Views.Settings.ThemeSettings;

namespace ExpencesTracker.Mobile.Views.Settings;

public partial class SettingsPage : BaseContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}

	private void CategoriesButton_OnClicked(object? sender, EventArgs e)
	{
		ProcessAction(async () =>
		{
			var categoriesPage = Handler?.MauiContext?.Services.GetService<ExpenseCategoriesSettingsPage>();

			if (categoriesPage is not null)
			{
				await Navigation.PushAsync(categoriesPage);
			}
		});
	}

	private void ThemeButton_OnClicked(object? sender, TappedEventArgs e)
	{
		ProcessAction(async () =>
		{
			var themePage = Handler?.MauiContext?.Services.GetService<ThemeSettingsPage>();

			if (themePage is not null)
			{
				await Navigation.PushAsync(themePage);
			}
		});
	}

	private void LanguageButton_OnClicked(object? sender, TappedEventArgs e)
	{
		ProcessAction(async () =>
		{
			var langPage = Handler?.MauiContext?.Services.GetService<LanguageSettingsPage>();

			if (langPage is not null)
			{
				await Navigation.PushAsync(langPage);
			}
		});
	}
}
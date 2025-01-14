using ExpencesTracker.Mobile.Abstractions;
using ExpencesTracker.Mobile.Resources.Strings;
using ExpencesTracker.Mobile.Services;
using ExpencesTracker.Mobile.ViewModels.Settings;

namespace ExpencesTracker.Mobile.Views.Settings.LanguageSettings;

public partial class LanguageSettingsPage : BaseContentPage
{
    private readonly LanguageSettingsViewModel _viewModel;
    
    public LanguageSettingsPage(LanguageSettingsViewModel viewModel)
    {
        _viewModel = viewModel;
        InitializeComponent();
        
        BindingContext = _viewModel;
    }
    
    private void LanguageSettingsPage_OnLoaded(object? sender, EventArgs e)
    {
        ProcessAction(async () =>
        {
            await _viewModel.Initialize();
        });
    }
    
    private void EnglishLanguage_OnSelected(object? sender, TappedEventArgs e)
    {
        ProcessAction(async () =>
        {
            await ChangeLanguage(LocalizationService.English);   
        });
    }
    
    private void RussianLanguage_OnSelected(object? sender, TappedEventArgs e)
    {
        ProcessAction(async () =>
        {
            await ChangeLanguage(LocalizationService.Russian);
        });
    }

    private async Task ChangeLanguage(string language)
    {
        await _viewModel.ChangeLanguage(language);
        
        await DisplayAlert(
            AppResources.ChangeLanguageAlertTitle, 
            AppResources.ChangeLanguageAlertMessage,
            AppResources.ChangeLanguageAlertOk);
    }
}
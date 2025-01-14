using ExpencesTracker.Core.Domain.Model.Settings.ValueObjects;
using ExpencesTracker.Mobile.Abstractions;
using ExpencesTracker.Mobile.ViewModels.Settings;

namespace ExpencesTracker.Mobile.Views.Settings.ThemeSettings
{
    public partial class ThemeSettingsPage : BaseContentPage
    {
        private readonly ThemeSettingsPageViewModel _viewModel;

        public ThemeSettingsPage(ThemeSettingsPageViewModel viewModel)
        {
            _viewModel = viewModel;
            BindingContext = _viewModel;
            InitializeComponent();
        }

        private void ThemeSettingsPage_OnLoaded(object? sender, EventArgs e)
        {
            ProcessAction(async () =>
            {
                await _viewModel.Initialize();    
            });
        }

        private void LightTheme_OnSelected(object? sender, EventArgs e)
        {
            ProcessAction(async () =>
            {
                await ChangeTheme(Theme.Light); 
            });
        }

        private void DarkTheme_OnSelected(object? sender, EventArgs e)
        {
            ProcessAction(async () =>
            {
                await ChangeTheme(Theme.Dark); 
            });
        }

        private void SystemTheme_OnSelected(object? sender, EventArgs e)
        {
            ProcessAction(async () =>
            {
                await ChangeTheme(Theme.System); 
            });
        }

        private async Task ChangeTheme(Theme theme)
        {
            await _viewModel.ChangeTheme(theme);
        }
    }
}
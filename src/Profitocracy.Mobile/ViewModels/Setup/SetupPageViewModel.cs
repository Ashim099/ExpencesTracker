using System.Globalization;
using ExpencesTracker.Core.Domain.Model.Profiles.Factories;
using ExpencesTracker.Core.Persistence;
using ExpencesTracker.Mobile.Abstractions;
using ExpencesTracker.Mobile.Resources.Strings;

namespace ExpencesTracker.Mobile.ViewModels.Setup;

public class SetupPageViewModel : BaseNotifyObject
{
    private string _name = "";
    private string _initialBalance = "0";

    private readonly IProfileRepository _profileRepository;
    
    public SetupPageViewModel(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string InitialBalance
    {
        get => _initialBalance;
        set => SetProperty(ref _initialBalance, value);
    }

    public async Task CreateFirstProfile()
    {
        _initialBalance = _initialBalance.Replace(
            ",", 
            CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
        
        if (!decimal.TryParse(_initialBalance, out var numValue))
        {
            throw new Exception(AppResources.CommonError_BalanceNumber);
        }
        
        var profile = new ProfileBuilder()
            .AddName(_name)
            .AddStartDate(DateTime.Now, numValue)
            .AddIsCurrent(true)
            .Build();
        
        await _profileRepository.Create(profile);
    }
}
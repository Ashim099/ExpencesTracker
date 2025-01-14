using System.Collections.ObjectModel;
using ExpencesTracker.Core.Persistence;
using ExpencesTracker.Mobile.Abstractions;
using ExpencesTracker.Mobile.Models.Categories;
using ExpencesTracker.Mobile.Resources.Strings;

namespace ExpencesTracker.Mobile.ViewModels.Categories;

public class ExpenseCategoriesSettingsPageViewModel : BaseNotifyObject
{
    private readonly IProfileRepository _profileRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ExpenseCategoriesSettingsPageViewModel(
        IProfileRepository profileRepository,
        ICategoryRepository categoryRepository)
    {
        _profileRepository = profileRepository;
        _categoryRepository = categoryRepository;
    }

    public readonly ObservableCollection<CategoryModel> Categories = [];

    public async Task Initialize()
    {
        var profileId = await _profileRepository.GetCurrentProfileId();

        if (profileId is null)
        {
            throw new Exception(AppResources.CommonError_GetCurrentProfile);
        }

        var categories = await _categoryRepository.GetAllByProfileId((Guid)profileId);
        Categories.Clear();

        foreach (var category in categories)
        {
            Categories.Add(CategoryModel.FromDomain(category));
        }
    }
}
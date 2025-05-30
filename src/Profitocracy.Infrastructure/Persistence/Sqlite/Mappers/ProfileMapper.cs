using ExpencesTracker.Core.Domain.Model.Profiles;
using ExpencesTracker.Core.Domain.Model.Profiles.Factories;
using ExpencesTracker.Infrastructure.Abstractions.Internal;
using ExpencesTracker.Infrastructure.Persistence.Sqlite.Models.Profile;

namespace ExpencesTracker.Infrastructure.Persistence.Sqlite.Mappers;

internal class ProfileMapper : IInfrastructureMapper<Profile, ProfileModel>
{
	public Profile MapToDomain(ProfileModel model)
	{
		var builder = new ProfileBuilder(model.Id)
			.AddBalance(model.Balance)
			.AddSavedBalance(model.SavedBalance)
			.AddName(model.Name)
			.AddStartDate(model.StartTimestamp, model.InitialBalance)
			.AddCurrency(model.CurrencyCode, model.CurrencyName, model.CurrencySymbol)
			.AddIsCurrent(model.IsCurrent);

		if (model.Categories is null)
		{
			return builder.Build();
		}
		
		foreach (var modelCategory in model.Categories)
		{
			builder.AddCategoryExpense(
				modelCategory.CategoryId, 
				modelCategory.Name, 
				modelCategory.PlannedAmount);
		}

		return builder.Build();
	}

	public ProfileModel MapToModel(Profile entity)
	{
		var categories = entity.CategoriesExpenses
			.Select(c => new ProfileCategoryModel
			{
				CategoryId = c.Id,
				Name = c.Name,
				PlannedAmount = c.PlannedAmount
			})
			.ToList();
		
		return new ProfileModel
		{
			Id = entity.Id,
			Name = entity.Name,
			StartTimestamp = entity.StartDate.Timestamp,
			InitialBalance = entity.StartDate.InitialBalance,
			Balance = entity.Balance,
			SavedBalance = entity.SavedBalance,
			IsCurrent = entity.IsCurrent,
			Categories = categories,
			CurrencyCode = entity.Settings.Currency.Code,
			CurrencyName = entity.Settings.Currency.Name,
			CurrencySymbol = entity.Settings.Currency.Symbol
		};
	}
}
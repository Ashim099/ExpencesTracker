using ExpencesTracker.Core.Domain.Model.Categories;
using ExpencesTracker.Infrastructure.Abstractions.Internal;
using ExpencesTracker.Infrastructure.Persistence.Sqlite.Models.Category;

namespace ExpencesTracker.Infrastructure.Persistence.Sqlite.Mappers;

internal class CategoryMapper : IInfrastructureMapper<Category, CategoryModel>
{
	public Category MapToDomain(CategoryModel model)
	{
		return new Category(model.Id)
		{
			Name = model.Name,
			ProfileId = model.ProfileId,
			PlannedAmount = model.PlannedAmount
		};
	}

	public CategoryModel MapToModel(Category entity)
	{
		return new CategoryModel
		{
			Id = entity.Id,
			Name = entity.Name,
			ProfileId = entity.ProfileId,
			PlannedAmount = entity.PlannedAmount
		};
	}
}
using ExpencesTracker.Core.Domain.Model.Categories;
using ExpencesTracker.Core.Persistence;
using ExpencesTracker.Infrastructure.Abstractions.Internal;
using ExpencesTracker.Infrastructure.Persistence.Sqlite.Configuration;
using ExpencesTracker.Infrastructure.Persistence.Sqlite.Models.Category;

namespace ExpencesTracker.Infrastructure.Persistence.Sqlite.Repositories;

internal class CategoryRepository : ICategoryRepository
{
	private readonly DbConnection _dbConnection;
	private readonly IInfrastructureMapper<Category, CategoryModel> _mapper;
	
	public CategoryRepository(
		DbConnection connection,
		IInfrastructureMapper<Category, CategoryModel> mapper)
	{
		_dbConnection = connection;
		_mapper = mapper;
	}
	
	public async Task<List<Category>> GetAllByProfileId(Guid profileId)
	{
		await _dbConnection.Init();

		var categories = await _dbConnection.Database
			.Table<CategoryModel>()
			.Where(c => c.ProfileId.Equals(profileId))
			.OrderByDescending(c => c.PlannedAmount)
			.ToListAsync();

		var domainCategories = categories
			.Select(_mapper.MapToDomain)
			.ToList();

		return domainCategories;
	}

	public async Task<Category> Create(Category category)
	{
		await _dbConnection.Init();

		var categoryToCreate = _mapper.MapToModel(category);
		_ = await _dbConnection.Database.InsertAsync(categoryToCreate);

		var createdCategory = await _dbConnection.Database
			.Table<CategoryModel>()
			.Where(c => c.Id == categoryToCreate.Id)
			.FirstAsync();

		return _mapper.MapToDomain(createdCategory);
	}

	public Task<Guid> Delete(Guid categoryId)
	{
		throw new NotImplementedException();
	}
}
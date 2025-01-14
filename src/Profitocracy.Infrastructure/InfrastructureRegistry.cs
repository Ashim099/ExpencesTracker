using Microsoft.Extensions.DependencyInjection;
using ExpencesTracker.Core.Domain.Model.Categories;
using ExpencesTracker.Core.Domain.Model.Profiles;
using ExpencesTracker.Core.Domain.Model.Settings;
using ExpencesTracker.Core.Domain.Model.Transactions;
using ExpencesTracker.Core.Persistence;
using ExpencesTracker.Infrastructure.Abstractions.Internal;
using ExpencesTracker.Infrastructure.Persistence.Sqlite.Configuration;
using ExpencesTracker.Infrastructure.Persistence.Sqlite.Mappers;
using ExpencesTracker.Infrastructure.Persistence.Sqlite.Models.Category;
using ExpencesTracker.Infrastructure.Persistence.Sqlite.Models.Profile;
using ExpencesTracker.Infrastructure.Persistence.Sqlite.Models.Settings;
using ExpencesTracker.Infrastructure.Persistence.Sqlite.Models.Transaction;
using ExpencesTracker.Infrastructure.Persistence.Sqlite.Repositories;

namespace ExpencesTracker.Infrastructure;

public static class InfrastructureRegistry
{
	public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, InfrastructureConfiguration configuration)
	{
		return services
			.RegisterPersistence(configuration)
			.RegisterMappers()
			.RegisterRepositories();
	}
	
	private static IServiceCollection RegisterPersistence(this IServiceCollection services, InfrastructureConfiguration configuration)
	{
		return services
			.AddSingleton(configuration)
			.AddTransient<DbConnection>();
	}

	private static IServiceCollection RegisterMappers(this IServiceCollection services)
	{
		return services
			.AddTransient<IInfrastructureMapper<Transaction, TransactionModel>, TransactionMapper>()
			.AddTransient<IInfrastructureMapper<Profile, ProfileModel>, ProfileMapper>()
			.AddTransient<IInfrastructureMapper<Category, CategoryModel>, CategoryMapper>()
			.AddTransient<IInfrastructureMapper<Settings, SettingsModel>, SettingsMapper>();
	}

	private static IServiceCollection RegisterRepositories(this IServiceCollection services)
	{
		return services
			.AddTransient<ITransactionRepository, TransactionRepository>()
			.AddTransient<IProfileRepository, ProfileRepository>()
			.AddTransient<ICategoryRepository, CategoryRepository>()
			.AddTransient<ISettingsRepository, SettingsRepository>();
	}
}
using ExpencesTracker.Core.Domain.Model.Profiles;
using ExpencesTracker.Core.Persistence;
using ExpencesTracker.Infrastructure.Abstractions.Internal;
using ExpencesTracker.Infrastructure.Persistence.Sqlite.Configuration;
using ExpencesTracker.Infrastructure.Persistence.Sqlite.Models.Profile;

namespace ExpencesTracker.Infrastructure.Persistence.Sqlite.Repositories;

internal class ProfileRepository : IProfileRepository
{
	private readonly DbConnection _dbConnection;
	private readonly IInfrastructureMapper<Profile, ProfileModel> _mapper;
	
	public ProfileRepository(
		DbConnection connection,
		IInfrastructureMapper<Profile, ProfileModel> mapper)
	{
		_dbConnection = connection;
		_mapper = mapper;
	}
	
	public async Task<Profile> Create(Profile profile)
	{
		await _dbConnection.Init();
		
		var profileToCreate = _mapper.MapToModel(profile);
		_ = await _dbConnection.Database.InsertAsync(profileToCreate);

		var createdProfile = await _dbConnection.Database
			.Table<ProfileModel>()
			.Where(p => p.Id.Equals(profile.Id))
			.FirstAsync();

		return _mapper.MapToDomain(createdProfile);
	}

	public async Task<Profile> Update(Profile profile)
	{
		await _dbConnection.Init();

		var profileToUpdate = _mapper.MapToModel(profile);
		_ = await _dbConnection.Database.UpdateAsync(profileToUpdate);
		
		var updatedProfile = await _dbConnection.Database
			.Table<ProfileModel>()
			.Where(p => p.Id.Equals(profile.Id))
			.FirstAsync();

		return _mapper.MapToDomain(updatedProfile);
	}

	public async Task<Guid?> GetCurrentProfileId()
	{
		await _dbConnection.Init();

		var profile = await _dbConnection.Database
			.Table<ProfileModel>()
			.Where(p => p.IsCurrent)
			.FirstOrDefaultAsync();

		return profile?.Id;
	}

	public async Task<Profile?> GetCurrentProfile()
	{
		await _dbConnection.Init();
		
		var profile = await _dbConnection.Database
			.Table<ProfileModel>()
			.Where(p => p.IsCurrent)
			.FirstOrDefaultAsync();

		return profile is null 
			? null 
			: _mapper.MapToDomain(profile);
	}
}
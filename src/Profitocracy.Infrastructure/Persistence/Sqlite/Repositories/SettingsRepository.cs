using ExpencesTracker.Core.Domain.Model.Settings;
using ExpencesTracker.Core.Persistence;
using ExpencesTracker.Infrastructure.Abstractions.Internal;
using ExpencesTracker.Infrastructure.Persistence.Sqlite.Configuration;
using ExpencesTracker.Infrastructure.Persistence.Sqlite.Models.Settings;

namespace ExpencesTracker.Infrastructure.Persistence.Sqlite.Repositories;

internal class SettingsRepository : ISettingsRepository
{
    private readonly DbConnection _dbConnection;
    private readonly IInfrastructureMapper<Settings, SettingsModel> _mapper;

    public SettingsRepository(
        DbConnection dbConnection,
        IInfrastructureMapper<Settings, SettingsModel> mapper)
    {
        _mapper = mapper;
        _dbConnection = dbConnection;
    }

    public async Task<Settings?> GetCurrentSettings()
    {
        await _dbConnection.Init();
        
        var settings = await _dbConnection.Database
            .Table<SettingsModel>()
            .FirstOrDefaultAsync();

        return settings is null 
            ? null 
            : _mapper.MapToDomain(settings);
    }

    public async Task<Settings> CreateOrUpdate(Settings settings)
    {
        await _dbConnection.Init();
        
        var settingsToEdit = _mapper.MapToModel(settings);
        
        var settingsModel = await _dbConnection.Database
            .Table<SettingsModel>()
            .FirstOrDefaultAsync();

        if (settingsModel is null)
        {
            _ = await _dbConnection.Database.InsertAsync(settingsToEdit);   
        }
        else
        {
            _ = await _dbConnection.Database.UpdateAsync(settingsToEdit);
        }
        
        settingsModel = await _dbConnection.Database
            .Table<SettingsModel>()
            .FirstOrDefaultAsync();

        return _mapper.MapToDomain(settingsModel);
    }
}
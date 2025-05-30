using ExpencesTracker.Core.Domain.Model.Profiles;

namespace ExpencesTracker.Core.Persistence;

/// <summary>
/// Provides operations for working with
/// persistence layer for profiles
/// </summary>
public interface IProfileRepository
{
	/// <summary>
	/// Get current active profile ID
	/// </summary>
	/// <returns>ID of the current profile if exists, otherwise, null</returns>
	Task<Guid?> GetCurrentProfileId();
	
	/// <summary>
	/// Get current active profile
	/// </summary>
	/// <returns>Current profile if exists, otherwise, null</returns>
	Task<Profile?> GetCurrentProfile();
	
	/// <summary>
	/// Create new profile
	/// </summary>
	/// <param name="profile">New profile to create</param>
	/// <returns>Created profile</returns>
	Task<Profile> Create(Profile profile);
	
	/// <summary>
	/// Update profile
	/// </summary>
	/// <param name="profile">Profile to update</param>
	/// <returns>Updated profile</returns>
	Task<Profile> Update(Profile profile);
}
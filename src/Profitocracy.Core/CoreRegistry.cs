using Microsoft.Extensions.DependencyInjection;
using ExpencesTracker.Core.Domain.Abstractions.Services;
using ExpencesTracker.Core.Domain.Services;

namespace ExpencesTracker.Core
{
    public static class CoreRegistry
    {
        public static IServiceCollection RegisterCoreServices(this IServiceCollection services) => services.AddTransient<IProfileService, ProfileService>();
    }
}
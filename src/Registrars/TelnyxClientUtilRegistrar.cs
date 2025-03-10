using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Telnyx.Client.Abstract;
using Soenneker.Utils.HttpClientCache.Registrar;

namespace Soenneker.Telnyx.Client.Registrars;

/// <summary>
/// A .NET thread-safe singleton HttpClient for Telnyx
/// </summary>
public static class TelnyxClientUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="ITelnyxClientUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddTelnyxClientUtilAsSingleton(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton();
        services.TryAddSingleton<ITelnyxClientUtil, TelnyxClientUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="ITelnyxClientUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddTelnyxClientUtilAsScoped(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton();
        services.TryAddScoped<ITelnyxClientUtil, TelnyxClientUtil>();

        return services;
    }
}

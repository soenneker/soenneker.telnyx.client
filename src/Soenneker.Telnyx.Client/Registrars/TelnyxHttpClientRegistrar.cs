using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Telnyx.Client.Abstract;
using Soenneker.Utils.HttpClientCache.Registrar;

namespace Soenneker.Telnyx.Client.Registrars;

/// <summary>
/// A .NET thread-safe singleton HttpClient for Telnyx
/// </summary>
public static class TelnyxHttpClientRegistrar
{
    /// <summary>
    /// Adds <see cref="ITelnyxHttpClient"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddTelnyxHttpClientAsSingleton(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton().TryAddSingleton<ITelnyxHttpClient, TelnyxHttpClient>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="ITelnyxHttpClient"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddTelnyxHttpClientAsScoped(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton().TryAddScoped<ITelnyxHttpClient, TelnyxHttpClient>();

        return services;
    }
}
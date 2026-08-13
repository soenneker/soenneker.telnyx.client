using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Soenneker.Dtos.HttpClientOptions;
using Soenneker.Extensions.Configuration;
using Soenneker.HttpClients.LoggingHandler;
using Soenneker.Telnyx.Client.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;

namespace Soenneker.Telnyx.Client;

/// <inheritdoc cref="ITelnyxHttpClient"/>
public sealed class TelnyxHttpClient : ITelnyxHttpClient
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TelnyxHttpClient> _logger;

    private const string _clientId = nameof(TelnyxHttpClient);
    private static readonly Uri _prodBaseUrl = new("https://api.telnyx.com/v2/", UriKind.Absolute);

    public TelnyxHttpClient(IHttpClientCache httpClientCache, IConfiguration configuration, ILogger<TelnyxHttpClient> logger)
    {
        _httpClientCache = httpClientCache;
        _configuration = configuration;
        _logger = logger;
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        // No closure: state passed explicitly + static lambda
        return _httpClientCache.Get(_clientId, (configuration: _configuration, logger: _logger, prodBaseUrl: _prodBaseUrl), static state =>
        {
            var token = state.configuration.GetValueStrict<string>("Telnyx:Token");
            bool logging = state.configuration.GetValue<bool>("Telnyx:RequestResponseLogging");

            HttpMessageHandler? pipeline = null;

            if (logging)
            {
                pipeline = new HttpClientLoggingHandler(state.logger, new HttpClientLoggingOptions
                {
                    LogLevel = LogLevel.Debug,
                    RedactedHeaders = ["Authorization"]
                })
                {
                    InnerHandler = new HttpClientHandler()
                };
            }

            return new HttpClientOptions
            {
                BaseAddress = state.prodBaseUrl,
                HttpMessageHandler = pipeline,
                DefaultRequestHeaders = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "Authorization", $"Bearer {token}" }
                }
            };
        }, cancellationToken);
    }

    /// <summary>
    /// Releases resources used by the current instance.
    /// </summary>
    public void Dispose()
    {
        _httpClientCache.RemoveSync(_clientId);
    }

    /// <summary>
    /// Asynchronously releases resources used by the current instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public ValueTask DisposeAsync()
    {
        return _httpClientCache.Remove(_clientId);
    }
}

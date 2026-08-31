using System;
using System.Collections.Generic;
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

public sealed class TelnyxHttpClient : ITelnyxHttpClient
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TelnyxHttpClient> _logger;

    private readonly string _clientId = $"{nameof(TelnyxHttpClient)}-{Guid.NewGuid():N}";
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

            List<Func<DelegatingHandler>>? handlerFactories = null;

            if (logging)
            {
                handlerFactories =
                [
                    () => new HttpClientLoggingHandler(state.logger, new HttpClientLoggingOptions
                    {
                        LogLevel = LogLevel.Debug,
                        RedactedHeaders = ["Authorization"]
                    })
                ];
            }

            return new HttpClientOptions
            {
                BaseAddress = state.prodBaseUrl,
                DelegatingHandlerFactories = handlerFactories,
                DefaultRequestHeaders = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "Authorization", $"Bearer {token}" }
                }
            };
        }, cancellationToken);
    }

    public void Dispose()
    {
        _httpClientCache.RemoveSync(_clientId);
    }

    public ValueTask DisposeAsync()
    {
        return _httpClientCache.Remove(_clientId);
    }
}

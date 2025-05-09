using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Soenneker.Dtos.HttpClientOptions;
using Soenneker.Extensions.Configuration;
using Soenneker.Telnyx.Client.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;

namespace Soenneker.Telnyx.Client;

/// <inheritdoc cref="ITelnyxClientUtil"/>
public sealed class TelnyxClientUtil : ITelnyxClientUtil
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly IConfiguration _configuration;

    private const string _clientId = nameof(TelnyxClientUtil);
    private const string _prodBaseUrl = "https://api.telnyx.com/v2/";

    public TelnyxClientUtil(IHttpClientCache httpClientCache, IConfiguration configuration)
    {
        _httpClientCache = httpClientCache;
        _configuration = configuration;
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        return _httpClientCache.Get(_clientId, () =>
        {
            var token = _configuration.GetValueStrict<string>("Telnyx:Token");

            return new HttpClientOptions
            {
                BaseAddress = _prodBaseUrl,
                DefaultRequestHeaders = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "Authorization", $"Bearer {token}" }
                }
            };
        }, cancellationToken);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _httpClientCache.RemoveSync(_clientId);
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return _httpClientCache.Remove(_clientId);
    }
}
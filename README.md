[![](https://img.shields.io/nuget/v/soenneker.telnyx.client.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.telnyx.client/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.telnyx.client/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.telnyx.client/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.telnyx.client.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.telnyx.client/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.telnyx.client/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.telnyx.client/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Telnyx.Client

Provides a cached `HttpClient` configured with Telnyx's REST API base address, bearer-token authentication, and optional request/response logging.

## Installation

```bash
dotnet add package Soenneker.Telnyx.Client
```

## Configuration

```json
{
  "Telnyx": {
    "Token": "KEY...",
    "RequestResponseLogging": false
  }
}
```

## Usage

```csharp
using Soenneker.Telnyx.Client.Abstract;
using Soenneker.Telnyx.Client.Registrars;

services.AddTelnyxHttpClientAsSingleton();

HttpClient client = await telnyxHttpClient.Get(cancellationToken);
HttpResponseMessage response = await client.GetAsync(
    "messaging_profiles",
    cancellationToken);

response.EnsureSuccessStatusCode();
```

The base address is `https://api.telnyx.com/v2/`, so relative request paths should not start with `/`. The provider owns its cached client; callers should not dispose the value returned by `Get` independently.

When `RequestResponseLogging` is enabled, the Authorization header is redacted, but request and response bodies may still contain phone numbers, message content, or other sensitive data. Enable it only where that output is appropriately protected.

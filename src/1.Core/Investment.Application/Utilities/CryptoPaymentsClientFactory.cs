namespace Investment.Application.Utilities;

using Investment.Application.Utilities.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class CryptoPaymentsClientFactory
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _apiKey;
    private readonly JsonSerializerOptions _jsonOptions;
    public const string ClientFactoryName = "NowPayments";
    public CryptoPaymentsClientFactory(IHttpClientFactory httpClientFactory, IConfiguration config, IOptions<CryptoPaymentConfig> cryptoPaymentConfigOptions)
    {
        _httpClientFactory = httpClientFactory;
        _apiKey = cryptoPaymentConfigOptions.Value.ApiKey;

        _jsonOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
        };
    }

    private HttpClient CreateClient(int timeoutToSecond)
    {
        var client = _httpClientFactory.CreateClient(ClientFactoryName);
        client.Timeout = TimeSpan.FromSeconds(timeoutToSecond);
        client.DefaultRequestHeaders.Remove("x-api-key");
        client.DefaultRequestHeaders.Add("x-api-key", _apiKey);
        return client;
    }

    public async Task<T> SendAsync<T>(
        HttpMethod method,
        string relativeUrl,
         int timeoutToSecond,
        object? body = null,
        CancellationToken cancellationToken = default)
    {
        using var client = CreateClient(timeoutToSecond);

        using var request = new HttpRequestMessage(method, relativeUrl);

        if (body is not null)
        {
            var json = JsonSerializer.Serialize(body, _jsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        string raw = await response.Content.ReadAsStringAsync(cancellationToken);


        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                $"Code 200 not received from http with address :{client.BaseAddress}\n" +
                $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}\n" +
                $"Body:\n{raw}"
            );
        }

        // Some endpoints may return empty body; handle gracefully.


        var data = JsonSerializer.Deserialize<T>(raw, _jsonOptions) ?? throw new Exception("An error occurred while deserializing the object received from nowPayment");

        return data;
    }

    public async Task SendAsync(
       HttpMethod method,
       string relativeUrl,
       int timeoutToSecond,
       object? body = null,
       CancellationToken ct = default)
    {
        using var client = CreateClient(timeoutToSecond);

        using var request = new HttpRequestMessage(method, relativeUrl);

        if (body is not null)
        {
            var json = JsonSerializer.Serialize(body, _jsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);

        string raw = await response.Content.ReadAsStringAsync(ct);


        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                $"Code 200 not received from http with address :{client.BaseAddress}\n" +
                $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}\n" +
                $"Body:\n{raw}"
            );
        }

        // Some endpoints may return empty body; handle gracefully.
    }

    public Task<T> PostAsync<T>(string relativeUrl, object? body, int timeoutToSecond = 20, CancellationToken cancellationToken = default) =>
    SendAsync<T>(HttpMethod.Post, relativeUrl, timeoutToSecond, body, cancellationToken);

    public async Task<T> SendWithJwtTokenAsync<T>(
        HttpMethod method,
        string relativeUrl,
         int timeoutToSecond,
        string jwtToken,
        object? body = null,
        CancellationToken cancellationToken = default)
    {
        using var client = CreateClientWithJwtToken(jwtToken, timeoutToSecond);

        using var request = new HttpRequestMessage(method, relativeUrl);

        if (body is not null)
        {
            var json = JsonSerializer.Serialize(body, _jsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        string raw = await response.Content.ReadAsStringAsync(cancellationToken);


        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                $"Code 200 not received from http with address :{client.BaseAddress}\n" +
                $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}\n" +
                $"Body:\n{raw}"
            );
        }

        // Some endpoints may return empty body; handle gracefully.


        var data = JsonSerializer.Deserialize<T>(raw, _jsonOptions) ?? throw new Exception("An error occurred while deserializing the object received from nowPayment");

        return data;
    }

    public async Task SendWithJwtTokenAsync(
        HttpMethod method,
        string relativeUrl,
         int timeoutToSecond,
        string jwtToken,
        object? body = null,
        CancellationToken cancellationToken = default)
    {
        using var client = CreateClientWithJwtToken(jwtToken, timeoutToSecond);

        using var request = new HttpRequestMessage(method, relativeUrl);

        if (body is not null)
        {
            var json = JsonSerializer.Serialize(body, _jsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        string raw = await response.Content.ReadAsStringAsync(cancellationToken);


        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                $"Code 200 not received from http with address :{client.BaseAddress}\n" +
                $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}\n" +
                $"Body:\n{raw}"
            );
        }
    }

    private HttpClient CreateClientWithJwtToken(string jwtToken, int timeoutToSecond)
    {
        var client = _httpClientFactory.CreateClient(ClientFactoryName);
        client.Timeout = TimeSpan.FromSeconds(timeoutToSecond);
        client.DefaultRequestHeaders.Add("x-api-key", _apiKey);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        return client;
    }

    public Task<T> PostWithJwtTokenAsync<T>(string relativeUrl, string jwtToken, object? body, int timeoutToSecond = 20, CancellationToken cancellationToken = default) =>
    SendWithJwtTokenAsync<T>(HttpMethod.Post, relativeUrl, timeoutToSecond, jwtToken, body, cancellationToken);

    public Task PostWithJwtTokenAsync(string relativeUrl, string jwtToken, object? body, int timeoutToSecond = 20, CancellationToken cancellationToken = default) =>
    SendWithJwtTokenAsync(HttpMethod.Post, relativeUrl, timeoutToSecond, jwtToken, body, cancellationToken);

    public Task<T> GetAsync<T>(string relativeUrl, int timeoutToSecond = 20, CancellationToken ct = default) =>
        SendAsync<T>(HttpMethod.Get, relativeUrl, timeoutToSecond, null, ct);

    public async Task GetAsync<T>(object minimumPaymentAmountPath, int timeoutToSecond, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
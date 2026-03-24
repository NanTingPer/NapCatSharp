using NapCatSharp.RequestModels;
using System.Text.Json;

namespace NapCatSharp.Core;

public partial class NapCatHttpServer : IDisposable
{
    private readonly HttpClient httpClient;

    public Uri Uri { get; set; }
    public string Password { get; set; }

    private bool isDispose = false;

    public NapCatHttpServer(Uri uri, string password, HttpClient? httpClient = null)
    {
        Password = password;
        Uri = uri;
        
        this.httpClient = httpClient ??= new HttpClient(new SocketsHttpHandler()
        {
            AllowAutoRedirect = true, // 可被重定向 http -> https
            MaxAutomaticRedirections = 10, // 防止死循
            PooledConnectionLifetime = TimeSpan.FromSeconds(5), // 5s刷新，以更新dns等信息
            SslOptions = 
            {
                 AllowRenegotiation = true,
                 AllowTlsResume = true,
            },
        });
        try {
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Password}");
        } catch {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Password);
        }
    }

    /// <summary>
    /// 释放维护的<see cref="HttpClient"/>
    /// </summary>
    public void Dispose()
    {
        if(isDispose == false) {
            httpClient.Dispose();
            isDispose = true;
            GC.SuppressFinalize(this);
        }
    }

    private string GetServerURL()
    {
        return $"http://{Uri.Host}:{Uri.Port}";
    }

    private string GetAPIEndpoint(RequestModelBase request)
    {
        return GetServerURL() + request.GetEndpoint();
    }

    private Task<HttpResponseMessage> Post(RequestModelBase model) 
        => httpClient.PostAsync(GetAPIEndpoint(model), model.ToJsonStringContent());

    private async Task<TResponse?> Post<TResponse>(RequestModelBase model)
        where TResponse : class
    {
        var response = await httpClient.PostAsync(GetAPIEndpoint(model), model.ToJsonStringContent());
        var responseResult = await response.Content.ReadAsStringAsync();
        if (responseResult == null) return null;
        return JsonSerializer.Deserialize<TResponse>(responseResult);
    }
}

using NapCatSharp.RequestModels;

namespace NapCatSharp.Core;

public partial class NapCatHttpServer : IDisposable
{
    private readonly HttpClient httpClient;

    public required Uri Uri { get; set; }
    public required string Password { get; set; }

    private bool isDispose = false;

    public NapCatHttpServer()
    {
        httpClient = new HttpClient();
    }

    public NapCatHttpServer(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    /// <summary>
    /// 释放维护的<see cref="HttpClient"/>
    /// </summary>
    public void Dispose()
    {
        if(isDispose == false) {
            GC.SuppressFinalize(this);
            httpClient.Dispose();
            isDispose = true;
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
}

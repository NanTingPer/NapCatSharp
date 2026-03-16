using NapCatSharp.OB11;
using NapCatSharp.OB11.OB11MessageModels;
using NapCatSharp.RequestModels;
using NapCatSharp.RequestModels.MessageInterfaces;

namespace NapCatSharp.Core;

public class NapCatHttpServer : IDisposable
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

    /// <summary>
    /// 发送私聊文本
    /// </summary>
    /// <param name="text"> 文本内容 </param>
    /// <param name="userid"> 用户id </param>
    /// <returns></returns>
    public async Task SendPrivateTextMsg(string text, string userid)
    {
        var msg = new SendPrivateMsg()
        {
            UserId = userid,
            Message = [
                new Text()
                {
                    Data = new Text.OB11MessageText()
                    {
                        Text = text
                    }
                }
            ],
        };

        await httpClient.PostAsync(GetAPIEndpoint(msg), msg.ToJsonStringContent());
    }

    /// <summary>
    /// 发送私聊消息
    /// </summary>
    /// <returns></returns>
    public async Task SendPrivateMsg(List<IOB11MessageModelFlag> msgs, string userid)
    {
        var msg = new SendPrivateMsg()
        {
            UserId = userid,
            Message = msgs
        };
        await httpClient.PostAsync(GetAPIEndpoint(msg), msg.ToJsonStringContent());
    }
}

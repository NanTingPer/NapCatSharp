namespace NapCatSharp.Exceptions;

/// <summary>
/// Socket连接失败错误
/// </summary>
/// <param name="message"></param>
public class SocketConnectionException(string message) : Exception
{
    public override string Message => message;
}
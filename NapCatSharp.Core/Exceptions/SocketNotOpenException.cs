namespace NapCatSharp.Exceptions;

/// <summary>
/// Socket未被打开错误
/// </summary>
/// <param name="message"></param>
public class SocketNotOpenException(string message) : Exception
{
    public override string Message => message;
}

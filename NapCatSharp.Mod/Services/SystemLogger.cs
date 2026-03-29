using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Text;

namespace NapCatSharp.Mod.Services;

public class SystemLogger : Serilog.ILogger
{
    public readonly Serilog.ILogger logger;
    public SystemLogger()
    {
        var logsBaseDir = Path.Combine(AppContext.BaseDirectory, "logs", "systemlogs");
        logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(path: Path.Combine(logsBaseDir, "waring.log"), restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning, encoding: Encoding.UTF8)
            .WriteTo.File(path: Path.Combine(logsBaseDir, "debug.log"), restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug, encoding: Encoding.UTF8)
            .WriteTo.File(path: Path.Combine(logsBaseDir, "error.log"), restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error, encoding: Encoding.UTF8)
            .WriteTo.File(path: Path.Combine(logsBaseDir, "info.log"), restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information, encoding: Encoding.UTF8)
            .CreateLogger();
    }
    public void Write(LogEvent logEvent)
    {
        logger.Write(logEvent);
    }

    [MessageTemplateFormatMethod("messageTemplate")]
    public void Write(LogEventLevel level, string messageTemplate)
    {
        logger.Write(level, messageTemplate);
    }

    [MessageTemplateFormatMethod("messageTemplate")]
    public void Debug(string messageTemplate)
    {
        Write(LogEventLevel.Debug, messageTemplate);
    }

    [MessageTemplateFormatMethod("messageTemplate")]
    public void Information(string messageTemplate)
    {
        Write(LogEventLevel.Information, messageTemplate);
    }

    [MessageTemplateFormatMethod("messageTemplate")]
    public void Info(string messageTemplate)
    {
        Write(LogEventLevel.Information, messageTemplate);
    }

    [MessageTemplateFormatMethod("messageTemplate")]
    public void Warning(string messageTemplate)
    {
        Write(LogEventLevel.Warning, messageTemplate);
    }

    [MessageTemplateFormatMethod("messageTemplate")]
    public void Error(string messageTemplate)
    {
        Write(LogEventLevel.Error, messageTemplate);
    }
}

using NapCatSharp.Mod.Core;
using NapCatSharp.Mod.Extensions;
using NapCatSharp.Mod.Middlewares;
using NapCatSharp.Mod.Services;
using Serilog;
using System.Text;

#region Log
var logsBaseDir = Path.Combine(AppContext.BaseDirectory, "logs");
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(path: Path.Combine(logsBaseDir, "waring.log"), restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning, encoding: Encoding.UTF8)
    .WriteTo.File(path: Path.Combine(logsBaseDir, "debug.log"), restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug, encoding: Encoding.UTF8)
    .WriteTo.File(path: Path.Combine(logsBaseDir, "error.log"), restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error, encoding: Encoding.UTF8)
    .WriteTo.File(path: Path.Combine(logsBaseDir, "info.log"), restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information, encoding: Encoding.UTF8)
    .CreateLogger();
#endregion
var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddSerilog(Log.Logger);
var sl = new SystemLogger();
ModLoader.logger = sl;
builder.Services.AddSingleton<SystemLogger>(sl);
builder.Services
    .AddNapCatSharpServices()
    .AddControllers()
    .AddControllersAsServices();
// Add services to the container.
builder.Services
    .AddControllersWithViews(); // MVCBuilder

builder.Services
    .AddModSwaggerGen()
    .AddSingleton<JWTMiddleware>()
    //.AddAuthentication()
    ;
var app = builder.Build();
app.UseCors(cors => {
        cors.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()
    ;
});
app.UseRouting()
    //.UseAuthentication()
    //.UseAuthorization()
    .UseMiddleware<JWTMiddleware>();
app.MapDefaultEndpoints();
app.UseModSwagger();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
//app.Urls.Add("http://0.0.0.0:7044");
app.Run();
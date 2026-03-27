using NapCatSharp.Mod.Extensions;
using NapCatSharp.Mod.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
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

using NapCatSharp.Mod.BackgroundServices;
using NapCatSharp.Mod.Core;
using NapCatSharp.Mod.Extensions;



var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services
    .AddControllers()
    .AddControllersAsServices();
// Add services to the container.
builder.Services
    .AddControllersWithViews(); // MVCBuilder

builder.Services
    .AddModSwaggerGen()
    ;

var app = builder.Build();
app.MapDefaultEndpoints();
app.UseModSwagger();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseRouting();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

using System.Configuration;
using portalPoC.lib.Models;
using portalPoC.lib.Services;
using portalPoC.lib.Services.Interfaces;
using portalPoC.mvc.Models;

var builder = WebApplication.CreateBuilder(args);

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var domainRegistrationSettings = config.GetSection("DomainRegistrationSettings"); //.Get<DomainRegistrationSettings>();

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ITransipService, TransipService>();
builder.Services.AddScoped<IDomainService, DomainService>();
builder.Services.AddScoped<IBindingService, BindingService>();
builder.Services.AddScoped<ILesslService, LesslService>();

builder.Services.Configure<DomainRegistrationSettings>(domainRegistrationSettings);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

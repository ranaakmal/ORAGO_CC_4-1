using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using ORAGO_CC_4.Areas.Identity;
using ORAGO_CC_4.Data;
using Syncfusion.Blazor;
using System.Globalization;
using ORAGO_CC_4.Shared;
using ORAGO_CC_4.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Localization;
using ORAGO_CC_4;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var ORAGOCC4ConnectionString = builder.Configuration.GetConnectionString("ORAGOCC4Connection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDbContextFactory<ORAGOCC4Context>(options =>
    options.UseSqlite(ORAGOCC4ConnectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddControllers();

builder.Services.AddSyncfusionBlazor();
            builder.Services.AddSingleton(typeof(ISyncfusionStringLocalizer), typeof(SyncfusionLocalizer));
           var supportedCultures = new[] { "en-US", "de", "fr", "ar", "zh" };
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures); 

builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

builder.Services.AddServices(typeof(IGenericRepository<>).GetTypeInfo().Assembly, typeof(IGenericRepository<>));


var app = builder.Build();
app.UseRequestLocalization(localizationOptions);

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTE0OTEwN0AzMjMwMmUzNDJlMzBCOEZzbnBFbDlPL21rU3ovd1FWbmtQczAzNzJnVkNJYTVVR1FmQjVYSk1vPQ==");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

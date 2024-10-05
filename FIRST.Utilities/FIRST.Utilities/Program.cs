using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FIRST.Utilities.Client.Pages;
using FIRST.Utilities.Components;
using FIRST.Utilities.Components.Account;
using FIRST.Utilities.Configuration;
using FIRST.Utilities.Data;
using FIRST.Utilities.DataServices;
using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Entities;
using FIRST.Utilities.Repositories;
using FIRST.Utilities.Repositories.Interfaces;
using FIRST.Utilities.WebServices;
using FIRST.Utilities.WebServices.Interfaces;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.Configure<FtcApiOptions>(
    builder.Configuration.GetSection(FtcApiOptions.OptionName));

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

// API Services
builder.Services.AddScoped<IFtcApiService, FtcApiService>();

// Database Services
builder.Services.AddScoped<IFtcEventDataService, FtcEventDataService>();
builder.Services.AddScoped<IActiveProgramSeasonDataService, ActiveProgramSeasonDataService>();
builder.Services.AddScoped<IFtcTeamDataService, FtcTeamDataService>();
builder.Services.AddScoped<IFtcMatchDataService, FtcMatchDataService>();

// Repositories
builder.Services.AddScoped<IFtcEventRepository, FtcEventRepository>();
builder.Services.AddScoped<IActiveFtcEventRepository, ActiveFtcEventRepository>();
builder.Services.AddScoped<IActiveProgramSeasonRepository, ActiveProgramSeasonRepository>();
builder.Services.AddScoped<IFtcTeamRepository, FtcTeamRepository>();
builder.Services.AddScoped<IFtcMatchRepository, FtcMatchRepository>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddHttpClient("ftc-api",(serviceProvider, client) =>
{
    var apiSettings = serviceProvider
        .GetRequiredService<IOptions<FtcApiOptions>>().Value;
    
    var basicAuthenticationValue = Convert.ToBase64String(
        Encoding.ASCII.GetBytes(
            $"{apiSettings.Username}:{apiSettings.Password}"
        )
    );
    
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthenticationValue);
    client.BaseAddress = new Uri(apiSettings.BaseUrl);
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
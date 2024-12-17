using Cloud_Store.Components;
using Cloud_Store.Data;
using Cloud_Store.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

builder = InitCoreServicesBuilder(builder);


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}


// Then initialize core services
app = InitCoreServicesApp(app);

app.Run();

static WebApplicationBuilder InitCoreServicesBuilder(WebApplicationBuilder builder)
{ 
    // Initialize SQLite provider
    SQLitePCL.Batteries.Init();

    // Add services to the container.
    // Add Razor Components
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();
    builder.Services.AddRadzenComponents();

    // Add the Authentication and Authorization services
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddAuthenticationCore();
    builder.Services.AddCascadingAuthenticationState();
    builder.Services.AddAuthorization();

    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

    builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie(options =>
        {
            options.Cookie.Name = "cloud_store_cookie";
            options.LoginPath = "/";
            options.LogoutPath = "/logout";
            options.AccessDeniedPath = "/access-denied";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(45);
            
            options.Events = new CookieAuthenticationEvents
            {
                OnRedirectToLogin = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api"))
                    {
                        context.Response.Headers.Append("WWW-Authenticate", "Basic");
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    }
                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                },
                OnRedirectToAccessDenied = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api"))
                    {
                        context.Response.StatusCode = 403;
                        return Task.CompletedTask;
                    }
                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                }
            };
        });

    // Add the database context
    builder.Services.AddDbContext<CloudStoreContext>(options =>
        options.UseSqlite("data source=Database/cloud_store.db"));

    // Add File, Hashing and User Management Services
    builder.Services.AddScoped<IFileService, FileService>();
    builder.Services.AddScoped<IHashingService, HashingService>();
    builder.Services.AddScoped<UserService>();

    // Increase the maximum request size for lager file uploads
    // Increase Kestrel server limits
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.Limits.MaxRequestBodySize = 5L * 1024 * 1024 * 1024; // 5GB
    });
    // Configure form options
    builder.Services.Configure<FormOptions>(options =>
    {
        options.MultipartBodyLengthLimit = 5L * 1024 * 1024 * 1024; // 5GB
        options.ValueLengthLimit = int.MaxValue;
        options.MultipartHeadersLengthLimit = int.MaxValue;
    });
    return builder;
}

static WebApplication InitCoreServicesApp(WebApplication app)
{
    app.UseHttpsRedirection();
    app.UseAntiforgery();

    // Configure authentication/authorization for non-API routes
    app.UseWhen(
        context => !context.Request.Path.StartsWithSegments("/api"),
        builder => 
        {
            builder.UseAuthentication();
            builder.UseAuthorization();
        }
    );

    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    return app;
}

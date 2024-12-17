using Cloud_Store.Api;
using Cloud_Store.Api.Middleware;
using Cloud_Store.Api.Services;
using Cloud_Store.Data;
using Cloud_Store.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder = InitApiServicesBuilder(builder);

var app = builder.Build();
app = InitApiServicesApp(app);
app.Run();



static WebApplicationBuilder InitApiServicesBuilder(WebApplicationBuilder builder)
{
    builder.Services.AddAuthenticationCore();
    builder.Services.AddCascadingAuthenticationState();
    builder.Services.AddAuthorization();
    builder.Services.AddDbContext<CloudStoreContext>(options =>
        options.UseSqlite("data source=../Cloud_Store/Database/cloud_store.db"));
    // Register API authentication service
    // Add File, Hashing and User Management Services
    builder.Services.AddScoped<IFileService, FileService>();
    builder.Services.AddScoped<IHashingService, HashingService>();
    builder.Services.AddScoped<IApiAuthService, ApiAuthService>();

    // Add API-specific authorization policy
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("ApiPolicy", policy =>
            policy.AddAuthenticationSchemes("Basic").RequireAuthenticatedUser());
    });
     
    // Define CORS policy
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigins", policy =>
        {
            policy.SetIsOriginAllowed(origin => true)
                .AllowAnyHeader()
                .WithMethods("GET", "POST", "PUT", "DELETE")
                .AllowCredentials();
        });
    });
    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    return builder;
}

static WebApplication InitApiServicesApp(WebApplication app)
{
    app.UseCors("AllowSpecificOrigins");
    app.UseAuthentication();
    app.UseBasicAuth();
   
    app.UseHttpsRedirection();
    app = Api.CreateApis(app);

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
    return app;
}

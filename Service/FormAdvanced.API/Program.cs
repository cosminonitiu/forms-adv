using FormAdvanced.Infrastructure.Services.AzureAD;
using FormAdvanced.API.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using FormAdvanced.Domain.Interfaces;
using Microsoft.Azure.Cosmos;
using YamlDotNet.Serialization;
using FormAdvanced.Infrastructure.Services.AzureCosmosDB;
using FormAdvanced.API.Middleware;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AzureADOptions>(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddScoped<IAzureADService, AzureADService>();

builder.Services.AddOptions<AzureCosmosDBConfiguration>().Bind(builder.Configuration.GetSection(nameof(AzureCosmosDBConfiguration)));
builder.Services.AddSingleton<CosmosClient>((serviceProvider) =>
{
    IOptions<AzureCosmosDBConfiguration> configurationOptions = serviceProvider.GetRequiredService<IOptions<AzureCosmosDBConfiguration>>();
    AzureCosmosDBConfiguration configuration = configurationOptions.Value;

    CosmosClient client = new(
        connectionString: "AccountEndpoint=" + configuration.Endpoint + ";AccountKey=" + configuration.AccountKey + ";"
    );
    return client;
});

builder.Services.AddMemoryCache();
builder.Services.AddTransient<FormRequestService>();
builder.Services.AddTransient<IFormRequestService>(serviceProvider =>
{
    var formRequestService = serviceProvider.GetRequiredService<FormRequestService>();
    var memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
    return new CachedFormRequestService(formRequestService, memoryCache);
});

builder.Services.AddTransient<ISubmittedRequestService, SubmittedRequestService>();
builder.Services.AddTransient<INotificationsService, NotificationsService>();

// End of the Microsoft Identity platform block    
builder.AddControllers();

//builder.AddEventBus();
builder.AddHttpContextAccessor();
builder.AddJWTAuth();
builder.Services.AddAuthorization();
builder.AddMiddleware();
builder.AddSwaggerConfiguration();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin  
    .AllowCredentials()); // allow credentials

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

/*
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    endpoints.MapCustomHealthChecks();
});
*/

app.Run();

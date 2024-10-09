using Identity.API.Extensions;
using Identity.API.Models.Options;
using Newtonsoft.Json;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseSerilog((context, services, config) => config.ReadFrom.Configuration(context.Configuration, "Logging:Serilog")
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddIdentityApiServices(builder.Configuration);

WebApplication app = builder.Build();
IdentityOptions identityOptions = builder.Configuration.GetSection(nameof(IdentityOptions))
                .Get<IdentityOptions>() ?? throw new NullReferenceException(nameof(IdentityOptions));
var logger = app.Services.GetService<ILogger<Program>>();
logger.LogInformation($"Issuers: {JsonConvert.SerializeObject(identityOptions.Issuer)}");
logger.LogInformation($"Audience: {JsonConvert.SerializeObject(identityOptions.Audience)}");

app.Configure();
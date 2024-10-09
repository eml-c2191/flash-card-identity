using Identity.API.Services.Abstractions;
using Identity.API.Services;
using Identity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Net.Mime;
using Identity.API.Models.Options;
using Identity.Abstract.Extensions;
using Identity.API.Models.Validations;
using Newtonsoft.Json;

namespace Identity.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIDOptions<ApiSwaggerOptions>();
            services.AddIDOptions<IdentityOptions>();


            ApiSwaggerOptions apiSwaggerOptions = configuration.GetSection(nameof(ApiSwaggerOptions)).Get<ApiSwaggerOptions>()
              ?? throw new NullReferenceException(nameof(apiSwaggerOptions));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(apiSwaggerOptions.Version, new OpenApiInfo
                {
                    Version = apiSwaggerOptions.Version,
                    Title = apiSwaggerOptions.Title,
                    Description = apiSwaggerOptions.Description,
                    TermsOfService = apiSwaggerOptions.TermsOfService,
                    Contact = apiSwaggerOptions.Contact,
                    License = apiSwaggerOptions.License
                });
            });
            services
                .AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        ValidationFailedResult result = new(context.ModelState);

                        result.ContentTypes.Add(MediaTypeNames.Application.Json);
                        result.StatusCode = StatusCodes.Status400BadRequest;

                        return result;
                    };
                });

            #region Smsglobal sms provider
            SmsGlobalOptions smsGlobalOptions = configuration.GetSection(nameof(SmsGlobalOptions)).Get<SmsGlobalOptions>()
?? throw new NullReferenceException(nameof(smsGlobalOptions));

            string smsGlobalKey = File.ReadAllText("App_Data\\key\\SmsGlobal-Key.json");
            SmsGlobalOptions? smsGlobalKeyObject = JsonConvert.DeserializeObject<SmsGlobalOptions>(smsGlobalKey);
            smsGlobalOptions.ApiSecretKey = smsGlobalKeyObject?.ApiSecretKey ?? throw new NullReferenceException(nameof(smsGlobalOptions));
            smsGlobalOptions.ApiKey = smsGlobalKeyObject?.ApiKey ?? throw new NullReferenceException(nameof(smsGlobalOptions));

            services.AddIDOptions<SmsGlobalOptions>(smsGlobalOptions);

            #endregion

            #region Twilio sms provider

            #endregion

            string? connectionString = configuration.GetConnectionString("Identity");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new NullReferenceException(nameof(connectionString));
            }

            services
                .AddDbContext<IdentityDbContext>(options => options.UseSqlServer(connectionString, options => options.EnableRetryOnFailure()));


            return services
                .AddMemoryCache()
                .AddTransient<IIdentityCacheService, IdentityCacheService>()
                .AddTransient<IIdentityService, SelfIdentityService>()
                .AddTransient<IRefreshTokenService, RefreshTokenService>();
        }
        public static void Configure(this WebApplication app)
        {
            app.ConfigureExceptionHandler(app.Logger);

            ApiSwaggerOptions? apiSwaggerOptions = app.Configuration.GetSection(nameof(ApiSwaggerOptions)).Get<ApiSwaggerOptions>();
            if (apiSwaggerOptions is null)
            {
                throw new NullReferenceException(nameof(apiSwaggerOptions));
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(apiSwaggerOptions.Endpoint, apiSwaggerOptions.Version);
                options.RoutePrefix = string.Empty;
                options.DisplayRequestDuration();
            });
            app.MapControllers();
            app.Run();
        }
    }
}

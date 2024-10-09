using Microsoft.Extensions.DependencyInjection;

namespace Identity.Abstract.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddIDOptions<T>(this IServiceCollection services) where T : class
        {
            services.AddOptions<T>()
                .BindConfiguration(typeof(T).Name)
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }
        public static void AddIDOptions<T>
        (
            this IServiceCollection services,
            T data
        ) where T : class
        {
            services.AddSingleton<T>(data);
        }
    }
}

namespace Sample.Tris.WebApi.Configuration
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Sample.Tris.Lib.Grid;
    using Sample.Tris.Lib.Services;
    using Sample.Tris.WebApi.Providers;

    /// <summary>
    ///
    /// </summary>
    public static class TrisApiServiceCollectionExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddTrisApiConfiguration(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            TrisApiGridSettings trisApiGridSettings = new TrisApiGridSettings();

            serviceCollection
                .AddOptions()
                .Configure<TrisApiGridSettings>(configuration.GetSection(TrisApiGridSettings.SECTION_NAME));

            serviceCollection
                .AddScoped<IGridConstraintsFactory, ConfiguredGridConstraintsFactory>()
                .AddScoped<IGridAddressScheme, AlphaNumeralGridAddressScheme>()
                .AddScoped<ITriangleGridQueryService, TriangleGridQueryService>();

            serviceCollection
                .AddSingleton(provider => provider.GetRequiredService<IOptions<TrisApiGridSettings>>().Value);

            return serviceCollection;
        }
    }
}

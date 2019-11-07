namespace Sample.Tris.WebApi.Configuration
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    ///
    /// </summary>
    public static class TrisApiConfigurationHostBuilderExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        public static IHostBuilder ConfiigureTrisApi(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddEnvironmentVariables();
            });

            return hostBuilder;
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Validot;
using Wheaterbit.Client.Abstractions;
using Wheaterbit.Client.Factories;
using Wheaterbit.Client.Options;
using Wheaterbit.Client.Validation;

namespace Wheaterbit.Client.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddWeatherbit(this IServiceCollection serviceCollection, IConfigurationSection weatherbitConfiguration)
        {
            serviceCollection.Configure<WeatherbitOptions>(weatherbitConfiguration);

            return serviceCollection.AddSingleton<IWeatherbitHttpClient, WeatherbitHttpClient>()
                .AddSingleton(typeof(IValidator<WeatherbitOptions>), Validator.Factory.Create(new WeatherbitOptionsSpecificationHolder()))
                .AddSingleton<IJsonSerializerSettingsFactory, JsonSerializerSettingsFactory>();
        }
    }
}

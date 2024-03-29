﻿using Validot;

namespace Weather.API.Domain.Extensions
{
    public static class ValidotDependencyInjectionExtensions
    {
        public static IServiceCollection AddValidotSingleton<TValidator, THolder, TType>(this IServiceCollection serviceCollection)
            where TValidator : IValidator<TType>
            where THolder : ISpecificationHolder<TType>, new()
        {
            return serviceCollection.AddSingleton(typeof(TValidator), Validator.Factory.Create(new THolder()));
        }
    }
}

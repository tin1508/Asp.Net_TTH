using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using StationeryStore.Mvc.Mapper;

namespace StationeryStore.Mvc.Configuration;

public static class MapperServicesConfig
{
    public static IServiceCollection AddAppMapper(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(
            mc => mc.AddProfile(new StationeryMapper()),
            NullLoggerFactory.Instance);

        services.AddSingleton(mapperConfig.CreateMapper());
        return services;
    }
}
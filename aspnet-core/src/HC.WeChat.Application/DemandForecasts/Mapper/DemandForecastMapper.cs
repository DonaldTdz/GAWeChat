
using AutoMapper;
using HC.WeChat.DemandForecasts;
using HC.WeChat.DemandForecasts.Dtos;

namespace HC.WeChat.DemandForecasts.Mapper
{

	/// <summary>
    /// 配置DemandForecast的AutoMapper
    /// </summary>
	internal static class DemandForecastMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <DemandForecast,DemandForecastListDto>();
            configuration.CreateMap <DemandForecastListDto,DemandForecast>();

            configuration.CreateMap <DemandForecastEditDto,DemandForecast>();
            configuration.CreateMap <DemandForecast,DemandForecastEditDto>();

        }
	}
}


using AutoMapper;
using HC.WeChat.ForecastRecords;
using HC.WeChat.ForecastRecords.Dtos;

namespace HC.WeChat.ForecastRecords.Mapper
{

	/// <summary>
    /// 配置ForecastRecord的AutoMapper
    /// </summary>
	internal static class ForecastRecordMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <ForecastRecord,ForecastRecordListDto>();
            configuration.CreateMap <ForecastRecordListDto,ForecastRecord>();

            configuration.CreateMap <ForecastRecordEditDto,ForecastRecord>();
            configuration.CreateMap <ForecastRecord,ForecastRecordEditDto>();

        }
	}
}

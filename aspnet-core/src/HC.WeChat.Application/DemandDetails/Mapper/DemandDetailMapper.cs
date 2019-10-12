
using AutoMapper;
using HC.WeChat.DemandDetails;
using HC.WeChat.DemandDetails.Dtos;

namespace HC.WeChat.DemandDetails.Mapper
{

	/// <summary>
    /// 配置DemandDetail的AutoMapper
    /// </summary>
	internal static class DemandDetailMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <DemandDetail,DemandDetailListDto>();
            configuration.CreateMap <DemandDetailListDto,DemandDetail>();

            configuration.CreateMap <DemandDetailEditDto,DemandDetail>();
            configuration.CreateMap <DemandDetail,DemandDetailEditDto>();

        }
	}
}

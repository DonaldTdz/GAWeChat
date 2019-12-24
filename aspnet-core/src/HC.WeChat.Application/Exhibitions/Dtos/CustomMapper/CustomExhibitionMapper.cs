using AutoMapper;
using HC.WeChat.Exhibitions;

namespace HC.WeChat.Exhibitions.Dtos
{

	/// <summary>
	/// 配置Exhibition的AutoMapper
	/// </summary>
	internal static class CustomerExhibitionMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Exhibition, ExhibitionListDto>();
            configuration.CreateMap <ExhibitionEditDto, Exhibition>();
        }
    }
}
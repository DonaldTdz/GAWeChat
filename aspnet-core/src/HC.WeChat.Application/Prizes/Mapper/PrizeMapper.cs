
using AutoMapper;
using HC.WeChat.Prizes;
using HC.WeChat.Prizes.Dtos;

namespace HC.WeChat.Prizes.Mapper
{

	/// <summary>
    /// 配置Prize的AutoMapper
    /// </summary>
	internal static class PrizeMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Prize,PrizeListDto>();
            configuration.CreateMap <PrizeListDto,Prize>();

            configuration.CreateMap <PrizeEditDto,Prize>();
            configuration.CreateMap <Prize,PrizeEditDto>();

        }
	}
}

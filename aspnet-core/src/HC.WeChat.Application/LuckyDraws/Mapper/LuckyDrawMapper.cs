
using AutoMapper;
using HC.WeChat.LuckyDraws;
using HC.WeChat.LuckyDraws.Dtos;

namespace HC.WeChat.LuckyDraws.Mapper
{

	/// <summary>
    /// 配置LuckyDraw的AutoMapper
    /// </summary>
	internal static class LuckyDrawMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <LuckyDraw,LuckyDrawListDto>();
            configuration.CreateMap <LuckyDrawListDto,LuckyDraw>();

            configuration.CreateMap <LuckyDrawEditDto,LuckyDraw>();
            configuration.CreateMap <LuckyDraw,LuckyDrawEditDto>();

        }
	}
}


using AutoMapper;
using HC.WeChat.LuckySigns;
using HC.WeChat.LuckySigns.Dtos;

namespace HC.WeChat.LuckySigns.Mapper
{

	/// <summary>
    /// 配置LuckySign的AutoMapper
    /// </summary>
	internal static class LuckySignMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <LuckySign,LuckySignListDto>();
            configuration.CreateMap <LuckySignListDto,LuckySign>();

            configuration.CreateMap <LuckySignEditDto,LuckySign>();
            configuration.CreateMap <LuckySign,LuckySignEditDto>();

        }
	}
}

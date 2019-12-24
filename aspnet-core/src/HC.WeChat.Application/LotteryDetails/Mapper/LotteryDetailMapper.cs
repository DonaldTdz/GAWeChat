
using AutoMapper;
using HC.WeChat.LotteryDetails;
using HC.WeChat.LotteryDetails.Dtos;

namespace HC.WeChat.LotteryDetails.Mapper
{

	/// <summary>
    /// 配置LotteryDetail的AutoMapper
    /// </summary>
	internal static class LotteryDetailMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <LotteryDetail,LotteryDetailListDto>();
            configuration.CreateMap <LotteryDetailListDto,LotteryDetail>();

            configuration.CreateMap <LotteryDetailEditDto,LotteryDetail>();
            configuration.CreateMap <LotteryDetail,LotteryDetailEditDto>();

        }
	}
}

using AutoMapper;
using HC.WeChat.VoteLogs;

namespace HC.WeChat.VoteLogs.Dtos
{

	/// <summary>
	/// 配置VoteLog的AutoMapper
	/// </summary>
	internal static class CustomerVoteLogMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <VoteLog, VoteLogListDto>();
            configuration.CreateMap <VoteLogEditDto, VoteLog>();
        }
    }
}
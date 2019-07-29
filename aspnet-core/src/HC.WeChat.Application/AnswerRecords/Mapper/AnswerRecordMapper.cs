
using AutoMapper;
using HC.WeChat.AnswerRecords;
using HC.WeChat.AnswerRecords.Dtos;

namespace HC.WeChat.AnswerRecords.Mapper
{

	/// <summary>
    /// 配置AnswerRecord的AutoMapper
    /// </summary>
	internal static class AnswerRecordMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <AnswerRecord,AnswerRecordListDto>();
            configuration.CreateMap <AnswerRecordListDto,AnswerRecord>();

            configuration.CreateMap <AnswerRecordEditDto,AnswerRecord>();
            configuration.CreateMap <AnswerRecord,AnswerRecordEditDto>();

        }
	}
}

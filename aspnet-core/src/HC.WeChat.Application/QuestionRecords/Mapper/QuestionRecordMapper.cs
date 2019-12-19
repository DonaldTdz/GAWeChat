
using AutoMapper;
using HC.WeChat.QuestionRecords;
using HC.WeChat.QuestionRecords.Dtos;

namespace HC.WeChat.QuestionRecords.Mapper
{

	/// <summary>
    /// 配置QuestionRecord的AutoMapper
    /// </summary>
	internal static class QuestionRecordMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <QuestionRecord,QuestionRecordListDto>();
            configuration.CreateMap <QuestionRecordListDto,QuestionRecord>();

            configuration.CreateMap <QuestionRecordEditDto,QuestionRecord>();
            configuration.CreateMap <QuestionRecord,QuestionRecordEditDto>();

        }
	}
}


using AutoMapper;
using HC.WeChat.QuestionOptions;
using HC.WeChat.QuestionOptions.Dtos;

namespace HC.WeChat.QuestionOptions.Mapper
{

	/// <summary>
    /// 配置QuestionOption的AutoMapper
    /// </summary>
	internal static class QuestionOptionMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <QuestionOption,QuestionOptionListDto>();
            configuration.CreateMap <QuestionOptionListDto,QuestionOption>();

            configuration.CreateMap <QuestionOptionEditDto,QuestionOption>();
            configuration.CreateMap <QuestionOption,QuestionOptionEditDto>();

        }
	}
}

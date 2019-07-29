
using AutoMapper;
using HC.WeChat.Questionnaires;
using HC.WeChat.Questionnaires.Dtos;

namespace HC.WeChat.Questionnaires.Mapper
{

	/// <summary>
    /// 配置Questionnaire的AutoMapper
    /// </summary>
	internal static class QuestionnaireMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Questionnaire,QuestionnaireListDto>();
            configuration.CreateMap <QuestionnaireListDto,Questionnaire>();

            configuration.CreateMap <QuestionnaireEditDto,Questionnaire>();
            configuration.CreateMap <Questionnaire,QuestionnaireEditDto>();

        }
	}
}

using AutoMapper;
using HC.WeChat.QrCodeLogs;

namespace HC.WeChat.QrCodeLogs.Dtos
{

	/// <summary>
	/// 配置QrCodeLog的AutoMapper
	/// </summary>
	internal static class CustomerQrCodeLogMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <QrCodeLog, QrCodeLogListDto>();
            configuration.CreateMap <QrCodeLogEditDto, QrCodeLog>();
        }
    }
}
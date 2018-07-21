using System;
using Abp.Application.Services.Dto;
using HC.WeChat.IntegralDetails.Dtos.LTMAutoMapper;
using HC.WeChat.IntegralDetails;
using HC.WeChat.WechatEnums;

namespace HC.WeChat.IntegralDetails.Dtos
{
    public class IntegralDetailListDto : EntityDto<Guid>
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        public string OpenId { get; set; }
        public int? InitialIntegral { get; set; }
        public int? Integral { get; set; }
        public int? FinalIntegral { get; set; }
        public string Desc { get; set; }
        public string RefId { get; set; }
        public DateTime CreationTime { get; set; }
        public int? TenantId { get; set; }
        public IntegralTypeEnum? Type { get; set; }

        public string WXName { get; set; }
        public string TypeName
        {
            get
            {
                if (Type.HasValue)
                {
                    return Type.ToString();
                }
                return string.Empty;
            }
        }
        public string Phone { get; set; }
        public string MemberBarCode { get; set; }
        public string UserTypeName { get; set; }
        public string Code { get; set; }
    }
}
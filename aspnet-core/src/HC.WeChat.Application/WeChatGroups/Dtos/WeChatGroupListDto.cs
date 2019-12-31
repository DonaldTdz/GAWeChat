using System;
using Abp.Application.Services.Dto;
using HC.WeChat.WeChatGroups.Dtos.LTMAutoMapper;
using HC.WeChat.WeChatGroups;
using HC.WeChat.WechatEnums;

namespace HC.WeChat.WeChatGroups.Dtos
{
    public class WeChatGroupListDto : AuditedEntityDto
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        public UserTypeEnum TypeCode { get; set; }
        public string TypeName { get; set; }
        public int TagId { get; set; }
        public string TagName { get; set; }
        public int? TenantId { get; set; }
    }
    public class CheckResult
    {
        public int? TagId { get; set; }
        public bool IsExist { get; set; }
        public bool IsValid { get; set; }
    }

    /// <summary>
    /// 创建标签ByString (抽奖活动用)
    /// </summary>
    public class WeChatGroupByNameDto : AuditedEntityDto
    {
        public int TypeCode { get; set; }
        public string TypeName { get; set; }
        public int TagId { get; set; }
        public string TagName { get; set; }
        public int? TenantId { get; set; }
    }
}
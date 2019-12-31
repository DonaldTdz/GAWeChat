using System.ComponentModel.DataAnnotations;
using HC.WeChat.WeChatGroups.Dtos.LTMAutoMapper;
using HC.WeChat.WeChatGroups;
using HC.WeChat.WechatEnums;
using Abp.Domain.Entities.Auditing;

namespace HC.WeChat.WeChatGroups.Dtos
{
    public class WeChatGroupEditDto : AuditedEntity<int?>
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        //public int? Id { get; set; }
        /// <summary>
        /// 用户类型code
        /// </summary>
        [Required]
        public UserTypeEnum TypeCode { get; set; }


        /// <summary>
        ///  用户类型名
        /// </summary>
        [StringLength(50)]
        public string TypeName { get; set; }


        /// <summary>
        /// 标签id
        /// </summary>
        [Required]
        public int TagId { get; set; }


        /// <summary>
        /// 标签名
        /// </summary>
        [Required]
        [StringLength(50)]
        public string TagName { get; set; }
        public int? TenantId { get; set; }
    }
}
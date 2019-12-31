using System.ComponentModel.DataAnnotations;
using HC.WeChat.WeChatUsers.Dtos.LTMAutoMapper;
using HC.WeChat.WeChatUsers;
using System;
using HC.WeChat.WechatEnums;
using Abp.AutoMapper;

namespace HC.WeChat.WeChatUsers.Dtos
{
    public class WeChatUserEditDto
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        public Guid? Id { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string NickName { get; set; }


        /// <summary>
        /// 微信OpenId
        /// </summary>
        [Required]
        [StringLength(50)]
        public string OpenId { get; set; }


        /// <summary>
        /// 用户类型(枚举 零售用户、内部员工、消费者)
        /// </summary>
        [Required]
        public UserTypeEnum UserType { get; set; }


        /// <summary>
        /// 零售用户 或 营销人员Id 外键
        /// </summary>
        public Guid? UserId { get; set; }


        /// <summary>
        /// 零售用户 或 营销人员 名称 快照
        /// </summary>
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }


        /// <summary>
        /// 绑定状态(枚举 已绑定、未绑定)
        /// </summary>
        [Required]
        public BindStatusEnum BindStatus { get; set; }
        public DateTime? BindTime { get; set; }
        /// <summary>
        /// 关注时间
        /// </summary>
        public DateTime? AttentionTime { get; set; }

        /// <summary>
        /// 取关时间
        /// </summary>
        public DateTime? UnfollowTime { get; set; }
        public int? TenantId { get; set; }
        public DateTime? UnBindTime { get; set; }

        [StringLength(500)]
        public string HeadImgUrl { get; set; }

        /// <summary>
        /// 用户绑定电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 会员卡条形码
        /// </summary>
        public string MemberBarCode { get; set; }

        /// <summary>
        /// 用户总积分
        /// </summary>
        public int IntegralTotal { get; set; }

        /// <summary>
        /// 是否是店主 针对零售客户
        /// </summary>
        public bool? IsShopkeeper { get; set; }

        /// <summary>
        /// 审核状态(枚举：审核通过、未审核) 非店主零售客户需审核
        /// </summary>
        public UserAuditStatus? Status { get; set; }

        /// <summary>
        /// 微信票据（二维码）
        /// </summary>
        [StringLength(200)]
        public string Ticket { get; set; }

        /// <summary>
        /// 关注来源信息
        /// </summary>
        public int? SourceType { get; set; }

        /// <summary>
        /// 关注来源Id
        /// </summary>
        [StringLength(100)]
        public string SourceId { get; set; }
    }

    [AutoMapTo(typeof(WeChatUser))]
    public class UserBindDto
    {
        public string host { get; set; }
        public string OpenId { get; set; }

        public UserTypeEnum UserType { get; set; }

        //public string UserName { get; set; }

        public string LicenseKey { get; set; }

        public string Code { get; set; }

        public int? TenantId { get; set; }

        //public string NickName { get; set; }

        //public string HeadImgUrl { get; set; }

        public string VerificationCode { get; set; }

        //public string Phone { get; set; }
    }

    /// <summary>
    /// 会员绑定
    /// </summary>
    public class MemberBindDto
    {
        public string OpenId { get; set; }

        public int? TenantId { get; set; }

        public string Phone { get; set; }

        public string Host { get; set; }
    }

    public class LotteryUserBindDto
    {
        public string OpenId { get; set; }

        public string Code { get; set; }
    }
}
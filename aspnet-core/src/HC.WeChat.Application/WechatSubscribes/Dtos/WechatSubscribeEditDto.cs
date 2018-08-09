using System.ComponentModel.DataAnnotations;
using HC.WeChat.WechatSubscribes.Dtos.LTMAutoMapper;
using HC.WeChat.WechatSubscribes;
using System;
using HC.WeChat.WechatEnums;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using HC.WeChat.WechatMessages;
using Abp.AutoMapper;

namespace HC.WeChat.WechatSubscribes.Dtos
{
    [AutoMapTo(typeof(WechatSubscribe))]
    public class WechatSubscribeEditDto : AuditedEntityDto<Guid?>
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        ///public Guid? Id { get; set; }

        /// <summary>
        /// 消息类型（枚举 文字消息、图文消息）
        /// </summary>
        [Required]
        public MsgTypeEnum MsgType { get; set; }


        /// <summary>
        /// 回复内容
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 图片链接
        /// </summary>
        public string PicLink { get; set; }
        /// <summary>
        /// 租户ID
        /// </summary>
        //[Required]
        //public int TenantId { get; set; }
        public string MediaId { get; set; }
    }
}
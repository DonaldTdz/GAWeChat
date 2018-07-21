using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using HC.WeChat.WechatEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.WeChat.Exhibitions
{
    /// <summary>
    /// 陈列活动
    /// </summary>
    [Table("Exhibitions")]
    public class Exhibition : Entity<Guid>
    {

        /// <summary>
        /// 开始时间
        /// </summary>
        public virtual DateTime? BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public virtual DateTime? EndTime { get; set; }

        /// <summary>
        /// 内容（活动规则）（html）
        /// </summary>
        public virtual string Content { get; set; }

        /// <summary>
        /// 说明（投票规则）（html）
        /// </summary>
        public virtual string Desc { get; set; }

        /// <summary>
        /// 展示数据量
        /// </summary>
        [Required]
        public virtual int TopTotal { get; set; }

        /// <summary>
        /// 每日投票上限 默认5
        /// </summary>
        public virtual int? Frequency { get; set; }
    }
}

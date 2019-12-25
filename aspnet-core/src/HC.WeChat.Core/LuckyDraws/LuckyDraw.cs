using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using HC.WeChat.WechatEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.WeChat.LuckyDraws
{
    /// <summary>
    /// 抽奖活动
    /// </summary>
    [Table("LuckyDraws")]
    public class LuckyDraw : Entity<Guid>, IHasCreationTime
    {

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(200)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Required]
        public virtual DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Required]
        public virtual DateTime EndTime { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        [Required]
        public virtual bool IsPublish { get; set; }

        ///// <summary>
        ///// CreationTime
        ///// </summary>
        [Required]
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public virtual DateTime? PublishTime { get; set; }
    }
}

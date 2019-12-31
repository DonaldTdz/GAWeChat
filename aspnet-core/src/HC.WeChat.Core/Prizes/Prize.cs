using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using HC.WeChat.WechatEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.WeChat.Prizes
{
    /// <summary>
    /// 奖品
    /// </summary>
    [Table("Prizes")]
    public class Prize : Entity<Guid>,IHasCreationTime
    {

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }


        /// <summary>
        /// 抽奖活动Id
        /// </summary>
        [Required]
        public virtual Guid LuckyDrawId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public virtual PrizeType Type { get; set; }

        /// <summary>
        /// 投放总量 
        /// </summary>
        public virtual int Num { get; set; }

        /// <summary>
        /// 中奖人Id
        /// </summary>
        public virtual Guid? WinUserId { get; set; }

        public virtual DateTime CreationTime { get; set; }
    }
}

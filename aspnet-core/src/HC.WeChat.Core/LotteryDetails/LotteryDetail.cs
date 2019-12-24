using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.WeChat.LotteryDetails
{
    [Table("LotteryDetails")]
    public class LotteryDetail : Entity<Guid>, IHasCreationTime
    {

        /// <summary>
        /// 抽奖活动Id
        /// </summary>
        [Required]
        public virtual Guid LuckyDrawId { get; set; }

        /// <summary>
        /// OpenId
        /// </summary>
        [Required]
        public virtual Guid UserId { get; set; }

        /// <summary>
        /// 是否允许中奖
        /// </summary>
        [Required]
        public virtual bool IsCanWin { get; set; }

        /// <summary>
        /// 是否中奖
        /// </summary>
        [Required]
        public virtual bool IsWin { get; set; }

        /// <summary>
        /// 奖品Id
        /// </summary>
        public virtual Guid? PrizeId { get; set; }

        /// <summary>
        /// 奖品名
        /// </summary>
        [StringLength(50)]
        public virtual string PrizeName { get; set; }

        /// <summary>
        /// 是否抽奖
        /// </summary>
        public virtual bool IsLottery { get; set; }

        /// <summary>
        /// 抽奖时间
        /// </summary>
        public virtual DateTime? LotteryTime { get; set; }

        public virtual DateTime CreationTime { get; set; }
    }
}
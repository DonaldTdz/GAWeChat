using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.WeChat.DemandDetails
{
    /// <summary>
    /// 预测详情表
    /// </summary>
    [Table("DemandDetails")]
    public class DemandDetail : Entity<Guid>, IHasCreationTime
    {

        /// <summary>
        /// 外键
        /// </summary>
        [Required]
        public virtual Guid DemandForecastId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// 价类(一、二、三、四、五类)
        /// </summary>
        public virtual int? Type { get; set; }

        /// <summary>
        /// 批发价
        /// </summary>
        public virtual decimal? WholesalePrice { get; set; }

        /// <summary>
        /// 建议零售价
        /// </summary>
        public virtual decimal? SuggestPrice { get; set; }

        /// <summary>
        /// 是否异型烟
        /// </summary>
        public virtual bool? IsAlien { get; set; }

        /// <summary>
        /// 上月订购量
        /// </summary>
        public virtual int? LastMonthNum { get; set; }

        /// <summary>
        /// 同比
        /// </summary>
        public virtual decimal? YearOnYear { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public virtual DateTime CreationTime { get; set; }
    }
}

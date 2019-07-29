using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.WeChat.ForecastRecords
{
    /// <summary>
    /// 零售户预测表
    /// </summary>
    [Table("ForecastRecords")]
    public class ForecastRecord : Entity<Guid>, IHasCreationTime
    {

        /// <summary>
        /// 外键
        /// </summary>
        [Required]
        public virtual Guid DemandDetailId { get; set; }

        /// <summary>
        /// 预测量
        /// </summary>
        [Required]
        public virtual int PredictiveValue { get; set; }

        /// <summary>
        /// 微信OpenId
        /// </summary>
        [Required]
        [StringLength(50)]
        public virtual string OpenId { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public virtual DateTime CreationTime { get; set; }
    }
}

using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.WeChat.DemandForecasts
{
    /// <summary>
    /// 需求预测表
    /// </summary>
    [Table("DemandForecasts")]
    public class DemandForecast : Entity<Guid>,ICreationAudited
    {

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        public virtual string Title { get; set; }

        /// <summary>
        /// 预测月份
        /// </summary>
        [Required]
        public virtual DateTime? Month { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// CreatorUserId
        /// </summary>
        public virtual long? CreatorUserId { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        [Required]
        public virtual bool IsPublish { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public virtual DateTime? PublishTime { get; set; }
    }
}

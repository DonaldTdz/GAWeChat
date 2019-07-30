using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using HC.WeChat.WechatEnums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.WeChat.Questionnaires
{
    /// <summary>
    /// 问卷调查表
    /// </summary>
    [Table("Questionnaires")]
    public class Questionnaire : FullAuditedEntity<Guid>
    {

        /// <summary>
        /// 类型（客户服务评价、卷烟供应评价、市场管理评价、综合评价）
        /// </summary>
        [Required]
        public virtual QuestionType Type { get; set; }

        /// <summary>
        /// 是否多选
        /// </summary>
        [Required]
        public virtual bool IsMultiple { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        public virtual string No { get; set; }

        /// <summary>
        /// 问题描述
        /// </summary>
        [Required]
        [StringLength(500)]
        public virtual string Question { get; set; }
    }
}

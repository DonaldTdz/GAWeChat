using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.WeChat.QuestionOptions
{
    /// <summary>
    /// 选项表
    /// </summary>
    [Table("QuestionOptions")]
    public class QuestionOption : Entity<Guid>
    {
        /// <summary>
        /// 问题外键
        /// </summary>
        [Required]
        public virtual Guid QuestionnaireId { get; set; }

        /// <summary>
        /// 选项值
        /// </summary>
        [Required]
        [StringLength(50)]
        public virtual string Value { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        [StringLength(50)]
        public virtual string Desc { get; set; }
    }
}

using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.WeChat.AnswerRecords
{
    /// <summary>
    /// 问卷填写表
    /// </summary>
    [Table("AnswerRecords")]
    public class AnswerRecord : Entity<Guid>, IHasCreationTime
    {

        /// <summary>
        /// 问卷调查外键
        /// </summary>
        [Required]
        public virtual Guid QuestionnaireId { get; set; }
        /// <summary>
        /// 选项Id
        /// </summary>
        [Required]
        public virtual Guid OptionId { get; set; }
        /// <summary>
        /// 选项值（多选逗号分隔）
        /// </summary>
        public virtual string Values { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(500)]
        public virtual string Remark { get; set; }

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

using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.WeChat.QuestionRecords
{
    /// <summary>
    /// 问题表
    /// </summary>
    [Table("QuestionRecords")]
    public class QuestionRecord : Entity<Guid>, ICreationAudited
    {

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        public virtual string Title { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        [Required]
        public virtual string Year { get; set; }

        /// <summary>
        /// 季度
        /// </summary>
        public virtual int Quarter { get; set; }

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

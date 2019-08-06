
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using HC.WeChat.AnswerRecords;

namespace  HC.WeChat.AnswerRecords.Dtos
{
    public class AnswerRecordEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 问卷调查Id
        /// </summary>
        [Required]
        public Guid QuestionRecordId { get; set; }
        /// <summary>
        /// 选项Id
        /// </summary>
        [Required]
        public Guid OptionId { get; set; }

        /// <summary>
        /// QuestionnaireId
        /// </summary>
        [Required(ErrorMessage="QuestionnaireId不能为空")]
		public Guid QuestionnaireId { get; set; }



		/// <summary>
		/// Values
		/// </summary>
		public string Values { get; set; }



		/// <summary>
		/// Remark
		/// </summary>
		public string Remark { get; set; }



		/// <summary>
		/// OpenId
		/// </summary>
		[Required(ErrorMessage="OpenId不能为空")]
		public string OpenId { get; set; }



		/// <summary>
		/// CreationTime
		/// </summary>
		public DateTime CreationTime { get; set; }
    }

    /// <summary>
    /// 保存调查问卷dto
    /// </summary>
    public class CreateWXAnswerDto
    {
        public string OpenId { get; set; }
        public Guid QuestionRecordId { get; set; }
        public List<QuestionAnswerList> List { get; set; }
    }

    /// <summary>
    /// 答案列表
    /// </summary>
    public class QuestionAnswerList
    {
        public Guid QuestionnaireId { get; set; }
        public string Values { get; set; }
        public string Remark { get; set; }

    }
    public class QuestionnaireFillRecordsDto
    {
        public int Quarter { get; set; }

        public string Status { get; set; }

        public string Desc { get; set; }
    }
}
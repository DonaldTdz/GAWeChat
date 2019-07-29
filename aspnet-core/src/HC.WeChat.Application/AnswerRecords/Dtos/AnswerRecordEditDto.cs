
using System;
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
}
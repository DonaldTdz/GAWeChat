

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.AnswerRecords;

namespace HC.WeChat.AnswerRecords.Dtos
{
    public class AnswerRecordListDto : EntityDto<Guid>,IHasCreationTime 
    {

        
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


using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.QuestionOptions;

namespace HC.WeChat.QuestionOptions.Dtos
{
    public class QuestionOptionListDto : EntityDto<Guid> 
    {

        
		/// <summary>
		/// QuestionnaireId
		/// </summary>
		[Required(ErrorMessage="QuestionnaireId不能为空")]
		public Guid QuestionnaireId { get; set; }



		/// <summary>
		/// Value
		/// </summary>
		[Required(ErrorMessage="Value不能为空")]
		public string Value { get; set; }



		/// <summary>
		/// Desc
		/// </summary>
		[Required(ErrorMessage="Desc不能为空")]
		public string Desc { get; set; }




    }
}


using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.Questionnaires;
using HC.WeChat.WechatEnums;
using Abp.AutoMapper;

namespace HC.WeChat.Questionnaires.Dtos
{
    [AutoMapFrom(typeof(Questionnaire))]
    public class QuestionnaireListDto : FullAuditedEntityDto<Guid> 
    {

        
		/// <summary>
		/// Type
		/// </summary>
		[Required(ErrorMessage="Type不能为空")]
		public QuestionType Type { get; set; }



		/// <summary>
		/// IsMultiple
		/// </summary>
		[Required(ErrorMessage="IsMultiple不能为空")]
		public bool IsMultiple { get; set; }



		/// <summary>
		/// No
		/// </summary>
		[Required(ErrorMessage="No不能为空")]
		public string No { get; set; }



		/// <summary>
		/// Question
		/// </summary>
		[Required(ErrorMessage="Question不能为空")]
		public string Question { get; set; }


        /// <summary>
        /// 分类名称
        /// </summary>
        public virtual string TypeName { get; set; }

       
        //public QuestionnaireListDto()
        //{
        //    TypeName = Type.ToString();
        //}
    }
}
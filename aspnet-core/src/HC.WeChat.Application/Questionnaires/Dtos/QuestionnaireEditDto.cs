
using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using HC.WeChat.Questionnaires;
using HC.WeChat.WechatEnums;

namespace  HC.WeChat.Questionnaires.Dtos
{
    [AutoMapTo(typeof(Questionnaire))]
    public class QuestionnaireEditDto: FullAuditedEntity<Guid?>
    {

        /// <summary>
        /// Id
        /// </summary>
        //public Guid? Id { get; set; }



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




    }
}
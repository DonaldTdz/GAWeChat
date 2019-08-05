

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
        /// 选项Id
        /// </summary>
        [Required]
        public Guid OptionId { get; set; }
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

    public class AnswerRecordWXListDto
    {
        /// <summary>
        /// 问卷调查外键
        /// </summary>
        [Required]
        public virtual Guid QuestionnaireId { get; set; }

        /// <summary>
        /// 选项值（多选逗号分隔）
        /// </summary>
        public virtual string Values { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(500)]
        public virtual string Remark { get; set; }

        ///// <summary>
        ///// 微信OpenId
        ///// </summary>
        //[Required]
        //[StringLength(50)]
        //public virtual string OpenId { get; set; }
    }
}


using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.Questionnaires;
using HC.WeChat.WechatEnums;
using Abp.AutoMapper;
using System.Collections.Generic;
using HC.WeChat.QuestionOptions;

namespace HC.WeChat.Questionnaires.Dtos
{
    [AutoMapFrom(typeof(Questionnaire))]
    public class QuestionnaireListDto : FullAuditedEntityDto<Guid>
    {


        /// <summary>
        /// Type
        /// </summary>
        [Required(ErrorMessage = "Type不能为空")]
        public QuestionType Type { get; set; }



        /// <summary>
        /// IsMultiple
        /// </summary>
        [Required(ErrorMessage = "IsMultiple不能为空")]
        public bool IsMultiple { get; set; }



        /// <summary>
        /// No
        /// </summary>
        [Required(ErrorMessage = "No不能为空")]
        public string No { get; set; }



        /// <summary>
        /// Question
        /// </summary>
        [Required(ErrorMessage = "Question不能为空")]
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

    public class WXQuestionnaireListDto
    {
        public WXQuestionnaireListDto()
        {
            QuestionOptions = new List<QuestionOption>();
        }
        public Guid Id { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        public QuestionType Type { get; set; }

        /// <summary>
        /// IsMultiple
        /// </summary>
        public bool IsMultiple { get; set; }

        /// <summary>
        /// No
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// Question
        /// </summary>
        public string Question { get; set; }

        public string Remark { get; set; }

        public float Index { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string TypeName
        {
            get
            {
                return Type.ToString();
            }
        }

        public string Value { get; set; }
        public Guid RecordId { get; set; }
        /// <summary>
        /// 答案配置
        /// </summary>
        public List<QuestionOption> QuestionOptions { get; set; }
    }

    /// <summary>
    /// 问卷调查记录详情
    /// </summary>
    public class AnswerQuestionWXListDto
    {
        public AnswerQuestionWXListDto()
        {
            list = new List<RecordOptionWxDto>();
        }
        public Guid Id { get; set; }
        public string No { get; set; }
        public string Question { get; set; }
        public Guid RecordId { get; set; }
        public string Value { get; set; }
        public List<RecordOptionWxDto> list { get; set; }
    }

    public class RecordOptionWxDto
    {
        public string Value { get; set; }
        public string Desc { get; set; }
    }
}
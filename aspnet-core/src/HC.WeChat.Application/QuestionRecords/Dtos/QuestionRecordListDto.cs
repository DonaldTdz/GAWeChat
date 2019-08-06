

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.QuestionRecords;
using Abp.AutoMapper;
using System.Collections.Generic;

namespace HC.WeChat.QuestionRecords.Dtos
{
    [AutoMapFrom(typeof(QuestionRecord))]
    public class QuestionRecordListDto : EntityDto<Guid>,ICreationAudited 
    {

        
		/// <summary>
		/// Title
		/// </summary>
		[Required(ErrorMessage="Title不能为空")]
		public string Title { get; set; }



		/// <summary>
		/// Year
		/// </summary>
		[Required(ErrorMessage="Year不能为空")]
		public string Year { get; set; }



		/// <summary>
		/// Quarter
		/// </summary>
		public int Quarter { get; set; }



		/// <summary>
		/// CreationTime
		/// </summary>
		public DateTime CreationTime { get; set; }



		/// <summary>
		/// CreatorUserId
		/// </summary>
		public long? CreatorUserId { get; set; }



		/// <summary>
		/// IsPublish
		/// </summary>
		[Required(ErrorMessage="IsPublish不能为空")]
		public bool IsPublish { get; set; }



		/// <summary>
		/// PublishTime
		/// </summary>
		public DateTime? PublishTime { get; set; }


        /// <summary>
        /// 年份季度拼接字符串
        /// </summary>
        public string QuarterString { get; set; }


        /// <summary>
        /// 填写时间
        /// </summary>
        public DateTime? WriteTime { get; set; }

        /// <summary>
        /// OpenId
        /// </summary>
        public string OpenId { get; set; }
    }


    /// <summary>
    /// 零售户问卷填写记录
    /// </summary>
    public class RetailQuestionRecordListDto : EntityDto<Guid>
    {
        /// <summary>
        /// 零售户名称,展示用
        /// </summary>
        public string UserName { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// 年份季度拼接字符串
        /// </summary>
        public string QuarterString { get; set; }


        public DateTime WriteTime { get; set; }

        public DateTime CreationTime { get; set; }
    }


    /// <summary>
    /// 零售户调查问卷填写详情
    /// </summary>
    public class RetailQuestionRecordDetailDto
    {
        public string No { get; set; }

        public Guid QuestionnaireId { get; set; }

        public string Question { get; set; }

        public string[] Desc { get; set; }

        public string Values { get; set; }

        public string Remark { get; set; }
    }
}
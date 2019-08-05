
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using HC.WeChat.QuestionRecords;

namespace  HC.WeChat.QuestionRecords.Dtos
{
    public class QuestionRecordEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }         


        
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




    }
}
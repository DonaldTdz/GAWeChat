

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.DemandForecasts;

namespace HC.WeChat.DemandForecasts.Dtos
{
    public class DemandForecastListDto : EntityDto<Guid>,ICreationAudited 
    {

        
		/// <summary>
		/// Title
		/// </summary>
		public string Title { get; set; }



		/// <summary>
		/// Month
		/// </summary>
		public DateTime? Month { get; set; }



		/// <summary>
		/// CreationTime
		/// </summary>
		public DateTime CreationTime { get; set; }



		/// <summary>
		/// CreatorUserId
		/// </summary>
		public long? CreatorUserId { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        [Required]
        public bool IsPublish { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? PublishTime { get; set; }
    }

    /// <summary>
    /// 微信列表DTO
    /// </summary>
    public class DemandWXListDto : EntityDto<Guid>
    {
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Month
        /// </summary>
        public DateTime? Month { get; set; }
        public string Status { get; set; }
    }
}
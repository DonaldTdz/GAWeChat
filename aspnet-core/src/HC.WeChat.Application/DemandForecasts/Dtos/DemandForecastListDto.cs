

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

    /// <summary>
    /// 零售户预测记录主表
    /// </summary>
    public class RetailDemandForecastListDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public DateTime? Month { get; set; }
        public DateTime? CompleteTime { get; set; }
    }

    /// <summary>
    /// 零售户预测列表头部信息
    /// </summary>
    public class RetailDemandHeadDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public DateTime? Month { get; set; }
        public DateTime? CompleteTime { get; set; }
    }
}


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
        /// �Ƿ񷢲�
        /// </summary>
        [Required]
        public bool IsPublish { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime? PublishTime { get; set; }
    }

    /// <summary>
    /// ΢���б�DTO
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
    /// ���ۻ�Ԥ���¼����
    /// </summary>
    public class RetailDemandForecastListDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public DateTime? Month { get; set; }
        public DateTime? CompleteTime { get; set; }
    }

    /// <summary>
    /// ���ۻ�Ԥ���б�ͷ����Ϣ
    /// </summary>
    public class RetailDemandHeadDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public DateTime? Month { get; set; }
        public DateTime? CompleteTime { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using HC.WeChat.DemandForecasts;

namespace  HC.WeChat.DemandForecasts.Dtos
{
    public class DemandForecastEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }         


        
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
}
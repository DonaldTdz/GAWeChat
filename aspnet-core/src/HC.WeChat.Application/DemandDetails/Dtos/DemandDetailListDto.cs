

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.DemandDetails;

namespace HC.WeChat.DemandDetails.Dtos
{
    public class DemandDetailListDto : EntityDto<Guid>,IHasCreationTime 
    {

        
		/// <summary>
		/// DemandForecastId
		/// </summary>
		[Required(ErrorMessage="DemandForecastId不能为空")]
		public Guid DemandForecastId { get; set; }



		/// <summary>
		/// Name
		/// </summary>
		[Required(ErrorMessage="Name不能为空")]
		public string Name { get; set; }



		/// <summary>
		/// Type
		/// </summary>
		public int? Type { get; set; }



		/// <summary>
		/// WholesalePrice
		/// </summary>
		public decimal? WholesalePrice { get; set; }



		/// <summary>
		/// SuggestPrice
		/// </summary>
		public decimal? SuggestPrice { get; set; }



		/// <summary>
		/// IsAlien
		/// </summary>
		public bool? IsAlien { get; set; }



		/// <summary>
		/// LastMonthNum
		/// </summary>
		public int? LastMonthNum { get; set; }



		/// <summary>
		/// YearOnYear
		/// </summary>
		public decimal? YearOnYear { get; set; }



		/// <summary>
		/// CreationTime
		/// </summary>
		public DateTime CreationTime { get; set; }

        /// <summary>
        /// 零售户姓名
        /// </summary>
        [Required]
        [StringLength(50)]
        public string RetailerName { get; set; }

        /// <summary>
        /// 零售户编码
        /// </summary>
        [Required]
        [StringLength(50)]
        public string RetailerCode { get; set; }
    }
}
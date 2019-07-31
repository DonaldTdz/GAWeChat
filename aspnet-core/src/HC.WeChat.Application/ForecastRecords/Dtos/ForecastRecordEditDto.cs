
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using HC.WeChat.ForecastRecords;

namespace  HC.WeChat.ForecastRecords.Dtos
{
    public class ForecastRecordEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }


        /// <summary>
        /// 预测主表Id
        /// </summary>
        [Required]
        public Guid DemandForecastId { get; set; }
        /// <summary>
        /// DemandDetailId
        /// </summary>
        [Required(ErrorMessage="DemandDetailId不能为空")]
		public Guid DemandDetailId { get; set; }



		/// <summary>
		/// PredictiveValue
		/// </summary>
		[Required(ErrorMessage="PredictiveValue不能为空")]
		public int PredictiveValue { get; set; }



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

    /// <summary>
    /// 微信保存预测记录
    /// </summary>
    public class ForecastRecordWXEditDto
    {
        public List<PredictDto> List { get; set; }
        [Required(ErrorMessage = "DemandForecastId")]
        public Guid DemandForecastId { get; set; }
        [Required(ErrorMessage = "OpenId不能为空")]
        public string OpenId { get; set; }
    }

    public class PredictDto
    {
        [Required(ErrorMessage = "DemandDetailId不能为空")]
        public Guid DemandDetailId { get; set; }
        /// <summary>
        /// PredictiveValue
        /// </summary>
        [Required(ErrorMessage = "PredictiveValue不能为空")]
        public int PredictiveValue { get; set; }
    }
}
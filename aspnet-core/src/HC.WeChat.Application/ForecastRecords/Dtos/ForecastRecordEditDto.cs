
using System;
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
}
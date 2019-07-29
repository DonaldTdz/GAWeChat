

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.ForecastRecords;

namespace HC.WeChat.ForecastRecords.Dtos
{
    public class ForecastRecordListDto : EntityDto<Guid>,IHasCreationTime 
    {

        
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
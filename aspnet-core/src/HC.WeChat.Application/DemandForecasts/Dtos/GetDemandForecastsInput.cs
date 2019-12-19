
using Abp.Runtime.Validation;
using HC.WeChat.Dto;
using HC.WeChat.DemandForecasts;
using System;

namespace HC.WeChat.DemandForecasts.Dtos
{
    public class GetDemandForecastsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        public Guid UserId { get; set; }
        public Guid DemandForecastId { get; set; }
        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }

    }
}

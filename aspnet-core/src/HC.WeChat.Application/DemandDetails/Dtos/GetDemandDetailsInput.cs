
using Abp.Runtime.Validation;
using HC.WeChat.Dto;
using System;

namespace HC.WeChat.DemandDetails.Dtos
{
    public class GetDemandDetailsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        public Guid DemandForecastId { get; set; }
        public Guid UserId { get; set; }
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

    /// <summary>
    /// 数据导入DTO
    /// </summary>
    public class ImportDto
    {
        public Guid DemandForecastId { get; set; }
    }

    /// <summary>
    /// 微信获取列表DTO
    /// </summary>
    public class GetWXDetailListDto
    {
        public string OpenId { get; set; }
        public Guid DemandForecastId { get; set; }
    }
}

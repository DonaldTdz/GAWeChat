using Abp.Runtime.Validation;
using HC.WeChat.Dto;
using HC.WeChat.Shops;
using HC.WeChat.WechatEnums;
using System;

namespace HC.WeChat.Shops.Dtos
{
    public class GetShopsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        /// <summary>
        /// 模糊搜索使用的关键字
        /// </summary>
        public string Filter { get; set; }
        public string Name { get; set; }
        public ShopAuditStatus? Status { get; set; }
        public string Tel { get; set; }
        public string SortSaleTotal { get; set; }
        public string SortReadTotal { get; set; }
        public string SortSingleTotal { get; set; }
        public string SortFansTotal { get; set; }
        public string RetailCode { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }

        public Guid ShopId { get; set; }
        public string OpenId { get; set; }

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

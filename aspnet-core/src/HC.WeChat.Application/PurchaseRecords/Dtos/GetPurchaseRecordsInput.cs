using Abp.Runtime.Validation;
using HC.WeChat.Dto;
using HC.WeChat.PurchaseRecords;
using System;

namespace HC.WeChat.PurchaseRecords.Dtos
{
    public class GetPurchaseRecordsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        /// <summary>
        /// 模糊搜索使用的关键字
        /// </summary>
        public string Filter { get; set; }
        public string Name { get; set; }
        public string OpenId { get; set; }
        public Guid? ShopId { get; set; }
        public string SortQuantityTotal { get; set; }
        public string SortPriceTotal { get; set; }
        public string SortIntegralTotal { get; set; }
        public Guid ProductId { get; set; }


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

using Abp.Runtime.Validation;
using HC.WeChat.Dto;
using HC.WeChat.ExhibitionShops;

namespace HC.WeChat.ExhibitionShops.Dtos
{ 
    public class GetExhibitionShopsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 模糊搜索使用的关键字
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// 店铺名
        /// </summary>
        public string ShopName { get; set; }
        public string CustName { get; set; }
        public string CustCode { get; set; }
        public string Phone { get; set; }
        public string SortFansTotal { get; set; }
        public string SortVotesTotal { get; set; }


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

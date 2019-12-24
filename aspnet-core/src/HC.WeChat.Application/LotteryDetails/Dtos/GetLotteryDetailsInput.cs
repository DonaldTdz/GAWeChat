
using Abp.Runtime.Validation;
using HC.WeChat.Dto;
using HC.WeChat.LotteryDetails;

namespace HC.WeChat.LotteryDetails.Dtos
{
    public class GetLotteryDetailsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {

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


using Abp.Runtime.Validation;
using HC.WeChat.Dto;
using HC.WeChat.LuckyDraws;

namespace HC.WeChat.LuckyDraws.Dtos
{
    public class GetLuckyDrawsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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

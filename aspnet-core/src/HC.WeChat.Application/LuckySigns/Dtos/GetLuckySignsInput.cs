
using Abp.Runtime.Validation;
using HC.WeChat.Dto;
using HC.WeChat.LuckySigns;

namespace HC.WeChat.LuckySigns.Dtos
{
    public class GetLuckySignsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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

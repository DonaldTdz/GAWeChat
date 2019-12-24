
using Abp.Runtime.Validation;
using HC.WeChat.Dto;
using HC.WeChat.Prizes;

namespace HC.WeChat.Prizes.Dtos
{
    public class GetPrizesInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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

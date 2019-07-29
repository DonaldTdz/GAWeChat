
using Abp.Runtime.Validation;
using HC.WeChat.Dto;

namespace HC.WeChat.DemandDetails.Dtos
{
    public class GetDemandDetailsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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


using Abp.Runtime.Validation;
using HC.WeChat.Dto;
using HC.WeChat.ForecastRecords;

namespace HC.WeChat.ForecastRecords.Dtos
{
    public class GetForecastRecordsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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

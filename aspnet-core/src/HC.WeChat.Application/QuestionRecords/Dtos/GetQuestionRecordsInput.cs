
using Abp.Runtime.Validation;
using HC.WeChat.Dto;
using HC.WeChat.QuestionRecords;

namespace HC.WeChat.QuestionRecords.Dtos
{
    public class GetQuestionRecordsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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

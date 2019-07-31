
using Abp.Runtime.Validation;
using HC.WeChat.Dto;
using HC.WeChat.QuestionOptions;

namespace HC.WeChat.QuestionOptions.Dtos
{
    public class GetQuestionOptionsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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

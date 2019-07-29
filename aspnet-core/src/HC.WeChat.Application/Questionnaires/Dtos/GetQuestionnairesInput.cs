
using Abp.Runtime.Validation;
using HC.WeChat.Dto;
using HC.WeChat.Questionnaires;

namespace HC.WeChat.Questionnaires.Dtos
{
    public class GetQuestionnairesInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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


using Abp.Runtime.Validation;
using HC.WeChat.Dto;
using HC.WeChat.QuestionRecords;
using System;

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

        public int? Quarter { get; set; }

        public Guid? RetailerId { get; set; }

        public string Title { get; set; }
    }

    public class GetQuestionRecordHeadInput
    {
        public Guid RetailerId { get; set; }

        public Guid QuestionRecordId { get; set; }
    }
}


using Abp.Runtime.Validation;
using HC.WeChat.Dto;
using HC.WeChat.AnswerRecords;
using System;

namespace HC.WeChat.AnswerRecords.Dtos
{
    public class GetAnswerRecordsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        public string OpenId { get; set; }

        public int? Quarter { get; set; }

    }

    public class GetRetailAnswerRecordsInput
    {
        
        public Guid RetailerId { get; set; }

        public Guid QuestionRecordId { get; set; }
    }
}

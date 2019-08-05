

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.QuestionRecords;

namespace HC.WeChat.QuestionRecords.Dtos
{
    public class CreateOrUpdateQuestionRecordInput
    {
        [Required]
        public QuestionRecordEditDto QuestionRecord { get; set; }

    }
}
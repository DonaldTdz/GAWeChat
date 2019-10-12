

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.AnswerRecords;

namespace HC.WeChat.AnswerRecords.Dtos
{
    public class CreateOrUpdateAnswerRecordInput
    {
        [Required]
        public AnswerRecordEditDto AnswerRecord { get; set; }

    }
}
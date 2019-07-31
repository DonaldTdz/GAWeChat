

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.QuestionOptions;

namespace HC.WeChat.QuestionOptions.Dtos
{
    public class CreateOrUpdateQuestionOptionInput
    {
        [Required]
        public QuestionOptionEditDto QuestionOption { get; set; }

    }
}


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.Questionnaires;

namespace HC.WeChat.Questionnaires.Dtos
{
    public class CreateOrUpdateQuestionnaireInput
    {
        [Required]
        public QuestionnaireEditDto Questionnaire { get; set; }

    }
}
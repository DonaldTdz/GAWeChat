

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.LuckyDraws;

namespace HC.WeChat.LuckyDraws.Dtos
{
    public class CreateOrUpdateLuckyDrawInput
    {
        [Required]
        public LuckyDrawEditDto LuckyDraw { get; set; }

    }
}
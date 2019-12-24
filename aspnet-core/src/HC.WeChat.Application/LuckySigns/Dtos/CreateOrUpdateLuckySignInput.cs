

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.LuckySigns;

namespace HC.WeChat.LuckySigns.Dtos
{
    public class CreateOrUpdateLuckySignInput
    {
        [Required]
        public LuckySignEditDto LuckySign { get; set; }

    }
}


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.Prizes;

namespace HC.WeChat.Prizes.Dtos
{
    public class CreateOrUpdatePrizeInput
    {
        [Required]
        public PrizeEditDto Prize { get; set; }

    }
}
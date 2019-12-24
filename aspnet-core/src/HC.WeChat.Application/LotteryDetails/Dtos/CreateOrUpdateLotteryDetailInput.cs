

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.LotteryDetails;

namespace HC.WeChat.LotteryDetails.Dtos
{
    public class CreateOrUpdateLotteryDetailInput
    {
        [Required]
        public LotteryDetailEditDto LotteryDetail { get; set; }

    }
}
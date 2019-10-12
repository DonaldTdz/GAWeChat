

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.DemandDetails;

namespace HC.WeChat.DemandDetails.Dtos
{
    public class CreateOrUpdateDemandDetailInput
    {
        [Required]
        public DemandDetailEditDto DemandDetail { get; set; }

    }
}
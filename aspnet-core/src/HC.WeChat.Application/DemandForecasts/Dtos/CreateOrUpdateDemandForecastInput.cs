

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.DemandForecasts;

namespace HC.WeChat.DemandForecasts.Dtos
{
    public class CreateOrUpdateDemandForecastInput
    {
        [Required]
        public DemandForecastEditDto DemandForecast { get; set; }

    }
}


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.ForecastRecords;

namespace HC.WeChat.ForecastRecords.Dtos
{
    public class CreateOrUpdateForecastRecordInput
    {
        [Required]
        public ForecastRecordEditDto ForecastRecord { get; set; }

    }
}
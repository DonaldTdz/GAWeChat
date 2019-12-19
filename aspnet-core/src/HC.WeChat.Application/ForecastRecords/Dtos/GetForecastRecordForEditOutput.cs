

using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HC.WeChat.ForecastRecords;

namespace HC.WeChat.ForecastRecords.Dtos
{
    public class GetForecastRecordForEditOutput
    {

        public ForecastRecordEditDto ForecastRecord { get; set; }

    }
}
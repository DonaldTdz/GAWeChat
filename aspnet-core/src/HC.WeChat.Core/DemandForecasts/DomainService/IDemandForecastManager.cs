

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.WeChat.DemandForecasts;


namespace HC.WeChat.DemandForecasts.DomainService
{
    public interface IDemandForecastManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitDemandForecast();



		 
      
         

    }
}

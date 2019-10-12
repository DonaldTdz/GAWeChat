

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.WeChat.ForecastRecords;


namespace HC.WeChat.ForecastRecords.DomainService
{
    public interface IForecastRecordManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitForecastRecord();



		 
      
         

    }
}

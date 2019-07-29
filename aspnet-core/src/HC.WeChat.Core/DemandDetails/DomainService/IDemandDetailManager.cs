

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.WeChat.DemandDetails;


namespace HC.WeChat.DemandDetails.DomainService
{
    public interface IDemandDetailManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitDemandDetail();



		 
      
         

    }
}

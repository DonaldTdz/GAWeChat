

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.WeChat.LotteryDetails;


namespace HC.WeChat.LotteryDetails.DomainService
{
    public interface ILotteryDetailManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitLotteryDetail();



		 
      
         

    }
}

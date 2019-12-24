

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.WeChat.Prizes;


namespace HC.WeChat.Prizes.DomainService
{
    public interface IPrizeManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitPrize();



		 
      
         

    }
}

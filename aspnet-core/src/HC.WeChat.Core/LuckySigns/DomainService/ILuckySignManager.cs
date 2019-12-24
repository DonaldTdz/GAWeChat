

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.WeChat.LuckySigns;


namespace HC.WeChat.LuckySigns.DomainService
{
    public interface ILuckySignManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitLuckySign();



		 
      
         

    }
}

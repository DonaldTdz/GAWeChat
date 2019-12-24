

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.WeChat.LuckyDraws;


namespace HC.WeChat.LuckyDraws.DomainService
{
    public interface ILuckyDrawManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitLuckyDraw();



		 
      
         

    }
}

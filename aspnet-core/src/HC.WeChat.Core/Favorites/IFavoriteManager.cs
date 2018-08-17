

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.WeChat.Favorites;


namespace HC.WeChat.Favorites
{
    public interface IFavoriteManager : IDomainService
    {

        /// <summary>
    /// 初始化方法
    ///</summary>
        void InitFavorite();



		//// custom codes
 
        //// custom codes end

    }
}

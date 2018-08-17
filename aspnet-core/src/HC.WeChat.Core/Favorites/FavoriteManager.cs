

using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using HC.WeChat;
using HC.WeChat.Favorites;


namespace HC.WeChat.Favorites
{
    /// <summary>
    /// Favorite领域层的业务管理
    ///</summary>
    public class FavoriteManager :WeChatDomainServiceBase, IFavoriteManager
    {
    private readonly IRepository<Favorite,Guid> _favoriteRepository;

        /// <summary>
            /// Favorite的构造方法
            ///</summary>
        public FavoriteManager(IRepository<Favorite, Guid>
favoriteRepository)
            {
            _favoriteRepository =  favoriteRepository;
            }


            /// <summary>
                ///     初始化
                ///</summary>
            public void InitFavorite()
            {
            throw new NotImplementedException();
            }

            //TODO:编写领域业务代码



            //// custom codes
             
            //// custom codes end

            }
            }

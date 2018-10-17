
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using System.Linq;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;

using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using System;
using HC.WeChat.Authorization;
using HC.WeChat.Favorites.Authorization;
using HC.WeChat.Favorites.Dtos;
using HC.WeChat.WeChatUsers;
using HC.WeChat.Dto;
using HC.WeChat.Shops;
using Abp.Auditing;

namespace HC.WeChat.Favorites
{
    /// <summary>
    /// Favorite应用层服务的接口实现方法  
    ///</summary>
//[AbpAuthorize(FavoriteAppPermissions.Favorite)] 
    [AbpAuthorize(AppPermissions.Pages)]

    public class FavoriteAppService : WeChatAppServiceBase, IFavoriteAppService
    {
        private readonly IRepository<Favorite, Guid> _favoriteRepository;
        private readonly IRepository<WeChatUser, Guid> _wechatuserRepository;
        private readonly IRepository<Shop, Guid> _shopRepository;
        private readonly IFavoriteManager _favoriteManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public FavoriteAppService(
        IRepository<Favorite, Guid> favoriteRepository
            , IFavoriteManager favoriteManager
            , IRepository<WeChatUser, Guid> wechatuserRepository
            , IRepository<Shop, Guid> shopRepository
            )
        {
            _favoriteRepository = favoriteRepository;
            _favoriteManager = favoriteManager;
            _wechatuserRepository = wechatuserRepository;
            _shopRepository = shopRepository;
        }


        /// <summary>
        /// 获取Favorite的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<FavoriteListDto>> GetPagedFavorites(GetFavoritesInput input)
        {

            var query = _favoriteRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件

            var favoriteCount = await query.CountAsync();

            var favorites = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var favoriteListDtos = ObjectMapper.Map<List <FavoriteListDto>>(favorites);
            var favoriteListDtos = favorites.MapTo<List<FavoriteListDto>>();

            return new PagedResultDto<FavoriteListDto>(
favoriteCount,
favoriteListDtos
                );
        }


        /// <summary>
        /// 通过指定id获取FavoriteListDto信息
        /// </summary>
        public async Task<FavoriteListDto> GetFavoriteByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _favoriteRepository.GetAsync(input.Id);

            return entity.MapTo<FavoriteListDto>();
        }

        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetFavoriteForEditOutput> GetFavoriteForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetFavoriteForEditOutput();
            FavoriteEditDto favoriteEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _favoriteRepository.GetAsync(input.Id.Value);

                favoriteEditDto = entity.MapTo<FavoriteEditDto>();

                //favoriteEditDto = ObjectMapper.Map<List <favoriteEditDto>>(entity);
            }
            else
            {
                favoriteEditDto = new FavoriteEditDto();
            }

            output.Favorite = favoriteEditDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Favorite的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateFavorite(CreateOrUpdateFavoriteInput input)
        {

            if (input.Favorite.Id.HasValue)
            {
                await UpdateFavoriteAsync(input.Favorite);
            }
            else
            {
                await CreateFavoriteAsync(input.Favorite);
            }
        }


        /// <summary>
        /// 新增Favorite
        /// </summary>
        [AbpAuthorize(FavoriteAppPermissions.Favorite_CreateFavorite)]
        protected virtual async Task<FavoriteEditDto> CreateFavoriteAsync(FavoriteEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<Favorite>(input);

            entity = await _favoriteRepository.InsertAsync(entity);
            return entity.MapTo<FavoriteEditDto>();
        }

        /// <summary>
        /// 编辑Favorite
        /// </summary>
        [AbpAuthorize(FavoriteAppPermissions.Favorite_EditFavorite)]
        protected virtual async Task UpdateFavoriteAsync(FavoriteEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _favoriteRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _favoriteRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除Favorite信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(FavoriteAppPermissions.Favorite_DeleteFavorite)]
        public async Task DeleteFavorite(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _favoriteRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 判断用户是否取消收藏
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<bool> GetUserIsCancelShopAsycn(Guid shopId, string openId)
        {
            bool isCancel = false;
            int count = await _favoriteRepository.GetAll().Where(v => v.ShopId == shopId && v.OpenId == openId).CountAsync();
            if (count != 0)
            {
                isCancel = await _favoriteRepository.GetAll().Where(v => v.ShopId == shopId && v.OpenId == openId).Select(v => v.IsCancel).FirstOrDefaultAsync();
            }
            else
            {
                isCancel = true;
            }
            return isCancel;
        }

        /// <summary>
        /// 添加或取消收藏店铺
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]

        public async Task<APIResultDto> AddOrCancelFavouriteShopAsync(FavoriteEditDto input)
        {
            string coverPhoto = await _shopRepository.GetAll().Where(v => v.Id == input.ShopId).Select(v => v.CoverPhoto).FirstOrDefaultAsync();
            var entity = await _favoriteRepository.GetAll().Where(v => v.ShopId == input.ShopId && v.OpenId == input.OpenId).FirstOrDefaultAsync();
            string userName = await _wechatuserRepository.GetAll().Where(v => v.OpenId == input.OpenId).Select(v => v.NickName).FirstOrDefaultAsync();
            if (entity != null)
            {
                if (entity.IsCancel == false)
                {
                    entity.CancelTime = DateTime.Now;
                }
                entity.NickName = userName;
                entity.IsCancel = input.IsCancel;
                entity.CoverPhoto = coverPhoto;
                await _favoriteRepository.UpdateAsync(entity);
            }
            else
            {
                Favorite favorite = new Favorite();
                favorite.OpenId = input.OpenId;
                favorite.ShopId = input.ShopId;
                favorite.ShopName = input.ShopName;
                favorite.NickName = userName;
                favorite.CoverPhoto = coverPhoto;
                favorite.IsCancel = false;
                await _favoriteRepository.InsertAsync(favorite);
            }
            return new APIResultDto() { Code = 0, Msg = "成功" };
        }

        /// <summary>
        /// 我的收藏
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<List<FavoriteListDto>> GetWXMyFavoriteShopsAsync(string openId)
        {
            var query = _favoriteRepository.GetAll().Where(p => p.OpenId == openId && p.IsCancel ==false);
            var entity = from f in query
                                select new FavoriteListDto()
                                {
                                    Id = f.Id,
                                    ShopId = f.ShopId,
                                    CoverPhoto = f.CoverPhoto,
                                    ShopName = f.ShopName
                                };
            return await entity.OrderByDescending(v => v.CreationTime).ToListAsync();
        }
    }
}



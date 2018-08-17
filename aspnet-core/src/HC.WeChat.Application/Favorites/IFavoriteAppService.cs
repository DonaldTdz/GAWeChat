

using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HC.WeChat.Favorites.Dtos;
using HC.WeChat.Favorites;
using System;
using HC.WeChat.Dto;

namespace HC.WeChat.Favorites
{
    /// <summary>
    /// Favorite应用层服务的接口方法
    ///</summary>
    public interface IFavoriteAppService : IApplicationService
    {
        /// <summary>
    /// 获取Favorite的分页列表信息
    ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<FavoriteListDto>> GetPagedFavorites(GetFavoritesInput input);

		/// <summary>
		/// 通过指定id获取FavoriteListDto信息
		/// </summary>
		Task<FavoriteListDto> GetFavoriteByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 导出Favorite为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetFavoritesToExcel();

        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetFavoriteForEditOutput> GetFavoriteForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetFavoriteForEditOutput


        /// <summary>
        /// 添加或者修改Favorite的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateFavorite(CreateOrUpdateFavoriteInput input);


        /// <summary>
        /// 删除Favorite信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteFavorite(EntityDto<Guid> input);


        Task<bool> GetUserIsCancelShopAsycn(Guid shopId, string openId);
        Task<APIResultDto> AddOrCancelFavouriteShopAsync(FavoriteEditDto input);
        Task<List<FavoriteListDto>> GetWXMyFavoriteShopsAsync(string openId);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HC.WeChat.Shops.Dtos;
using HC.WeChat.Shops;
using System;
using Senparc.Weixin.MP.AdvancedAPIs.QrCode;
using HC.WeChat.Dto;
using System.IO;

namespace HC.WeChat.Shops
{
    /// <summary>
    /// Shop应用层服务的接口方法
    /// </summary>
    public interface IShopAppService : IApplicationService
    {
        /// <summary>
        /// 获取Shop的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ShopListDto>> GetPagedShops(GetShopsInput input);

        /// <summary>
        /// 通过指定id获取ShopListDto信息
        /// </summary>
        Task<ShopListDto> GetShopByIdAsync(EntityDto<Guid> input);

        /// <summary>
        /// 导出Shop为excel表
        /// </summary>
        /// <returns></returns>
        //  Task<FileDto> GetShopsToExcel();
        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetShopForEditOutput> GetShopForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetShopForEditOutput
        /// <summary>
        /// 添加或者修改Shop的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateShop(CreateOrUpdateShopInput input);

        /// <summary>
        /// 删除Shop信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteShop(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除Shop
        /// </summary>
        Task BatchDeleteShopsAsync(List<Guid> input);

        /// <summary>
        /// 添加或者修改Shop的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateShopDto(ShopEditDto input);

        /// <summary>
        /// 获取Shop的分页列表信息(连接零售客户表)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ShopListDto>> GetPagedShopsByRetailer(GetShopsInput input);

        /// <summary>
        /// 获取单个店铺信息（连接零售客户表）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ShopListDto> GetShopByIdRetailerAsync(Guid? input);
        /// 微信端保存和添加店铺
        /// </summary>
        Task WechatCreateOrUpdateShop(CreateOrUpdateShopInput input);

        /// <summary>
        /// 通过微信用户获取店铺信息
        /// </summary>
        Task<ShopListDto> GetShopByOpenId(int? tenantId, string openId);

        /// <summary>
        /// 店铺审核
        /// </summary>
        Task CheckShop(CheckShopDto input);

        //Task<ShopListDto> GetViewShopByIdAsync(Guid id, int? tenantId);

        Task<List<NearbyShopDto>> GetNearbyShopByLocationAsync(double latitude, double longitude, int? tenantId, string openId);

        Task<List<NearbyShopDto>> GetShopListByGoodsIdAsync(int? tenantId, Guid goodsId, double latitude, double longitude);

        Task<HomeInfo> GetHomeInfo();

        Task<APIResultDto> GetPendingShopList();

        /// <summary>
        /// 生成店码
        /// </summary>
        Task<CreateQRResult> GenerateShopCodeAsync(Guid shopId);

        /// <summary>
        /// 批量生成二维码
        /// </summary>
        /// <returns></returns>
        Task BatchCreateQRCodeAsync();
        APIResultDto PromotionCodeZip(GetShopsInput input);

        /// <summary>
        /// 二维码添加logo
        /// </summary>
        /// <param name="filePath">二维码路径</param>
        /// <param name="imgName">图片名称</param>
        string QrcodeImgLogo(string filePath, string imgName);

        /// <summary>
        /// 店铺入驻数统计（按分公司统计）
        /// </summary>
        Task<ShopStatisticLiDto> GetShopStatisticsByCompany();

        Task<APIResultDto> BatchCompressionPictures(string fromPath, string toPaht, int type, int val);

        /// <summary>
        /// 获取店铺二维码url
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        string GetQrCodeUrl(Guid shopId, string host);

        /// <summary>
        /// 获取店铺推广码
        /// </summary>
        /// <param name="shopId">店铺id</param>
        /// <returns></returns>
        Task<string> GetShopQrCodeURL(Guid shopId);

        Task<APIResultDto> GenerateShopQrCode(List<ShopUrlQrCodeDto> input);
        Task<bool> GetIsCurShopKeeper(GetShopsInput input);
        Task<bool> GetShopWithRangeAsync(double latitude, double longitude, Guid shopId);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HC.WeChat.Products.Dtos;
using HC.WeChat.Products;
using System;

namespace HC.WeChat.Products
{
    /// <summary>
    /// Product应用层服务的接口方法
    /// </summary>
    public interface IProductAppService : IApplicationService
    {
        /// <summary>
        /// 获取Product的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ProductListDto>> GetPagedProducts(GetProductsInput input);

        /// <summary>
        /// 通过指定id获取ProductListDto信息
        /// </summary>
        Task<ProductListDto> GetProductByIdAsync(EntityDto<Guid> input);

        /// <summary>
        /// 导出Product为excel表
        /// </summary>
        /// <returns></returns>
        //  Task<FileDto> GetProductsToExcel();
        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetProductForEditOutput> GetProductForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetProductForEditOutput
        /// <summary>
        /// 添加或者修改Product的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateProduct(CreateOrUpdateProductInput input);

        /// <summary>
        /// 删除Product信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteProduct(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除Product
        /// </summary>
        Task BatchDeleteProductsAsync(List<Guid> input);

        /// <summary>
        /// 添加或者修改Product的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ProductListDto> CreateOrUpdateProductDto(ProductEditDto input);

        /// <summary>
        /// 获取特色商品
        /// </summary>
        Task<RareProductDto> GetRareProduct(int? tenantId);

        /// <summary>
        /// 根据关键字搜索特色商品
        /// </summary>
        /// <returns></returns>
        Task<List<RareProductSearchDto>> GetRareProductByKeyAsync(int? tenantId, string key);

        /// <summary>
        /// 通过指定id获取ProductListDto信息
        /// </summary>
        Task<ProductListDto> GetProductByIdDtoAsync(EntityDto<Guid> input);

        /// <summary>
        /// 根据包码 和 条码 获取商品信息
        /// </summary>
        Task<ShopProductDto> GetShopProductByCode(string code, int? tenantId);

        /// <summary>
        /// 检查包码条码是否重复（0：不重复，1：包码重复，2：条码重复，3,：包码、条码重复）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<int> GetCheckCode(CheckInput input);

        /// <summary>
        /// 获取台账信息
        /// </summary>
        /// <param name="code">专卖证号</param>
        /// <returns></returns>
        Task<RetailAllInfoDe> GetCustAndAccountInfoAsync(int? tenantId, Guid userId, int span);

        /// <summary>
        /// 获取档级信息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<RetailInfoDto> GetRetailBasicInfoAsync(int? tenantId, Guid userId);

        /// <summary>
        /// 手动更新档级
        /// </summary>
        /// <returns></returns>
        void UpdateLevel();

        Task<ProductListDto> GetWXProductByIdAsync(Guid id);
    }
}

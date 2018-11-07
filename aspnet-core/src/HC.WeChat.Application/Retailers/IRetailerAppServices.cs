using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HC.WeChat.Retailers.Dtos;
using HC.WeChat.Retailers;
using System;
using HC.WeChat.Dto;

namespace HC.WeChat.Retailers
{
    /// <summary>
    /// Retailer应用层服务的接口方法
    /// </summary>
    public interface IRetailerAppService : IApplicationService
    {
        /// <summary>
        /// 获取Retailer的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<RetailerListDto>> GetPagedRetailers(GetRetailersInput input);

        /// <summary>
        /// 通过指定id获取RetailerListDto信息
        /// </summary>
        Task<RetailerListDto> GetRetailerByIdAsync(EntityDto<Guid> input);

        /// <summary>
        /// 导出Retailer为excel表
        /// </summary>
        /// <returns></returns>
        //  Task<FileDto> GetRetailersToExcel();
        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetRetailerForEditOutput> GetRetailerForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetRetailerForEditOutput
        /// <summary>
        /// 添加或者修改Retailer的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateRetailer(CreateOrUpdateRetailerInput input);

        /// <summary>
        /// 删除Retailer信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteRetailer(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除Retailer
        /// </summary>
        Task BatchDeleteRetailersAsync(List<Guid> input);

        /// <summary>
        /// 添加或者修改Retailer的公共方法
        /// </summary>
        /// <param name="input">零售客户实体</param>
        /// <returns></returns>
        Task CreateOrUpdateRetailerDto(RetailerEditDto input);

        /// <summary>
        /// 通过id查询单个零售户信息
        /// </summary>
        /// <param name="input">零售户id</param>
        /// <returns></returns>
        Task<RetailerListDto> GetRetailerByIdDtoAsync(EntityDto<Guid> input);

        /// <summary>
        /// 检查零售户编码是否可用
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        bool CheckName(string code, Guid? id);

        /// <summary>
        /// 分页获取零售户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<RetailerListDto>> GetAllRetailByPageAsync(GetRetailersWeChatInput input);

        /// <summary>
        /// 通过指定id获取RetailerListDto信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RetailerListDto> GetRetailerByIdDtoForWeChatAsync(Guid input);

        /// <summary>
        /// 通过指定userId获取RetailerListDto信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<RetailerListDto> GetRetailerByIdDtoByLKeyForWeChatAsync(Guid userId);
        Task<List<RetailerListDto>> GetRetailerLevelListAsync(GetRetailersInput input);
        Task<APIResultDto> ExportRetailerLevelExcel(GetRetailersInput input);
        Task<APIResultDto> ExportRetailerAllExcel(GetRetailersInput input);
        Task<PagedResultDto<ShopReportData>> GetDataStatisticsAsync(GetShopReportDataInput input);
        Task<PagedResultDto<ShopReportData>> GetShopReportDataDetailAsync(GetShopReportDataInput input);
        Task<APIResultDto> ExportShopReportDataExcel(GetShopReportDataInput input);
        Task<APIResultDto> ExportShopReportDataByOrganizationExcel(GetShopReportDataInput input);
    }
}

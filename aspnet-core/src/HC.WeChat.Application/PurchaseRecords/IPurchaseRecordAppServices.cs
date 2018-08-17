using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HC.WeChat.PurchaseRecords.Dtos;
using HC.WeChat.PurchaseRecords;
using System;
using HC.WeChat.Dto;

namespace HC.WeChat.PurchaseRecords
{
    /// <summary>
    /// PurchaseRecord应用层服务的接口方法
    /// </summary>
    public interface IPurchaseRecordAppService : IApplicationService
    {
        /// <summary>
        /// 获取PurchaseRecord的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<PurchaseRecordListDto>> GetPagedPurchaseRecords(GetPurchaseRecordsInput input);

        /// <summary>
        /// 通过指定id获取PurchaseRecordListDto信息
        /// </summary>
        Task<PurchaseRecordListDto> GetPurchaseRecordByIdAsync(EntityDto<Guid> input);

        /// <summary>
        /// 导出PurchaseRecord为excel表
        /// </summary>
        /// <returns></returns>
        //  Task<FileDto> GetPurchaseRecordsToExcel();
        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetPurchaseRecordForEditOutput> GetPurchaseRecordForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetPurchaseRecordForEditOutput
        /// <summary>
        /// 添加或者修改PurchaseRecord的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdatePurchaseRecord(CreateOrUpdatePurchaseRecordInput input);

        /// <summary>
        /// 删除PurchaseRecord信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeletePurchaseRecord(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除PurchaseRecord
        /// </summary>
        Task BatchDeletePurchaseRecordsAsync(List<Guid> input);

        /// <summary>
        /// 兑换积分
        /// </summary>
        Task<APIResultDto> ExchangeIntegralAsync(ExchangeIntegralDto input);

        //Task<List<PurchaseRecordListDto>> GetWXPagedPurchaseRecordAsync(int? tenantId, string openId, int pageIndex, int pageSize);
        Task<List<PurchaseRecordListDto>> GetWXPagedPurchaseRecordAsync(int? tenantId, string openId);
        /// <summary>
        /// 获取指定用户购买记录的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<PurchaseRecordListDto>> GetPagedPurchaseRecordsByIdAsync(GetPurchaseRecordsInput input);
        Task<PagedResultDto<PurchaseRecordListDto>> GetPagedPurchaseRecordByShopIdAsync(GetPurchaseRecordsInput input);
        Task<APIResultDto> ExportShopExcel(GetPurchaseRecordsInput input);
    }
}

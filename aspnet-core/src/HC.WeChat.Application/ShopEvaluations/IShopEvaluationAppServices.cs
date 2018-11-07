using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HC.WeChat.ShopEvaluations.Dtos;
using HC.WeChat.ShopEvaluations;
using System;
using HC.WeChat.PurchaseRecords.Dtos;
using HC.WeChat.Dto;

namespace HC.WeChat.ShopEvaluations
{
    /// <summary>
    /// ShopEvaluation应用层服务的接口方法
    /// </summary>
    public interface IShopEvaluationAppService : IApplicationService
    {
        /// <summary>
        /// 获取ShopEvaluation的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ShopEvaluationListDto>> GetPagedShopEvaluations(GetShopEvaluationsInput input);

        /// <summary>
        /// 通过指定id获取ShopEvaluationListDto信息
        /// </summary>
        Task<ShopEvaluationListDto> GetShopEvaluationByIdAsync(EntityDto<Guid> input);
        /// <summary>
        /// 导出ShopEvaluation为excel表
        /// </summary>
        /// <returns></returns>
        //  Task<FileDto> GetShopEvaluationsToExcel();
        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetShopEvaluationForEditOutput> GetShopEvaluationForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetShopEvaluationForEditOutput
        /// <summary>
        /// 添加或者修改ShopEvaluation的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateShopEvaluation(CreateOrUpdateShopEvaluationInput input);

        /// <summary>
        /// 删除ShopEvaluation信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteShopEvaluation(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除ShopEvaluation
        /// </summary>
        Task BatchDeleteShopEvaluationsAsync(List<Guid> input);

        /// <summary>
        /// 获取ShopEvaluation的分页列表信息联合购买记录表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ShopEvaluationListDto>> GetPagedShopEvaluationsByPurchaseRecord(GetShopEvaluationsInput input);
        //Task<List<PurchaseRecordListDto>> GetWXNotEvaluationByIdAsync(int? tenantId, string openId);
        Task<List<PurchaseRecordListDto>> GetWXNotEvaluationByIdAsync(int? tenantId, string openId, int pageIndex, int pageSize);
        Task<int> GetWXCountNotEvaluationByIdAsync(int? tenantId, string openId);
        Task<PurchaseRecordListDto> GetWXProductsDetailsByIdAsync(int? tenantId, string openId, Guid? productId, Guid? id);
        Task<APIResultDto> SubmitShopEvaluationAsync(ShopEvaluation input);
        Task<ShopEvaluationListDto> GetWXEvaluationByIdAsync(int? tenantId, Guid? shopEvaluationId);
    }
}

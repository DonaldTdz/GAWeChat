using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HC.WeChat.IntegralDetails.Dtos;
using HC.WeChat.IntegralDetails;
using System;
using HC.WeChat.WeChatUsers.Dtos;

namespace HC.WeChat.IntegralDetails
{
    /// <summary>
    /// IntegralDetail应用层服务的接口方法
    /// </summary>
    public interface IIntegralDetailAppService : IApplicationService
    {
        /// <summary>
        /// 获取IntegralDetail的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //Task<PagedResultDto<IntegralDetailListDto>> GetPagedIntegralDetailsAsync(GetIntegralDetailsInput input);

        /// <summary>
        /// 通过指定id获取IntegralDetailListDto信息
        /// </summary>
        Task<IntegralDetailListDto> GetIntegralDetailByIdAsync(EntityDto<Guid> input);

        /// <summary>
        /// 导出IntegralDetail为excel表
        /// </summary>
        /// <returns></returns>
        //  Task<FileDto> GetIntegralDetailsToExcel();
        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetIntegralDetailForEditOutput> GetIntegralDetailForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetIntegralDetailForEditOutput
        /// <summary>
        /// 添加或者修改IntegralDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateIntegralDetail(CreateOrUpdateIntegralDetailInput input);

        /// <summary>
        /// 删除IntegralDetail信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteIntegralDetail(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除IntegralDetail
        /// </summary>
        Task BatchDeleteIntegralDetailsAsync(List<Guid> input);

        /// <summary>
        /// 获取积分详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<IntegralDetailListDto>> GetPagedIntegralDetailsByIdAsync(GetIntegralDetailsInput input);
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        Task<WeChatUserListDto> GetUserInfoAsync(string openId,string host);
        Task<PagedResultDto<WeChatUserListDto>> GetPagedIntegralDetailsAsync(GetWeChatUsersInput input);
        Task<List<IntegralDetailListDto>> GetWXPagedIntegralDetailAsync(int? tenantId, string openId, int pageIndex, int pageSize);
    }
}

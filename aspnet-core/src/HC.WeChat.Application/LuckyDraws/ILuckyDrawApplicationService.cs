
using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Abp.UI;
using Abp.AutoMapper;
using Abp.Authorization;
using Abp.Linq.Extensions;
using Abp.Domain.Repositories;
using Abp.Application.Services;
using Abp.Application.Services.Dto;


using HC.WeChat.LuckyDraws.Dtos;
using HC.WeChat.LuckyDraws;
using HC.WeChat.Dto;

namespace HC.WeChat.LuckyDraws
{
    /// <summary>
    /// LuckyDraw应用层服务的接口方法
    ///</summary>
    public interface ILuckyDrawAppService : IApplicationService
    {
        /// <summary>
		/// 获取LuckyDraw的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<LuckyDrawListDto>> GetPaged(GetLuckyDrawsInput input);


		/// <summary>
		/// 通过指定id获取LuckyDrawListDto信息
		/// </summary>
		Task<LuckyDrawListDto> GetById(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetLuckyDrawForEditOutput> GetForEdit(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改LuckyDraw的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateLuckyDrawInput input);


        /// <summary>
        /// 删除LuckyDraw信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除LuckyDraw
        /// </summary>
        Task BatchDelete(List<Guid> input);


        /// <summary>
        /// 导出LuckyDraw为excel表
        /// </summary>
        /// <returns></returns>
        //Task<FileDto> GetToExcel();
        Task<APIResultDto> CreateWXLuckyDrawAsync(WeiXinCreateInput input);


        Task<List<WXLuckyDrawOutput>> GetWXLuckyDrawListAsync();


        Task<APIResultDto> ChangeWXLuckyDrawPubStatusAsync(WeiXinUpdatePubInput weiXinUpdatePubInput);

        /// <summary>
        /// 通过ID获取活动详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<WXLuckyDrawDetailIDOutput> GetLuckyDrawDetailByIdAsync(Guid Id, string openId);

        /// <summary>
        /// 内部员工获取已发布活动列表
        /// </summary>
        /// <returns></returns>
        Task<List<WXLuckyDrawOutput>> GetWXLuckyDrawListPublishedAsync();

        /// <summary>
        /// 获取参与活动抽奖的部门下员工抽奖情况
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="DeptName"></param>
        /// <returns></returns>
        Task<List<LotteryJoinDeptDetailOutput>> GetLotteryJoinDeptDetailAsync(Guid Id, string DeptName);

        /// <summary>
        /// 根据活动ID获取当前参与人的数量
        /// </summary>
        /// <param name="luckyId"></param>
        /// <returns></returns>
        Task<GetLuckyDrawPersonCountDto> GetLuckyDrawPersonCountAsync(Guid luckyId);

        /// <summary>
        /// 根据活动ID获取部门详情
        /// </summary>
        /// <param name="luckyId"></param>
        /// <returns></returns>
        Task<List<GetLuckyDeptmentLotteryPersonDto>> GetLuckyDeptmentLotteryPersonAsync(Guid luckyId);
        Task<string> GetAuthorizationUrl(string host);
        /// <summary>
        /// 更新一个抽奖活动
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<APIResultDto> RenewWXLuckyDrawAsync(WeiXinCreateInput input);

    }
}

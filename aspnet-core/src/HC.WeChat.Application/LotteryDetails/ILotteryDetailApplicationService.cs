
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


using HC.WeChat.LotteryDetails.Dtos;
using HC.WeChat.LotteryDetails;
using HC.WeChat.Dto;

namespace HC.WeChat.LotteryDetails
{
    /// <summary>
    /// LotteryDetail应用层服务的接口方法
    ///</summary>
    public interface ILotteryDetailAppService : IApplicationService
    {
        /// <summary>
		/// 获取LotteryDetail的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<LotteryDetailListDto>> GetPaged(GetLotteryDetailsInput input);


		/// <summary>
		/// 通过指定id获取LotteryDetailListDto信息
		/// </summary>
		Task<LotteryDetailListDto> GetById(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetLotteryDetailForEditOutput> GetForEdit(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改LotteryDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateLotteryDetailInput input);


        /// <summary>
        /// 删除LotteryDetail信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除LotteryDetail
        /// </summary>
        Task BatchDelete(List<Guid> input);


        Task<APIResultDto> GetCurLotteryDetailAsync(string openId, Guid luckyId);
    }
}

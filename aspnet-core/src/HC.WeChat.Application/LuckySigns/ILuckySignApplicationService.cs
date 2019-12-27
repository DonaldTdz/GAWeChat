
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


using HC.WeChat.LuckySigns.Dtos;
using HC.WeChat.LuckySigns;

namespace HC.WeChat.LuckySigns
{
    /// <summary>
    /// LuckySign应用层服务的接口方法
    ///</summary>
    public interface ILuckySignAppService : IApplicationService
    {
        /// <summary>
		/// 获取LuckySign的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<LuckySignListDto>> GetPaged(GetLuckySignsInput input);


		/// <summary>
		/// 通过指定id获取LuckySignListDto信息
		/// </summary>
		Task<LuckySignListDto> GetById(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetLuckySignForEditOutput> GetForEdit(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改LuckySign的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateLuckySignInput input);


        /// <summary>
        /// 删除LuckySign信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除LuckySign
        /// </summary>
        Task BatchDelete(List<Guid> input);

        /// <summary>
        /// 获取内部员工抽奖相关信息和状态
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        Task<GetLuckySignInfoDto> GetLuckySignInfoAsync(string openId);

    }
}

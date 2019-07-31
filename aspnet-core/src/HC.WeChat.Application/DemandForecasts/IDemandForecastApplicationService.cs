
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


using HC.WeChat.DemandForecasts.Dtos;
using HC.WeChat.DemandForecasts;
using HC.WeChat.Dto;

namespace HC.WeChat.DemandForecasts
{
    /// <summary>
    /// DemandForecast应用层服务的接口方法
    ///</summary>
    public interface IDemandForecastAppService : IApplicationService
    {
        /// <summary>
		/// 获取DemandForecast的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DemandForecastListDto>> GetPaged(GetDemandForecastsInput input);


		/// <summary>
		/// 通过指定id获取DemandForecastListDto信息
		/// </summary>
		Task<DemandForecastListDto> GetById(Guid id);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetDemandForecastForEditOutput> GetForEdit(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改DemandForecast的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<APIResultDto> CreateOrUpdate(DemandForecastEditDto input);

        /// <summary>
        /// 删除DemandForecast信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除DemandForecast
        /// </summary>
        Task BatchDelete(List<Guid> input);

        Task<List<DemandWXListDto>> GetWXDemandListAsync();
    }
}

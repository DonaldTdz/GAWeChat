
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


using HC.WeChat.ForecastRecords.Dtos;
using HC.WeChat.ForecastRecords;
using HC.WeChat.Dto;

namespace HC.WeChat.ForecastRecords
{
    /// <summary>
    /// ForecastRecord应用层服务的接口方法
    ///</summary>
    public interface IForecastRecordAppService : IApplicationService
    {
        /// <summary>
		/// 获取ForecastRecord的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ForecastRecordListDto>> GetPaged(GetForecastRecordsInput input);


		/// <summary>
		/// 通过指定id获取ForecastRecordListDto信息
		/// </summary>
		Task<ForecastRecordListDto> GetById(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetForecastRecordForEditOutput> GetForEdit(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改ForecastRecord的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateForecastRecordInput input);


        /// <summary>
        /// 删除ForecastRecord信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除ForecastRecord
        /// </summary>
        Task BatchDelete(List<Guid> input);
        Task<APIResultDto> CreateForecastRecordAsync(ForecastRecordWXEditDto input);
    }
}


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


using HC.WeChat.DemandDetails.Dtos;
using HC.WeChat.DemandDetails;
using HC.WeChat.Dto;

namespace HC.WeChat.DemandDetails
{
    /// <summary>
    /// DemandDetail应用层服务的接口方法
    ///</summary>
    public interface IDemandDetailAppService : IApplicationService
    {
        /// <summary>
		/// 获取DemandDetail的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DemandDetailListDto>> GetPaged(GetDemandDetailsInput input);


		/// <summary>
		/// 通过指定id获取DemandDetailListDto信息
		/// </summary>
		Task<DemandDetailListDto> GetById(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetDemandDetailForEditOutput> GetForEdit(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改DemandDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateDemandDetailInput input);


        /// <summary>
        /// 删除DemandDetail信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除DemandDetail
        /// </summary>
        Task BatchDelete(List<Guid> input);

        Task<APIResultDto> ImportDemandDetailExcelAsync(ImportDto input);
        Task<List<DetailWXListDto>> GetWXDetailListByIdAsync(GetWXDetailListDto input);
        Task<List<DetailWXListDto>> GetWXDetailRecordByIdAsync(GetWXDetailListDto input);
        Task<PagedResultDto<RetailDemandDetailListDto>> GetDetailRecordByIdAsync(GetDemandDetailsInput input);
    }
}


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


using HC.WeChat.AnswerRecords.Dtos;
using HC.WeChat.AnswerRecords;

namespace HC.WeChat.AnswerRecords
{
    /// <summary>
    /// AnswerRecord应用层服务的接口方法
    ///</summary>
    public interface IAnswerRecordAppService : IApplicationService
    {
        /// <summary>
		/// 获取AnswerRecord的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<AnswerRecordListDto>> GetPaged(GetAnswerRecordsInput input);


		/// <summary>
		/// 通过指定id获取AnswerRecordListDto信息
		/// </summary>
		Task<AnswerRecordListDto> GetById(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetAnswerRecordForEditOutput> GetForEdit(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改AnswerRecord的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateAnswerRecordInput input);


        /// <summary>
        /// 删除AnswerRecord信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除AnswerRecord
        /// </summary>
        Task BatchDelete(List<Guid> input);


        /// <summary>
        /// 导出AnswerRecord为excel表
        /// </summary>
        /// <returns></returns>
        //Task<FileDto> GetToExcel();

        Task<List<AnswerRecordWXListDto>> WXGetAnswerRecordList(GetAnswerRecordsInput input);

    }
}

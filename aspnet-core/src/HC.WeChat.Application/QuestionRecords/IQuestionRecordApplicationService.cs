
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


using HC.WeChat.QuestionRecords.Dtos;
using HC.WeChat.QuestionRecords;
using HC.WeChat.Dto;

namespace HC.WeChat.QuestionRecords
{
    /// <summary>
    /// QuestionRecord应用层服务的接口方法
    ///</summary>
    public interface IQuestionRecordAppService : IApplicationService
    {
        /// <summary>
		/// 获取QuestionRecord的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<QuestionRecordListDto>> GetPaged(GetQuestionRecordsInput input);

        /// <summary>
        /// 分页获取零售户问卷填写记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<RetailQuestionRecordListDto>> GetPagedByRetailerId(GetQuestionRecordsInput input);

        Task<RetailQuestionRecordListDto> GetRetailQuetionRecordHead(GetQuestionRecordHeadInput input);


        /// <summary>
        /// 通过指定id获取QuestionRecordListDto信息
        /// </summary>
        Task<QuestionRecordListDto> GetById(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetQuestionRecordForEditOutput> GetForEdit(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改QuestionRecord的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<APIResultDto> CreateOrUpdate(QuestionRecordEditDto input);


        /// <summary>
        /// 删除QuestionRecord信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<APIResultDto> Delete(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除QuestionRecord
        /// </summary>
        Task BatchDelete(List<Guid> input);


		/// <summary>
        /// 导出QuestionRecord为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetToExcel();

    }
}


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


using HC.WeChat.QuestionOptions.Dtos;
using HC.WeChat.QuestionOptions;

namespace HC.WeChat.QuestionOptions
{
    /// <summary>
    /// QuestionOption应用层服务的接口方法
    ///</summary>
    public interface IQuestionOptionAppService : IApplicationService
    {
        /// <summary>
		/// 获取QuestionOption的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<QuestionOptionListDto>> GetPaged(GetQuestionOptionsInput input);


		/// <summary>
		/// 通过指定id获取QuestionOptionListDto信息
		/// </summary>
		Task<QuestionOptionListDto> GetById(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetQuestionOptionForEditOutput> GetForEdit(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改QuestionOption的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateQuestionOptionInput input);


        /// <summary>
        /// 删除QuestionOption信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除QuestionOption
        /// </summary>
        Task BatchDelete(List<Guid> input);


		/// <summary>
        /// 导出QuestionOption为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetToExcel();

    }
}

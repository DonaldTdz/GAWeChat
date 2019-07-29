
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


using HC.WeChat.Questionnaires.Dtos;
using HC.WeChat.Questionnaires;

namespace HC.WeChat.Questionnaires
{
    /// <summary>
    /// Questionnaire应用层服务的接口方法
    ///</summary>
    public interface IQuestionnaireAppService : IApplicationService
    {
        /// <summary>
		/// 获取Questionnaire的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<QuestionnaireListDto>> GetPaged(GetQuestionnairesInput input);


		/// <summary>
		/// 通过指定id获取QuestionnaireListDto信息
		/// </summary>
		Task<QuestionnaireListDto> GetById(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetQuestionnaireForEditOutput> GetForEdit(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改Questionnaire的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateQuestionnaireInput input);


        /// <summary>
        /// 删除Questionnaire信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除Questionnaire
        /// </summary>
        Task BatchDelete(List<Guid> input);


		/// <summary>
        /// 导出Questionnaire为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetToExcel();

    }
}

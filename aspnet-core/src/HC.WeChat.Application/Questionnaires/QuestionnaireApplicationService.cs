
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
using Abp.Extensions;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;


using HC.WeChat.Questionnaires;
using HC.WeChat.Questionnaires.Dtos;
using HC.WeChat.Questionnaires.DomainService;



namespace HC.WeChat.Questionnaires
{
    /// <summary>
    /// Questionnaire应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class QuestionnaireAppService : WeChatAppServiceBase, IQuestionnaireAppService
    {
        private readonly IRepository<Questionnaire, Guid> _entityRepository;

        private readonly IQuestionnaireManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public QuestionnaireAppService(
        IRepository<Questionnaire, Guid> entityRepository
        ,IQuestionnaireManager entityManager
        )
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
        }


        /// <summary>
        /// 获取Questionnaire的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		 
        public async Task<PagedResultDto<QuestionnaireListDto>> GetPaged(GetQuestionnairesInput input)
		{

		    var query = _entityRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<QuestionnaireListDto>>(entityList);
			var entityListDtos =entityList.MapTo<List<QuestionnaireListDto>>();

			return new PagedResultDto<QuestionnaireListDto>(count,entityListDtos);
		}


		/// <summary>
		/// 通过指定id获取QuestionnaireListDto信息
		/// </summary>
		 
		public async Task<QuestionnaireListDto> GetById(EntityDto<Guid> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return entity.MapTo<QuestionnaireListDto>();
		}

		/// <summary>
		/// 获取编辑 Questionnaire
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task<GetQuestionnaireForEditOutput> GetForEdit(NullableIdDto<Guid> input)
		{
			var output = new GetQuestionnaireForEditOutput();
QuestionnaireEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = entity.MapTo<QuestionnaireEditDto>();

				//questionnaireEditDto = ObjectMapper.Map<List<questionnaireEditDto>>(entity);
			}
			else
			{
				editDto = new QuestionnaireEditDto();
			}

			output.Questionnaire = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改Questionnaire的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task CreateOrUpdate(CreateOrUpdateQuestionnaireInput input)
		{

			if (input.Questionnaire.Id.HasValue)
			{
				await Update(input.Questionnaire);
			}
			else
			{
				await Create(input.Questionnaire);
			}
		}


		/// <summary>
		/// 新增Questionnaire
		/// </summary>
		
		protected virtual async Task<QuestionnaireEditDto> Create(QuestionnaireEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <Questionnaire>(input);
            var entity=input.MapTo<Questionnaire>();
			

			entity = await _entityRepository.InsertAsync(entity);
			return entity.MapTo<QuestionnaireEditDto>();
		}

		/// <summary>
		/// 编辑Questionnaire
		/// </summary>
		
		protected virtual async Task Update(QuestionnaireEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除Questionnaire信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task Delete(EntityDto<Guid> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除Questionnaire的方法
		/// </summary>
		
		public async Task BatchDelete(List<Guid> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出Questionnaire为excel表,等待开发。
		/// </summary>
		/// <returns></returns>
		//public async Task<FileDto> GetToExcel()
		//{
		//	var users = await UserManager.Users.ToListAsync();
		//	var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
		//	await FillRoleNames(userListDtos);
		//	return _userListExcelExporter.ExportToFile(userListDtos);
		//}

    }
}



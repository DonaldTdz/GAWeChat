
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


using HC.WeChat.QuestionOptions;
using HC.WeChat.QuestionOptions.Dtos;
using HC.WeChat.QuestionOptions.DomainService;



namespace HC.WeChat.QuestionOptions
{
    /// <summary>
    /// QuestionOption应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class QuestionOptionAppService : WeChatAppServiceBase, IQuestionOptionAppService
    {
        private readonly IRepository<QuestionOption, Guid> _entityRepository;

        private readonly IQuestionOptionManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public QuestionOptionAppService(
        IRepository<QuestionOption, Guid> entityRepository
        ,IQuestionOptionManager entityManager
        )
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
        }


        /// <summary>
        /// 获取QuestionOption的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		 
        public async Task<PagedResultDto<QuestionOptionListDto>> GetPaged(GetQuestionOptionsInput input)
		{

		    var query = _entityRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<QuestionOptionListDto>>(entityList);
			var entityListDtos =entityList.MapTo<List<QuestionOptionListDto>>();

			return new PagedResultDto<QuestionOptionListDto>(count,entityListDtos);
		}


		/// <summary>
		/// 通过指定id获取QuestionOptionListDto信息
		/// </summary>
		 
		public async Task<QuestionOptionListDto> GetById(EntityDto<Guid> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return entity.MapTo<QuestionOptionListDto>();
		}

		/// <summary>
		/// 获取编辑 QuestionOption
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task<GetQuestionOptionForEditOutput> GetForEdit(NullableIdDto<Guid> input)
		{
			var output = new GetQuestionOptionForEditOutput();
QuestionOptionEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = entity.MapTo<QuestionOptionEditDto>();

				//questionOptionEditDto = ObjectMapper.Map<List<questionOptionEditDto>>(entity);
			}
			else
			{
				editDto = new QuestionOptionEditDto();
			}

			output.QuestionOption = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改QuestionOption的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task CreateOrUpdate(CreateOrUpdateQuestionOptionInput input)
		{

			if (input.QuestionOption.Id.HasValue)
			{
				await Update(input.QuestionOption);
			}
			else
			{
				await Create(input.QuestionOption);
			}
		}


		/// <summary>
		/// 新增QuestionOption
		/// </summary>
		
		protected virtual async Task<QuestionOptionEditDto> Create(QuestionOptionEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <QuestionOption>(input);
            var entity=input.MapTo<QuestionOption>();
			

			entity = await _entityRepository.InsertAsync(entity);
			return entity.MapTo<QuestionOptionEditDto>();
		}

		/// <summary>
		/// 编辑QuestionOption
		/// </summary>
		
		protected virtual async Task Update(QuestionOptionEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除QuestionOption信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task Delete(EntityDto<Guid> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除QuestionOption的方法
		/// </summary>
		
		public async Task BatchDelete(List<Guid> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出QuestionOption为excel表,等待开发。
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



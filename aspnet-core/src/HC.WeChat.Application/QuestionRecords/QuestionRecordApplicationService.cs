
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


using HC.WeChat.QuestionRecords;
using HC.WeChat.QuestionRecords.Dtos;
using HC.WeChat.QuestionRecords.DomainService;



namespace HC.WeChat.QuestionRecords
{
    /// <summary>
    /// QuestionRecord应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class QuestionRecordAppService : WeChatAppServiceBase, IQuestionRecordAppService
    {
        private readonly IRepository<QuestionRecord, Guid> _entityRepository;

        private readonly IQuestionRecordManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public QuestionRecordAppService(
        IRepository<QuestionRecord, Guid> entityRepository
        ,IQuestionRecordManager entityManager
        )
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
        }


        /// <summary>
        /// 获取QuestionRecord的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		 
        public async Task<PagedResultDto<QuestionRecordListDto>> GetPaged(GetQuestionRecordsInput input)
		{

		    var query = _entityRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<QuestionRecordListDto>>(entityList);
			var entityListDtos =entityList.MapTo<List<QuestionRecordListDto>>();

			return new PagedResultDto<QuestionRecordListDto>(count,entityListDtos);
		}


		/// <summary>
		/// 通过指定id获取QuestionRecordListDto信息
		/// </summary>
		 
		public async Task<QuestionRecordListDto> GetById(EntityDto<Guid> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return entity.MapTo<QuestionRecordListDto>();
		}

		/// <summary>
		/// 获取编辑 QuestionRecord
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task<GetQuestionRecordForEditOutput> GetForEdit(NullableIdDto<Guid> input)
		{
			var output = new GetQuestionRecordForEditOutput();
QuestionRecordEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = entity.MapTo<QuestionRecordEditDto>();

				//questionRecordEditDto = ObjectMapper.Map<List<questionRecordEditDto>>(entity);
			}
			else
			{
				editDto = new QuestionRecordEditDto();
			}

			output.QuestionRecord = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改QuestionRecord的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task CreateOrUpdate(CreateOrUpdateQuestionRecordInput input)
		{

			if (input.QuestionRecord.Id.HasValue)
			{
				await Update(input.QuestionRecord);
			}
			else
			{
				await Create(input.QuestionRecord);
			}
		}


		/// <summary>
		/// 新增QuestionRecord
		/// </summary>
		
		protected virtual async Task<QuestionRecordEditDto> Create(QuestionRecordEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <QuestionRecord>(input);
            var entity=input.MapTo<QuestionRecord>();
			

			entity = await _entityRepository.InsertAsync(entity);
			return entity.MapTo<QuestionRecordEditDto>();
		}

		/// <summary>
		/// 编辑QuestionRecord
		/// </summary>
		
		protected virtual async Task Update(QuestionRecordEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除QuestionRecord信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task Delete(EntityDto<Guid> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除QuestionRecord的方法
		/// </summary>
		
		public async Task BatchDelete(List<Guid> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出QuestionRecord为excel表,等待开发。
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



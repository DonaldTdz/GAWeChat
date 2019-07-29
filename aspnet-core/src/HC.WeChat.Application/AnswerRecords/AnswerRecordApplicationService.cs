
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


using HC.WeChat.AnswerRecords;
using HC.WeChat.AnswerRecords.Dtos;
using HC.WeChat.AnswerRecords.DomainService;



namespace HC.WeChat.AnswerRecords
{
    /// <summary>
    /// AnswerRecord应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class AnswerRecordAppService : WeChatAppServiceBase, IAnswerRecordAppService
    {
        private readonly IRepository<AnswerRecord, Guid> _entityRepository;

        private readonly IAnswerRecordManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public AnswerRecordAppService(
        IRepository<AnswerRecord, Guid> entityRepository
        ,IAnswerRecordManager entityManager
        )
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
        }


        /// <summary>
        /// 获取AnswerRecord的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		 
        public async Task<PagedResultDto<AnswerRecordListDto>> GetPaged(GetAnswerRecordsInput input)
		{

		    var query = _entityRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<AnswerRecordListDto>>(entityList);
			var entityListDtos =entityList.MapTo<List<AnswerRecordListDto>>();

			return new PagedResultDto<AnswerRecordListDto>(count,entityListDtos);
		}


		/// <summary>
		/// 通过指定id获取AnswerRecordListDto信息
		/// </summary>
		 
		public async Task<AnswerRecordListDto> GetById(EntityDto<Guid> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return entity.MapTo<AnswerRecordListDto>();
		}

		/// <summary>
		/// 获取编辑 AnswerRecord
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task<GetAnswerRecordForEditOutput> GetForEdit(NullableIdDto<Guid> input)
		{
			var output = new GetAnswerRecordForEditOutput();
AnswerRecordEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = entity.MapTo<AnswerRecordEditDto>();

				//answerRecordEditDto = ObjectMapper.Map<List<answerRecordEditDto>>(entity);
			}
			else
			{
				editDto = new AnswerRecordEditDto();
			}

			output.AnswerRecord = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改AnswerRecord的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task CreateOrUpdate(CreateOrUpdateAnswerRecordInput input)
		{

			if (input.AnswerRecord.Id.HasValue)
			{
				await Update(input.AnswerRecord);
			}
			else
			{
				await Create(input.AnswerRecord);
			}
		}


		/// <summary>
		/// 新增AnswerRecord
		/// </summary>
		
		protected virtual async Task<AnswerRecordEditDto> Create(AnswerRecordEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <AnswerRecord>(input);
            var entity=input.MapTo<AnswerRecord>();
			

			entity = await _entityRepository.InsertAsync(entity);
			return entity.MapTo<AnswerRecordEditDto>();
		}

		/// <summary>
		/// 编辑AnswerRecord
		/// </summary>
		
		protected virtual async Task Update(AnswerRecordEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除AnswerRecord信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task Delete(EntityDto<Guid> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除AnswerRecord的方法
		/// </summary>
		
		public async Task BatchDelete(List<Guid> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出AnswerRecord为excel表,等待开发。
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



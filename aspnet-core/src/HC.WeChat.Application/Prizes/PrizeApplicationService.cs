
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


using HC.WeChat.Prizes;
using HC.WeChat.Prizes.Dtos;
using HC.WeChat.Prizes.DomainService;



namespace HC.WeChat.Prizes
{
    /// <summary>
    /// Prize应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class PrizeAppService : WeChatAppServiceBase, IPrizeAppService
    {
        private readonly IRepository<Prize, Guid> _entityRepository;

        private readonly IPrizeManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public PrizeAppService(
        IRepository<Prize, Guid> entityRepository
        ,IPrizeManager entityManager
        )
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
        }


        /// <summary>
        /// 获取Prize的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		 
        public async Task<PagedResultDto<PrizeListDto>> GetPaged(GetPrizesInput input)
		{

		    var query = _entityRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<PrizeListDto>>(entityList);
			var entityListDtos =entityList.MapTo<List<PrizeListDto>>();

			return new PagedResultDto<PrizeListDto>(count,entityListDtos);
		}


		/// <summary>
		/// 通过指定id获取PrizeListDto信息
		/// </summary>
		 
		public async Task<PrizeListDto> GetById(EntityDto<Guid> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return entity.MapTo<PrizeListDto>();
		}

		/// <summary>
		/// 获取编辑 Prize
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task<GetPrizeForEditOutput> GetForEdit(NullableIdDto<Guid> input)
		{
			var output = new GetPrizeForEditOutput();
PrizeEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = entity.MapTo<PrizeEditDto>();

				//prizeEditDto = ObjectMapper.Map<List<prizeEditDto>>(entity);
			}
			else
			{
				editDto = new PrizeEditDto();
			}

			output.Prize = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改Prize的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task CreateOrUpdate(CreateOrUpdatePrizeInput input)
		{

			if (input.Prize.Id.HasValue)
			{
				await Update(input.Prize);
			}
			else
			{
				await Create(input.Prize);
			}
		}


		/// <summary>
		/// 新增Prize
		/// </summary>
		
		protected virtual async Task<PrizeEditDto> Create(PrizeEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <Prize>(input);
            var entity=input.MapTo<Prize>();
			

			entity = await _entityRepository.InsertAsync(entity);
			return entity.MapTo<PrizeEditDto>();
		}

		/// <summary>
		/// 编辑Prize
		/// </summary>
		
		protected virtual async Task Update(PrizeEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除Prize信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task Delete(EntityDto<Guid> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除Prize的方法
		/// </summary>
		
		public async Task BatchDelete(List<Guid> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出Prize为excel表,等待开发。
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



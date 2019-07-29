
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


using HC.WeChat.DemandForecasts;
using HC.WeChat.DemandForecasts.Dtos;
using HC.WeChat.DemandForecasts.DomainService;



namespace HC.WeChat.DemandForecasts
{
    /// <summary>
    /// DemandForecast应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class DemandForecastAppService : WeChatAppServiceBase, IDemandForecastAppService
    {
        private readonly IRepository<DemandForecast, Guid> _entityRepository;

        private readonly IDemandForecastManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public DemandForecastAppService(
        IRepository<DemandForecast, Guid> entityRepository
        ,IDemandForecastManager entityManager
        )
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
        }


        /// <summary>
        /// 获取DemandForecast的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		 
        public async Task<PagedResultDto<DemandForecastListDto>> GetPaged(GetDemandForecastsInput input)
		{

		    var query = _entityRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<DemandForecastListDto>>(entityList);
			var entityListDtos =entityList.MapTo<List<DemandForecastListDto>>();

			return new PagedResultDto<DemandForecastListDto>(count,entityListDtos);
		}


		/// <summary>
		/// 通过指定id获取DemandForecastListDto信息
		/// </summary>
		 
		public async Task<DemandForecastListDto> GetById(EntityDto<Guid> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return entity.MapTo<DemandForecastListDto>();
		}

		/// <summary>
		/// 获取编辑 DemandForecast
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task<GetDemandForecastForEditOutput> GetForEdit(NullableIdDto<Guid> input)
		{
			var output = new GetDemandForecastForEditOutput();
DemandForecastEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = entity.MapTo<DemandForecastEditDto>();

				//demandForecastEditDto = ObjectMapper.Map<List<demandForecastEditDto>>(entity);
			}
			else
			{
				editDto = new DemandForecastEditDto();
			}

			output.DemandForecast = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改DemandForecast的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task CreateOrUpdate(CreateOrUpdateDemandForecastInput input)
		{

			if (input.DemandForecast.Id.HasValue)
			{
				await Update(input.DemandForecast);
			}
			else
			{
				await Create(input.DemandForecast);
			}
		}


		/// <summary>
		/// 新增DemandForecast
		/// </summary>
		
		protected virtual async Task<DemandForecastEditDto> Create(DemandForecastEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <DemandForecast>(input);
            var entity=input.MapTo<DemandForecast>();
			

			entity = await _entityRepository.InsertAsync(entity);
			return entity.MapTo<DemandForecastEditDto>();
		}

		/// <summary>
		/// 编辑DemandForecast
		/// </summary>
		
		protected virtual async Task Update(DemandForecastEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除DemandForecast信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task Delete(EntityDto<Guid> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除DemandForecast的方法
		/// </summary>
		
		public async Task BatchDelete(List<Guid> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出DemandForecast为excel表,等待开发。
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



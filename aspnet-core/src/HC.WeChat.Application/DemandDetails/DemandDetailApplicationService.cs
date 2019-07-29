
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


using HC.WeChat.DemandDetails;
using HC.WeChat.DemandDetails.Dtos;
using HC.WeChat.DemandDetails.DomainService;



namespace HC.WeChat.DemandDetails
{
    /// <summary>
    /// DemandDetail应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class DemandDetailAppService : WeChatAppServiceBase, IDemandDetailAppService
    {
        private readonly IRepository<DemandDetail, Guid> _entityRepository;

        private readonly IDemandDetailManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public DemandDetailAppService(
        IRepository<DemandDetail, Guid> entityRepository
        ,IDemandDetailManager entityManager
        )
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
        }


        /// <summary>
        /// 获取DemandDetail的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		 
        public async Task<PagedResultDto<DemandDetailListDto>> GetPaged(GetDemandDetailsInput input)
		{

		    var query = _entityRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<DemandDetailListDto>>(entityList);
			var entityListDtos =entityList.MapTo<List<DemandDetailListDto>>();

			return new PagedResultDto<DemandDetailListDto>(count,entityListDtos);
		}


		/// <summary>
		/// 通过指定id获取DemandDetailListDto信息
		/// </summary>
		 
		public async Task<DemandDetailListDto> GetById(EntityDto<Guid> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return entity.MapTo<DemandDetailListDto>();
		}

		/// <summary>
		/// 获取编辑 DemandDetail
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task<GetDemandDetailForEditOutput> GetForEdit(NullableIdDto<Guid> input)
		{
			var output = new GetDemandDetailForEditOutput();
DemandDetailEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = entity.MapTo<DemandDetailEditDto>();

				//demandDetailEditDto = ObjectMapper.Map<List<demandDetailEditDto>>(entity);
			}
			else
			{
				editDto = new DemandDetailEditDto();
			}

			output.DemandDetail = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改DemandDetail的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task CreateOrUpdate(CreateOrUpdateDemandDetailInput input)
		{

			if (input.DemandDetail.Id.HasValue)
			{
				await Update(input.DemandDetail);
			}
			else
			{
				await Create(input.DemandDetail);
			}
		}


		/// <summary>
		/// 新增DemandDetail
		/// </summary>
		
		protected virtual async Task<DemandDetailEditDto> Create(DemandDetailEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <DemandDetail>(input);
            var entity=input.MapTo<DemandDetail>();
			

			entity = await _entityRepository.InsertAsync(entity);
			return entity.MapTo<DemandDetailEditDto>();
		}

		/// <summary>
		/// 编辑DemandDetail
		/// </summary>
		
		protected virtual async Task Update(DemandDetailEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除DemandDetail信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task Delete(EntityDto<Guid> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除DemandDetail的方法
		/// </summary>
		
		public async Task BatchDelete(List<Guid> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出DemandDetail为excel表,等待开发。
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



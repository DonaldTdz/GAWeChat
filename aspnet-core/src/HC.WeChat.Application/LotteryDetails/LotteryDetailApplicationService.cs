
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


using HC.WeChat.LotteryDetails;
using HC.WeChat.LotteryDetails.Dtos;
using HC.WeChat.LotteryDetails.DomainService;



namespace HC.WeChat.LotteryDetails
{
    /// <summary>
    /// LotteryDetail应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class LotteryDetailAppService : WeChatAppServiceBase, ILotteryDetailAppService
    {
        private readonly IRepository<LotteryDetail, Guid> _entityRepository;

        private readonly ILotteryDetailManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public LotteryDetailAppService(
        IRepository<LotteryDetail, Guid> entityRepository
        ,ILotteryDetailManager entityManager
        )
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
        }


        /// <summary>
        /// 获取LotteryDetail的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		 
        public async Task<PagedResultDto<LotteryDetailListDto>> GetPaged(GetLotteryDetailsInput input)
		{

		    var query = _entityRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<LotteryDetailListDto>>(entityList);
			var entityListDtos =entityList.MapTo<List<LotteryDetailListDto>>();

			return new PagedResultDto<LotteryDetailListDto>(count,entityListDtos);
		}


		/// <summary>
		/// 通过指定id获取LotteryDetailListDto信息
		/// </summary>
		 
		public async Task<LotteryDetailListDto> GetById(EntityDto<Guid> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return entity.MapTo<LotteryDetailListDto>();
		}

		/// <summary>
		/// 获取编辑 LotteryDetail
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task<GetLotteryDetailForEditOutput> GetForEdit(NullableIdDto<Guid> input)
		{
			var output = new GetLotteryDetailForEditOutput();
LotteryDetailEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = entity.MapTo<LotteryDetailEditDto>();

				//lotteryDetailEditDto = ObjectMapper.Map<List<lotteryDetailEditDto>>(entity);
			}
			else
			{
				editDto = new LotteryDetailEditDto();
			}

			output.LotteryDetail = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改LotteryDetail的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task CreateOrUpdate(CreateOrUpdateLotteryDetailInput input)
		{

			if (input.LotteryDetail.Id.HasValue)
			{
				await Update(input.LotteryDetail);
			}
			else
			{
				await Create(input.LotteryDetail);
			}
		}


		/// <summary>
		/// 新增LotteryDetail
		/// </summary>
		
		protected virtual async Task<LotteryDetailEditDto> Create(LotteryDetailEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <LotteryDetail>(input);
            var entity=input.MapTo<LotteryDetail>();
			

			entity = await _entityRepository.InsertAsync(entity);
			return entity.MapTo<LotteryDetailEditDto>();
		}

		/// <summary>
		/// 编辑LotteryDetail
		/// </summary>
		
		protected virtual async Task Update(LotteryDetailEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除LotteryDetail信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task Delete(EntityDto<Guid> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除LotteryDetail的方法
		/// </summary>
		
		public async Task BatchDelete(List<Guid> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出LotteryDetail为excel表,等待开发。
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



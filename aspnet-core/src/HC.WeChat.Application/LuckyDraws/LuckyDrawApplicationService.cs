
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


using HC.WeChat.LuckyDraws;
using HC.WeChat.LuckyDraws.Dtos;
using HC.WeChat.LuckyDraws.DomainService;
using HC.WeChat.Prizes;
using Abp.Auditing;
using HC.WeChat.Dto;
using HC.WeChat.LotteryDetails;

namespace HC.WeChat.LuckyDraws
{
    /// <summary>
    /// LuckyDraw应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class LuckyDrawAppService : WeChatAppServiceBase, ILuckyDrawAppService
    {
        private readonly IRepository<LuckyDraw, Guid> _entityRepository;

        private readonly ILuckyDrawManager _entityManager;

		private readonly IRepository<Prize, Guid> _PrizeRepository;

		private readonly IRepository<LotteryDetail, Guid> _LotteryDetailRepository;
		/// <summary>
		/// 构造函数 
		///</summary>
		public LuckyDrawAppService(
        IRepository<LuckyDraw, Guid> entityRepository
        ,ILuckyDrawManager entityManager
			, IRepository<Prize, Guid> PrizeRepository
			, IRepository<LotteryDetail, Guid> LotteryDetailRepository
		)
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
			_PrizeRepository = PrizeRepository;
			_LotteryDetailRepository = LotteryDetailRepository;

		}


        /// <summary>
        /// 获取LuckyDraw的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		 
        public async Task<PagedResultDto<LuckyDrawListDto>> GetPaged(GetLuckyDrawsInput input)
		{

		    var query = _entityRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<LuckyDrawListDto>>(entityList);
			var entityListDtos =entityList.MapTo<List<LuckyDrawListDto>>();

			return new PagedResultDto<LuckyDrawListDto>(count,entityListDtos);
		}


		/// <summary>
		/// 通过指定id获取LuckyDrawListDto信息
		/// </summary>
		 
		public async Task<LuckyDrawListDto> GetById(EntityDto<Guid> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return entity.MapTo<LuckyDrawListDto>();
		}

		/// <summary>
		/// 获取编辑 LuckyDraw
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task<GetLuckyDrawForEditOutput> GetForEdit(NullableIdDto<Guid> input)
		{
			var output = new GetLuckyDrawForEditOutput();
LuckyDrawEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = entity.MapTo<LuckyDrawEditDto>();

				//luckyDrawEditDto = ObjectMapper.Map<List<luckyDrawEditDto>>(entity);
			}
			else
			{
				editDto = new LuckyDrawEditDto();
			}

			output.LuckyDraw = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改LuckyDraw的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task CreateOrUpdate(CreateOrUpdateLuckyDrawInput input)
		{

			if (input.LuckyDraw.Id.HasValue)
			{
				await Update(input.LuckyDraw);
			}
			else
			{
				await Create(input.LuckyDraw);
			}
		}


		/// <summary>
		/// 新增LuckyDraw
		/// </summary>
		
		protected virtual async Task<LuckyDrawEditDto> Create(LuckyDrawEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <LuckyDraw>(input);
            var entity=input.MapTo<LuckyDraw>();
			

			entity = await _entityRepository.InsertAsync(entity);
			return entity.MapTo<LuckyDrawEditDto>();
		}

		/// <summary>
		/// 编辑LuckyDraw
		/// </summary>
		
		protected virtual async Task Update(LuckyDrawEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除LuckyDraw信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task Delete(EntityDto<Guid> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除LuckyDraw的方法
		/// </summary>
		
		public async Task BatchDelete(List<Guid> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}

		/// <summary>
		/// 微信端新加入一个抽奖活动
		/// </summary>
		/// <returns></returns>
		[AbpAllowAnonymous]
		public async Task<APIResultDto> CreateWXLuckyDrawAsync(WeiXinCreateInput input) 
		{
			var entity = new LuckyDraw();
			entity.BeginTime = input.BeginTime.Value;
			entity.CreationTime = DateTime.Now;
			entity.EndTime = input.EndTime.Value;
			entity.IsPublish = input.IsPublish;
			entity.Name = input.Name;

			if (input.IsPublish) entity.PublishTime = DateTime.Now;

			var refEntity =await _entityRepository.InsertAsync(entity);

			
			foreach (var priceEn in input.List) {

				for(var i = 0; i < priceEn.Num; i++) 
				{ 
					Prize prize = new Prize();
					prize.CreationTime = DateTime.Now;
					prize.Name = priceEn.Name;
					prize.LuckyDrawId = refEntity.Id;
					prize.Num = 1;
					prize.Type = priceEn.Type;
					await _PrizeRepository.InsertAsync(prize);
				}
			}
			return new APIResultDto { Code =0};

		}

		/// <summary>
		/// 获取活动列表
		/// </summary>
		/// <returns></returns>
		[AbpAllowAnonymous]
		[DisableAuditing]
		public async Task<List<WXLuckyDrawOutput>> GetWXLuckyDrawListAsync() {

			var entities = from a in _entityRepository.GetAll()
					 .OrderByDescending(v => v.CreationTime)
						   select new WXLuckyDrawOutput
						   {
							   CreationTime=a.CreationTime,
							   Name=a.Name,
							   Id=a.Id,
							   IsPublish=a.IsPublish
						   };
			return await entities.ToListAsync();

		}
		/// <summary>
		/// 更改公布状态
		/// </summary>
		/// <param name="weiXinUpdatePubInput"></param>
		/// <returns></returns>
		[AbpAllowAnonymous]
		public async Task<APIResultDto> UpdateWXLuckyDrawPubStatusAsync(WeiXinUpdatePubInput weiXinUpdatePubInput) {

			try
			{

				var entity = await _entityRepository.GetAsync(weiXinUpdatePubInput.Id);
				if (!entity.IsPublish)
				{
					entity.IsPublish = true;
					entity.PublishTime = DateTime.Now;
					return new APIResultDto { Code = 1 };
				}
				else
				{
					return new APIResultDto { Code = 0 };
				}
			}
			catch (Exception ex) {
				return new APIResultDto { Code = 0 };
			}

		}
		/// <summary>
		/// 根据ID获取一个活动的详细信息 Admin
		/// </summary>
		/// <returns></returns>
		[AbpAllowAnonymous]
		[DisableAuditing]
		public async Task<WXLuckyDrawDetailIDOutput> GetLuckyDrawDetailByIdAsync(Guid Id) {

			var entity = await _entityRepository.GetAll()
				.Where(v=>v.Id==Id)
				.AsNoTracking()
				.FirstOrDefaultAsync();

			var prizes = await _PrizeRepository.GetAll().Where(v => v.LuckyDrawId == Id)
						  .GroupBy(v => new { v.Name, v.Type, v.Num })
						  .Select(m => new WeiXinPriceInput
						  {
							  Name = m.Key.Name,
							  Type = m.Key.Type,
							  Num = m.Sum(t => t.Num)

						  }).ToListAsync();

			return new WXLuckyDrawDetailIDOutput
			{
				Name=entity.Name,
				BeginTime=entity.BeginTime,
				EndTime=entity.EndTime,
				IsPublish=entity.IsPublish,
				List=prizes
			};


		}
		/// <summary>
		/// 内部员工获取开奖列表
		/// </summary>
		/// <returns></returns>
		[AbpAllowAnonymous]
		[DisableAuditing]
		public async Task<List<WXLuckyDrawOutput>> GetWXLuckyDrawListPublishedAsync() {

			var entities = await (from a in _entityRepository.GetAll().Where(v => v.IsPublish == true).OrderByDescending(v => v.CreationTime)
								  select new WXLuckyDrawOutput
								  {
									  Name = a.Name,
									  Id = a.Id,
									  BeginTime = a.BeginTime,
									  EndTime = a.EndTime,
									  LotteryState = a.EndTime >= DateTime.Now && a.BeginTime <= DateTime.Now ? true : false
								  }).ToListAsync();

			for (var i = 0; i < entities.Count(); i++) {

				var num = await _LotteryDetailRepository.CountAsync(v => v.LuckyDrawId == entities[i].Id.Value);
				var _num = await _LotteryDetailRepository.CountAsync(v => v.LuckyDrawId == entities[i].Id.Value && v.IsLottery == true);
				if (num == _num) { entities[i].LotteryState = entities[i].LotteryState || true; }
				else { entities[i].LotteryState = entities[i].LotteryState || false; }

			}
			

			return entities;
		}

    }
}



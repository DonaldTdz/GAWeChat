
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
using HC.WeChat.Employees;
using HC.WeChat.LuckySigns;
using HC.WeChat.WeChatUsers;
using HC.WeChat.LotteryDetails.DomainService;

namespace HC.WeChat.LuckyDraws
{
	/// <summary>
	/// LuckyDraw应用层服务的接口实现方法  
	///</summary>
	[AbpAuthorize]
	public class LuckyDrawAppService : WeChatAppServiceBase, ILuckyDrawAppService
	{
		private readonly IRepository<LuckyDraw, Guid> _entityRepository;

		private  readonly ILuckyDrawManager _entityManager;
		private  readonly IRepository<Prize, Guid> _PrizeRepository;
		private  readonly IRepository<LotteryDetail, Guid> _LotteryDetailRepository;
		private  readonly IRepository<Employee, Guid> _employeeRepository;
		private  readonly IRepository<LuckySign, Guid> _LuckySignRepository;
		private  readonly IRepository<WeChatUser, Guid> _wechatuserRepository;


		/// <summary>
		/// 构造函数 
		///</summary>
		public LuckyDrawAppService(
        IRepository<LuckyDraw, Guid> entityRepository
        ,ILuckyDrawManager entityManager
			, IRepository<Prize, Guid> PrizeRepository
			, IRepository<LotteryDetail, Guid> LotteryDetailRepository
			, IRepository<Employee, Guid> employeeRepository
			, IRepository<WeChatUser, Guid> wechatuserRepository
			, IRepository<LuckySign, Guid> LuckySignRepository
		)
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
			_PrizeRepository = PrizeRepository;
			_LotteryDetailRepository = LotteryDetailRepository;
			_employeeRepository = employeeRepository;
			_LuckySignRepository = LuckySignRepository;
			_wechatuserRepository = wechatuserRepository;

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
			APIResultDto result = new APIResultDto();
			entity.BeginTime = input.BeginTime.Value;
			entity.EndTime = input.EndTime.Value;
			entity.IsPublish = input.IsPublish;
			entity.Name = input.Name;
			if (input.IsPublish)
			{ 
				entity.PublishTime = DateTime.Now;
				
			}
			var refEntity =await _entityRepository.InsertAsync(entity);


			foreach (var priceEn in input.List) {
				for(var i = 0; i < priceEn.Num; i++) 
				{ 
					Prize prize = new Prize();
					prize.Name = priceEn.Name;
					prize.LuckyDrawId = refEntity.Id;
					prize.Num = 1;
					prize.Type = priceEn.Type;
					await _PrizeRepository.InsertAsync(prize);
				}
			}

			return new APIResultDto
			{
				Code = 0,
				Data = refEntity.Id
			};


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
							   CreationTime=a.EndTime,
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
		public async Task<APIResultDto> ChangeWXLuckyDrawPubStatusAsync(WeiXinUpdatePubInput weiXinUpdatePubInput) {

			try
			{

				var entity = await _entityRepository.GetAsync(weiXinUpdatePubInput.Id);
				if (!entity.IsPublish)
				{
					entity.IsPublish = true;
					entity.PublishTime = DateTime.Now;
					return new APIResultDto 
					{
						Code = 0 ,
						Msg="发布成功！"
					};
				}
				else
				{
					return new APIResultDto 
					{
						Code = 901,
						Msg="未获取到信息！"
					};
				}
			}
			catch (Exception ex) {
				return new APIResultDto 
				{ 
					Code = 902,
					Msg="发布失败！"
				};
			}

		}
		/// <summary>
		/// 根据ID获取一个活动的详细信息
		/// </summary>
		/// <returns></returns>
		[AbpAllowAnonymous]
		[DisableAuditing]
		public async Task<WXLuckyDrawDetailIDOutput> GetLuckyDrawDetailByIdAsync(Guid Id,string openId) {

			var entity = await _entityRepository.GetAll()
				.Where(v => v.Id == Id)
				.AsNoTracking()
				.FirstOrDefaultAsync();
			//获取奖品列表
			var prizes = await _PrizeRepository.GetAll().Where(v => v.LuckyDrawId == Id)
						  .GroupBy(v => new { v.Name, v.Type, v.Num })
						  .Select(m => new WeiXinPriceInput
						  {
							  Name = m.Key.Name,
							  Type = m.Key.Type,
							  Num = m.Sum(t => t.Num)

						  }).ToListAsync();
			//所有人是否都已经开奖
			var isAlllottery = entity.EndTime <= DateTime.Now ? true : false;

			isAlllottery = isAlllottery || await _LotteryDetailRepository.GetAll().Where(v => v.LuckyDrawId == Id).AllAsync(v => v.IsLottery == true);

			List<LotteryDetailDto> lotteryDetailoOut = new List<LotteryDetailDto>();
			if (isAlllottery)
			{
				//获取相应的中奖列表
				lotteryDetailoOut = await (from l in _LotteryDetailRepository.GetAll().Where(v => v.LuckyDrawId == Id)
										   join e in _employeeRepository.GetAll()
										   on l.UserId equals e.Id
										   select new LotteryDetailDto
										   {
											   Name = e.Name,
											   Num = 1,
											   PrizeName = l.PrizeName
										   }).ToListAsync();
			}
			return new WXLuckyDrawDetailIDOutput
			{
				Name = entity.Name,//活动名字
				BeginTime = entity.BeginTime,
				EndTime = entity.EndTime,
				IsPublish = entity.IsPublish,//是否公布
				List = prizes,//奖品列表
				LotteryState = isAlllottery,//是否开奖	当前时间超过截至日期或所有都点击了开奖
				LotteryDetails = lotteryDetailoOut,//中奖名单
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
									  IsPublish=true,
									  Name = a.Name,
									  Id = a.Id,
									  BeginTime = a.BeginTime,
									  EndTime = a.EndTime,
									  LotteryState = a.EndTime <= DateTime.Now ? true : false,
									  CreationTime=a.CreationTime
								  }).ToListAsync();

			for (var i = 0; i < entities.Count(); i++)
			{
				var num = await _LotteryDetailRepository.CountAsync(v => v.LuckyDrawId == entities[i].Id.Value);
				var _num = await _LotteryDetailRepository.CountAsync(v => v.LuckyDrawId == entities[i].Id.Value && v.IsLottery == true);
				if (num == _num&&num>0) { entities[i].LotteryState = entities[i].LotteryState || true; }
				else { entities[i].LotteryState = entities[i].LotteryState || false; }
			}
			return entities;
		}

		/// <summary>
		/// 展示参与抽奖活动的部门和人员详情列表 
		/// </summary>
		/// <returns></returns>
		[AbpAllowAnonymous]
		[DisableAuditing]
		public async Task<List<LotteryJoinDeptDetailOutput>> GetLotteryJoinDeptDetailAsync(Guid Id,string DeptName) 
		{
			return await (from e in _employeeRepository.GetAll().Where(v => v.DeptName == DeptName)
						  join l in _LotteryDetailRepository.GetAll().Where(v => v.LuckyDrawId == Id&&v.IsLottery==true)
						  on e.Id equals l.UserId into table
						  from tb in table.DefaultIfEmpty()
						  select new LotteryJoinDeptDetailOutput
						  {
							  Name = e.Name,
							  Code = e.Code,
							  IsJoin = tb!=null?true:false
						  }).ToListAsync();
		}

		/// <summary>
		/// 获取参与抽奖活动的人员统计数字
		/// </summary>
		/// <returns></returns>
		[AbpAllowAnonymous]
		[DisableAuditing]
		public async Task<GetLuckyDrawPersonCountDto> GetLuckyDrawPersonCountAsync(Guid luckyId) 
		{
			//获取员工总人数
			var num_total = await _employeeRepository.CountAsync();

			//获取所有员工ID
			var employeeList = await _employeeRepository.GetAll().Select(v => v.Id).ToListAsync();
			//获取已参与抽奖
			var num_lottery = await _LotteryDetailRepository.GetAll().CountAsync(v => employeeList.Contains(v.UserId)&&v.LuckyDrawId==luckyId&&v.IsLottery==true);

			return new GetLuckyDrawPersonCountDto
			{
				Num_Lottery=num_lottery,
				Num_Total=num_total
			};
		}
		/// <summary>
		/// 分部门显示抽奖人数
		/// </summary>
		/// <param name="luckyId"></param>
		/// <returns></returns>
		[AbpAllowAnonymous]
		[DisableAuditing]
		public async Task<List<GetLuckyDeptmentLotteryPersonDto>> GetLuckyDeptmentLotteryPersonAsync(Guid luckyId) 
		{
			var query = from e in _employeeRepository.GetAll()
						join d in _LotteryDetailRepository.GetAll().Where(v => v.LuckyDrawId == luckyId)
						on e.Id equals d.UserId into table
						from tb in table.DefaultIfEmpty()
						select new
						{
							e.DeptName,
							tb.IsLottery
						};

			return await (from c in query
						  group c by c.DeptName into tB
						  select new GetLuckyDeptmentLotteryPersonDto
						  {
							  Name=tB.Key,
							  Num_Lottery=tB.Count(v=>v.IsLottery==true),
							  Num_Total=tB.Count()
						  }
						  ).ToListAsync();
						   
						   
		}
	}
}



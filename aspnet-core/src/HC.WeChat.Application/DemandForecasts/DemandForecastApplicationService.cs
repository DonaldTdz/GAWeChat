
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
using HC.WeChat.Authorization;
using HC.WeChat.Dto;
using HC.WeChat.ForecastRecords;
using HC.WeChat.WeChatUsers;
using HC.WeChat.Retailers;

namespace HC.WeChat.DemandForecasts
{
    /// <summary>
    /// DemandForecast应用层服务的接口实现方法  
    ///</summary>
    //[AbpAuthorize]
    [AbpAuthorize(AppPermissions.Pages)]
    public class DemandForecastAppService : WeChatAppServiceBase, IDemandForecastAppService
    {
        private readonly IRepository<DemandForecast, Guid> _entityRepository;
        private readonly IRepository<ForecastRecord, Guid> _forecastRecordRepository;
        private readonly IRepository<WeChatUser, Guid> _wechatuserRepository;
        private readonly IRepository<Retailer, Guid> _retailerRepository;
        private readonly IDemandForecastManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public DemandForecastAppService(
        IRepository<DemandForecast, Guid> entityRepository
        , IRepository<ForecastRecord, Guid> forecastRecordRepository
        , IRepository<WeChatUser, Guid> wechatuserRepository
        , IRepository<Retailer, Guid> retailerRepository
        , IDemandForecastManager entityManager
        )
        {
            _entityRepository = entityRepository;
            _forecastRecordRepository = forecastRecordRepository;
            _wechatuserRepository = wechatuserRepository;
            _retailerRepository = retailerRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取DemandForecast的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<DemandForecastListDto>> GetPaged(GetDemandForecastsInput input)
        {

            var query = _entityRepository.GetAll().WhereIf(!string.IsNullOrEmpty(input.Filter), v => v.Title.Contains(input.Filter));
            var count = await query.CountAsync();
            var entityList = await query
                    .OrderBy(v => v.IsPublish).ThenByDescending(v => v.Month).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();
            var entityListDtos = entityList.MapTo<List<DemandForecastListDto>>();
            return new PagedResultDto<DemandForecastListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取DemandForecastListDto信息
        /// </summary>

        public async Task<DemandForecastListDto> GetById(Guid id)
        {
            var entity = await _entityRepository.GetAsync(id);
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

        public async Task<APIResultDto> CreateOrUpdate(DemandForecastEditDto input)
        {

            if (input.Id.HasValue)
            {
                if (input.IsPublish == true)
                {
                    input.PublishTime = DateTime.Now;
                }
                var entity = await Update(input);
                return new APIResultDto() { Code = 0, Data = entity };
            }
            else
            {
                var entity = await Create(input);
                return new APIResultDto() { Code = 0, Data = entity };
            }
        }


        /// <summary>
        /// 新增DemandForecast
        /// </summary>

        protected virtual async Task<DemandForecastEditDto> Create(DemandForecastEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <DemandForecast>(input);
            var entity = input.MapTo<DemandForecast>();


            entity = await _entityRepository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return entity.MapTo<DemandForecastEditDto>();
        }

        /// <summary>
        /// 编辑DemandForecast
        /// </summary>

        protected virtual async Task<DemandForecast> Update(DemandForecastEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return entity;
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
        /// 微信获取需求预测记录
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<List<DemandWXListDto>> GetWXDemandListAsync()
        {
            var query = _entityRepository.GetAll().Where(v => v.IsPublish == true);
            var list = await (from q in query
                              select new DemandWXListDto()
                              {
                                  Id = q.Id,
                                  Month = q.Month,
                                  Title = q.Title,
                                  //Status = count>0?(DateTime.Now.Month >= q.Month.Value.Month ? "进行中" : "已逾期"):"已填写"
                              }).OrderByDescending(v => v.Month).ToListAsync();
            DateTime today = DateTime.Today;
            DateTime now = DateTime.Now;
            //本月最后一天
            DateTime endDay = today.AddMonths(1).AddDays(-today.Day);
            DateTime endTime = endDay.AddDays((86399F / 86400));
            DateTime beginTime = endDay.AddDays(-7);
            foreach (var item in list)
            {
                int count = await _forecastRecordRepository.CountAsync(v => v.DemandForecastId == item.Id);
                item.Status = count <= 0 ? (DateTime.Now.Month > item.Month.Value.Month ? "已逾期" : ((now >= beginTime || now <= endTime) ? "进行中" : "未开始")) : "查看记录";
            }
            return list;
        }


        /// <summary>
        /// 查询当前零售户需求预测列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<RetailDemandForecastListDto>> GetRetailDemandListByIdAsync(GetDemandForecastsInput input)
        {
            string openId = await _wechatuserRepository.GetAll().Where(v => v.UserId == input.UserId).Select(v => v.OpenId).FirstOrDefaultAsync();
            Guid[] DemandIds = await _forecastRecordRepository.GetAll().Where(v => v.OpenId == openId).Select(v => v.DemandForecastId).Distinct().ToArrayAsync();
            var query = _entityRepository.GetAll().Where(v => DemandIds.Contains(v.Id)).WhereIf(!string.IsNullOrEmpty(input.Filter), v => v.Title.Contains(input.Filter));
            var list = await (from q in query
                              select new RetailDemandForecastListDto()
                              {
                                  Id = q.Id,
                                  Title = q.Title,
                                  Month = q.Month
                              }).OrderByDescending(v => v.Month).PageBy(input).ToListAsync();
            foreach (var item in list)
            {
                item.CompleteTime = await _forecastRecordRepository.GetAll().Where(v => v.DemandForecastId == item.Id).Select(v => v.CreationTime).OrderByDescending(v => v).FirstOrDefaultAsync();
            }
            int count = query.Count();
            return new PagedResultDto<RetailDemandForecastListDto>(count, list);
        }

        /// <summary>
        /// 获取零售户预测列表头部信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RetailDemandHeadDto> GetRetailDemandHeadByIdAsync(GetDemandForecastsInput input)
        {
            string openId = await _wechatuserRepository.GetAll().Where(v => v.UserId == input.UserId).Select(v => v.OpenId).FirstOrDefaultAsync();
            string name = await _retailerRepository.GetAll().Where(v => v.Id == input.UserId).Select(v => v.Name).FirstOrDefaultAsync();
            DateTime completeTime = await _forecastRecordRepository.GetAll().Where(v => v.DemandForecastId == input.DemandForecastId && v.OpenId == openId).Select(v => v.CreationTime).OrderByDescending(v => v).FirstOrDefaultAsync();
            var result = await _entityRepository.GetAll().Where(v => v.Id == input.DemandForecastId).Select(v => new RetailDemandHeadDto()
            {
                Id = v.Id,
                Title = v.Title,
                Month = v.Month,
                Name = name,
                CompleteTime = completeTime
            }).FirstOrDefaultAsync();
            return result;
        }
    }
}



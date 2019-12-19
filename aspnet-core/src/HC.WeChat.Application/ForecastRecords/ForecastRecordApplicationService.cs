
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


using HC.WeChat.ForecastRecords;
using HC.WeChat.ForecastRecords.Dtos;
using HC.WeChat.ForecastRecords.DomainService;
using HC.WeChat.Authorization;
using HC.WeChat.Dto;

namespace HC.WeChat.ForecastRecords
{
    /// <summary>
    /// ForecastRecord应用层服务的接口实现方法  
    ///</summary>
    //[AbpAuthorize]
    [AbpAuthorize(AppPermissions.Pages)]
    public class ForecastRecordAppService : WeChatAppServiceBase, IForecastRecordAppService
    {
        private readonly IRepository<ForecastRecord, Guid> _entityRepository;

        private readonly IForecastRecordManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ForecastRecordAppService(
        IRepository<ForecastRecord, Guid> entityRepository
        , IForecastRecordManager entityManager
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取ForecastRecord的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<ForecastRecordListDto>> GetPaged(GetForecastRecordsInput input)
        {

            var query = _entityRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件


            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<ForecastRecordListDto>>(entityList);
            var entityListDtos = entityList.MapTo<List<ForecastRecordListDto>>();

            return new PagedResultDto<ForecastRecordListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取ForecastRecordListDto信息
        /// </summary>

        public async Task<ForecastRecordListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<ForecastRecordListDto>();
        }

        /// <summary>
        /// 获取编辑 ForecastRecord
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetForecastRecordForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetForecastRecordForEditOutput();
            ForecastRecordEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<ForecastRecordEditDto>();

                //forecastRecordEditDto = ObjectMapper.Map<List<forecastRecordEditDto>>(entity);
            }
            else
            {
                editDto = new ForecastRecordEditDto();
            }

            output.ForecastRecord = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改ForecastRecord的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task CreateOrUpdate(CreateOrUpdateForecastRecordInput input)
        {

            if (input.ForecastRecord.Id.HasValue)
            {
                await Update(input.ForecastRecord);
            }
            else
            {
                await Create(input.ForecastRecord);
            }
        }


        /// <summary>
        /// 新增ForecastRecord
        /// </summary>

        protected virtual async Task<ForecastRecordEditDto> Create(ForecastRecordEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <ForecastRecord>(input);
            var entity = input.MapTo<ForecastRecord>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<ForecastRecordEditDto>();
        }

        /// <summary>
        /// 编辑ForecastRecord
        /// </summary>

        protected virtual async Task Update(ForecastRecordEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除ForecastRecord信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task Delete(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除ForecastRecord的方法
        /// </summary>

        public async Task BatchDelete(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 判断用户是否填写需求预测
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<bool> GetIsFillInDemandAsync(ForecastRecordWXEditDto input)
        {
           int num = await _entityRepository.CountAsync(v => v.DemandForecastId == input.DemandForecastId && v.OpenId == input.OpenId);
            if(num != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 微信保存预测结果
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<APIResultDto> CreateForecastRecordAsync(ForecastRecordWXEditDto input)
        {
            try
            {
                foreach (var item in input.List)
                {
                    var entity = new ForecastRecord();
                    entity.OpenId = input.OpenId;
                    entity.DemandForecastId = input.DemandForecastId;
                    entity.PredictiveValue = item.PredictiveValue;
                    entity.DemandDetailId = item.DemandDetailId;
                    await _entityRepository.InsertAsync(entity);
                }
                return new APIResultDto() { Code = 0, Msg = "保存成功" };
            }
            catch (Exception)
            {
                return new APIResultDto() { Code = 901, Msg = "保存失败，请重试" };
            }
        }
    }
}



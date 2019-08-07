
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
using HC.WeChat.Dto;
using HC.WeChat.AnswerRecords;
using HC.WeChat.WeChatUsers;
using HC.WeChat.Retailers;

namespace HC.WeChat.QuestionRecords
{
    /// <summary>
    /// QuestionRecord应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class QuestionRecordAppService : WeChatAppServiceBase, IQuestionRecordAppService
    {
        private readonly IRepository<QuestionRecord, Guid> _entityRepository;
        private readonly IRepository<WeChatUser, Guid> _weChatRepository;
        private readonly IRepository<Retailer, Guid> _retailerRepository;
        private readonly IRepository<AnswerRecord, Guid> _answerRepository;

        private readonly IQuestionRecordManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public QuestionRecordAppService(
        IRepository<QuestionRecord, Guid> entityRepository
        , IQuestionRecordManager entityManager
        , IRepository<WeChatUser, Guid> weChatRepository
        , IRepository<AnswerRecord, Guid> answerRepository
        , IRepository<Retailer, Guid> retailerRepository
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
            _answerRepository = answerRepository;
            _weChatRepository = weChatRepository;
            _retailerRepository = retailerRepository;
        }


        /// <summary>
        /// 获取QuestionRecord的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<QuestionRecordListDto>> GetPaged(GetQuestionRecordsInput input)
        {
            var query = _entityRepository.GetAll()
                .WhereIf(input.Quarter.HasValue, i => i.Quarter == input.Quarter.Value);

            // TODO:根据传入的参数添加过滤条件


           var count = await query.CountAsync();

           var entityList = await query
                    .OrderBy(q => q.IsPublish).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();


            // var entityListDtos = ObjectMapper.Map<List<QuestionRecordListDto>>(entityList);
            var entityListDtos = entityList.MapTo<List<QuestionRecordListDto>>();
            //后台拼接字符串
            foreach (var item in entityListDtos)
            {
                item.QuarterString = item.Year + "年" + item.Quarter.ToString();
            }

            return new PagedResultDto<QuestionRecordListDto>(count, entityListDtos);
        }

        /// <summary>
        /// 通过零售户Id查询零售户问卷填写记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<RetailQuestionRecordListDto>> GetPagedByRetailerId(GetQuestionRecordsInput input)
        {
            if (input.RetailerId.HasValue)
            {
                var openId = await _weChatRepository.GetAll().Where(i => i.UserId == input.RetailerId).Select(i => i.OpenId).FirstOrDefaultAsync();
                var answerRecords = _answerRepository.GetAll().Where(i => i.OpenId == openId);
                var questionRecordIds = answerRecords.Select(i => i.QuestionRecordId).Distinct().ToList();
                var query = _entityRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Title),i=>i.Title.Contains(input.Title)).Where(i => questionRecordIds.Contains(i.Id));
                var results = await (from r in query
                                     select new RetailQuestionRecordListDto
                                     {
                                         Id = r.Id,
                                         Title = r.Title,
                                         QuarterString = r.Year + "年" + r.Quarter.ToString(),
                                         CreationTime = r.CreationTime
                                     }).OrderByDescending(i => i.CreationTime).PageBy(input).ToListAsync();
                foreach (var result in results)
                {
                    result.WriteTime = await _answerRepository.GetAll().Where(a => a.QuestionRecordId == result.Id).Select(i => i.CreationTime).OrderByDescending(i => i).FirstOrDefaultAsync();
                }
                var count = results.Count();
                return new PagedResultDto<RetailQuestionRecordListDto>(count, results);
            }
            return null;
        }
        
        /// <summary>
        /// 获取零售户调查问卷填写详情头部信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RetailQuestionRecordListDto> GetRetailQuetionRecordHead(GetQuestionRecordHeadInput input)
        {
            var result = new RetailQuestionRecordListDto();
            var retailerName = await _retailerRepository.GetAsync(input.RetailerId);

            var entity = await _entityRepository.GetAsync(input.QuestionRecordId);
            if (entity == null)
            {
                return null;
            }
            result.UserName = retailerName.Name;
            result.Id = entity.Id;
            result.Title = entity.Title;
            result.QuarterString = entity.Year + "年第" + entity.Quarter + "季度";
            result.WriteTime = await _answerRepository.GetAll().Where(a => a.QuestionRecordId == result.Id).Select(i => i.CreationTime).OrderByDescending(i => i).FirstOrDefaultAsync();
            return result;
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

        public async Task<APIResultDto> CreateOrUpdate(QuestionRecordEditDto input)
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
        /// 新增QuestionRecord
        /// </summary>

        protected virtual async Task<QuestionRecordEditDto> Create(QuestionRecordEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var entity = input.MapTo<QuestionRecord>();


            entity = await _entityRepository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return entity.MapTo<QuestionRecordEditDto>();
        }

        /// <summary>
        /// 编辑QuestionRecord
        /// </summary>

        protected virtual async Task<QuestionRecord> Update(QuestionRecordEditDto input)
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
        /// 删除QuestionRecord信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<APIResultDto> Delete(EntityDto<Guid> input)
        {

            //TODO:删除前的逻辑判断，是否允许删除
            try
            {
                await _entityRepository.DeleteAsync(input.Id);
                return new APIResultDto
                {
                    Code = 0
                };
            }
            catch
            {
                return new APIResultDto { Code = 999 };
            }
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
        /// 微信获取调查问卷记录列表
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<List<QuestionRecordWXListDto>> GetQuestionRecordWXListAsync()
        {
            DateTime today = DateTime.Today;
            DateTime now = DateTime.Now;
            int curMonth = today.Month;
            int curSeason = (curMonth % 3 == 0 ? curMonth / 3 : (curMonth / 3 + 1));
            DateTime startQuarter = today.AddMonths(0 - (today.Month - 1) % 3).AddDays(1 - today.Day);  //本季度初  
            DateTime endQuarter = startQuarter.AddMonths(3).AddDays(-1);  //本季度末 
            DateTime beginTime = endQuarter.AddDays(1 - endQuarter.Day);  //当前季度月初  
            DateTime endTime = endQuarter.AddDays((86399F / 86400));

            var list = await _entityRepository.GetAll().Where(v => v.IsPublish == true).Select(v => new QuestionRecordWXListDto()
            {
                Id = v.Id,
                Year = v.Year,
                Quarter = v.Quarter
            }).OrderByDescending(v => v.Year).ThenByDescending(v => v.Quarter).ToListAsync();
            foreach (var item in list)
            {
                int count = await _answerRepository.CountAsync(v => v.QuestionRecordId == item.Id);
                item.Title = item.Year + "年" + item.Quarter.ToString() + "调查问卷";
                //item.Status = count <= 0 ? (now.Year > DateTime.Parse(item.Year + "-01-01").Year || (curSeason > (int)item.Quarter) ? "已逾期" : ((now >= beginTime && now <= endTime) ? "进行中" : "未开始")) : "查看记录";
                item.Status= "进行中（*需注释）";
            }
            return list;
        }
        
    }
}



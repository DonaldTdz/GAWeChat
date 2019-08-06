
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
                item.QuarterString = item.Year + "年第" + item.Quarter + "季度";
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
                                         QuarterString = r.Year + "年第" + r.Quarter + "季度",
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
        /// 导出QuestionRecord为excel表,等待开发。
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



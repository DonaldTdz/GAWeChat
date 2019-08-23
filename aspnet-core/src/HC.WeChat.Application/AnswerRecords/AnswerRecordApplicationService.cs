
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


using HC.WeChat.AnswerRecords;
using HC.WeChat.AnswerRecords.Dtos;
using HC.WeChat.AnswerRecords.DomainService;
using HC.WeChat.Dto;
using HC.WeChat.QuestionRecords.Dtos;
using HC.WeChat.WeChatUsers;
using HC.WeChat.Questionnaires;
using HC.WeChat.QuestionOptions;

namespace HC.WeChat.AnswerRecords
{
    /// <summary>
    /// AnswerRecord应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class AnswerRecordAppService : WeChatAppServiceBase, IAnswerRecordAppService
    {
        private readonly IRepository<AnswerRecord, Guid> _entityRepository;
        private readonly IRepository<Questionnaire, Guid> _questionnaireRepository;
        private readonly IRepository<WeChatUser, Guid> _weChatRepository;
        private readonly IRepository<QuestionOption, Guid> _questionOptionRepository;
        private readonly IAnswerRecordManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public AnswerRecordAppService(
        IRepository<AnswerRecord, Guid> entityRepository
        , IRepository<WeChatUser, Guid> weChatRepository
        , IRepository<Questionnaire, Guid> questionnaireRepository
        , IRepository<QuestionOption, Guid> questionOptionRepository
        , IAnswerRecordManager entityManager
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
            _weChatRepository = weChatRepository;
            _questionnaireRepository = questionnaireRepository;
            _questionOptionRepository = questionOptionRepository;
        }


        /// <summary>
        /// 获取AnswerRecord的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<AnswerRecordListDto>> GetPaged(GetAnswerRecordsInput input)
        {

            var query = _entityRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件


            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<AnswerRecordListDto>>(entityList);
            var entityListDtos = entityList.MapTo<List<AnswerRecordListDto>>();

            return new PagedResultDto<AnswerRecordListDto>(count, entityListDtos);
        }

        /// <summary>
        /// 获取零售户调查问卷答题详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<RetailQuestionRecordDetailDto>> GetAnswerRecordsByRetailerId(GetRetailAnswerRecordsInput input)
        {
            var openId = await _weChatRepository.GetAll().Where(i => i.UserId == input.RetailerId).Select(i => i.OpenId).FirstOrDefaultAsync();

            var query = await (from qn in _questionnaireRepository.GetAll()
                               join a in _entityRepository.GetAll()
                               on qn.Id equals a.QuestionnaireId
                               where a.OpenId == openId
                               select new RetailQuestionRecordDetailDto
                               {
                                   No = qn.No,
                                   QuestionnaireId = a.QuestionnaireId,
                                   Question = qn.Question,
                                   Remark = a.Remark,
                                   Values = a.Values
                               }).AsNoTracking().ToListAsync();
           
            foreach (var item in query)
            {
                var options = item.Values.Split(',');
                item.Desc = await _questionOptionRepository.GetAll().Where(o => options.Contains(o.Value) && o.QuestionnaireId == item.QuestionnaireId).Select(o => o.Desc).ToArrayAsync();
            }
            return query;
        }


        /// <summary>
        /// 通过指定id获取AnswerRecordListDto信息
        /// </summary>

        public async Task<AnswerRecordListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<AnswerRecordListDto>();
        }

        /// <summary>
        /// 获取编辑 AnswerRecord
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetAnswerRecordForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetAnswerRecordForEditOutput();
            AnswerRecordEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<AnswerRecordEditDto>();

                //answerRecordEditDto = ObjectMapper.Map<List<answerRecordEditDto>>(entity);
            }
            else
            {
                editDto = new AnswerRecordEditDto();
            }

            output.AnswerRecord = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改AnswerRecord的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task CreateOrUpdate(CreateOrUpdateAnswerRecordInput input)
        {

            if (input.AnswerRecord.Id.HasValue)
            {
                await Update(input.AnswerRecord);
            }
            else
            {
                await Create(input.AnswerRecord);
            }
        }


        /// <summary>
        /// 新增AnswerRecord
        /// </summary>

        protected virtual async Task<AnswerRecordEditDto> Create(AnswerRecordEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <AnswerRecord>(input);
            var entity = input.MapTo<AnswerRecord>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<AnswerRecordEditDto>();
        }

        /// <summary>
        /// 编辑AnswerRecord
        /// </summary>

        protected virtual async Task Update(AnswerRecordEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除AnswerRecord信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task Delete(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除AnswerRecord的方法
        /// </summary>

        public async Task BatchDelete(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        /// <summary>
        /// 导出AnswerRecord为excel表,等待开发。
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetToExcel()
        //{
        //	var users = await UserManager.Users.ToListAsync();
        //	var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
        //	await FillRoleNames(userListDtos);
        //	return _userListExcelExporter.ExportToFile(userListDtos);
        //}

        /// <summary>
        /// 通过月份获取零售户调查问卷信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[AbpAllowAnonymous]
        //public async Task<List<AnswerRecordWXListDto>> WXGetAnswerRecordList(GetAnswerRecordsInput input)
        //{
        //    DateTime beginTime;
        //    DateTime endTime;
        //    var nowDate = DateTime.Now;
        //    switch (input.Quarter)
        //    {
        //        case 1:
        //            beginTime = new DateTime(nowDate.Year,1,1,0,0,0);
        //            endTime = new DateTime(nowDate.Year, 3, 31, 23, 59, 59);
        //            break;
        //        case 2:
        //            beginTime = new DateTime(nowDate.Year, 4, 1, 0, 0, 0);
        //            endTime = new DateTime(nowDate.Year, 6, 30, 23, 59, 59);
        //            break;
        //        case 3:
        //            beginTime = new DateTime(nowDate.Year, 7, 1, 0, 0, 0);
        //            endTime = new DateTime(nowDate.Year, 9, 30, 23, 59, 59);
        //            break;
        //        case 4:
        //            beginTime = new DateTime(nowDate.Year, 10, 1, 0, 0, 0);
        //            endTime = new DateTime(nowDate.Year, 12, 30, 23, 59, 59);
        //            break;
        //        default:
        //            beginTime = nowDate;
        //            endTime = nowDate;
        //            break;
        //    }
        //    var query = await _entityRepository.GetAll()
        //        .Where(a => a.OpenId == input.OpenId && a.CreationTime > beginTime && a.CreationTime < endTime)
        //        .AsNoTracking()
        //        .ToListAsync();

        //    // TODO:待实现
        //    return query.MapTo<List<AnswerRecordWXListDto>>();
        //}

        /// <summary>
        /// 微信端根据openId获取问卷填写情况
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        //[AbpAllowAnonymous]
        //public async Task<List<QuestionnaireFillRecordsDto>> WXGetQuestionnaireFillRecords(string openId)
        //{
        //    var result = new List<QuestionnaireFillRecordsDto>();
        //    var nowDate = DateTime.Now;
        //    if (nowDate.Month >= 3)
        //    {
        //        var entity = await _entityRepository.GetAll().Where(i => i.OpenId == openId && i.CreationTime < new DateTime(nowDate.Year, 3, 31, 23, 59, 59)).CountAsync();
        //        if (nowDate.Month==3 && entity<=0)
        //        {
        //            result.Add(new QuestionnaireFillRecordsDto
        //            {
        //                Desc = "第一季度调查问卷",
        //                Quarter = 1,
        //                Status = "未填写"
        //            });
        //        }
        //        else if (nowDate.Month>3 && entity<=0)
        //        {
        //            result.Add(new QuestionnaireFillRecordsDto
        //            {
        //                Desc = "第一季度调查问卷",
        //                Quarter = 1,
        //                Status = "已逾期"
        //            });
        //        }
        //        else
        //        {
        //            result.Add(new QuestionnaireFillRecordsDto
        //            {
        //                Desc = "第一季度调查问卷",
        //                Quarter = 1,
        //                Status = "已完成"
        //            });
        //        }
        //    }
        //    if (nowDate.Month >= 6)
        //    {
        //        var entity = await _entityRepository.GetAll().Where(i => i.OpenId == openId && i.CreationTime < new DateTime(nowDate.Year, 6, 30, 23, 59, 59)).CountAsync();
        //        if (nowDate.Month == 6 && entity <= 0)
        //        {
        //            result.Add(new QuestionnaireFillRecordsDto
        //            {
        //                Desc = "第二季度调查问卷",
        //                Quarter = 2,
        //                Status = "未填写"
        //            });
        //        }
        //        else if (nowDate.Month == 6 && entity <= 0)
        //        {
        //            result.Add(new QuestionnaireFillRecordsDto
        //            {
        //                Desc = "第二季度调查问卷",
        //                Quarter = 2,
        //                Status = "已逾期"
        //            });
        //        }
        //        else
        //        {
        //            result.Add(new QuestionnaireFillRecordsDto
        //            {
        //                Desc = "第二季度调查问卷",
        //                Quarter = 2,
        //                Status = "已完成"
        //            });
        //        }
        //    }
        //    if (nowDate.Month >= 9)
        //    {
        //        var entity = await _entityRepository.GetAll().Where(i => i.OpenId == openId && i.CreationTime < new DateTime(nowDate.Year, 9, 30, 23, 59, 59)).CountAsync();
        //        if (nowDate.Month == 9 && entity <= 0)
        //        {
        //            result.Add(new QuestionnaireFillRecordsDto
        //            {
        //                Desc = "第三季度调查问卷",
        //                Quarter = 3,
        //                Status = "未填写"
        //            });
        //        }
        //        else if (nowDate.Month == 9 && entity <= 0)
        //        {
        //            result.Add(new QuestionnaireFillRecordsDto
        //            {
        //                Desc = "第三季度调查问卷",
        //                Quarter = 3,
        //                Status = "已逾期"
        //            });
        //        }
        //        else
        //        {
        //            result.Add(new QuestionnaireFillRecordsDto
        //            {
        //                Desc = "第三季度调查问卷",
        //                Quarter = 3,
        //                Status = "已完成"
        //            });
        //        }
        //    }
        //    if (nowDate.Month == 12)
        //    {
        //        var entity = await _entityRepository.GetAll().Where(i => i.OpenId == openId && i.CreationTime < new DateTime(nowDate.Year, 12, 30, 23, 59, 59)).CountAsync();
        //        if ( entity <= 0)
        //        {
        //            result.Add(new QuestionnaireFillRecordsDto
        //            {
        //                Desc = "第四季度调查问卷",
        //                Quarter = 4,
        //                Status = "未填写"
        //            });
        //        }
        //        else
        //        {
        //            result.Add(new QuestionnaireFillRecordsDto
        //            {
        //                Desc = "第四季度调查问卷",
        //                Quarter = 4,
        //                Status = "已完成"
        //            });
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        /// 微信端保存答题记录
        /// </summary>
        /// <param name="answerRecords"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<APIResultDto> CreateWXAnswerRecordsAsync(CreateWXAnswerDto input)
        {
            try
            {
                foreach (var item in input.List)
                {
                    AnswerRecord entity = new AnswerRecord();
                    entity.OpenId = input.OpenId;
                    entity.QuestionRecordId = input.QuestionRecordId;
                    entity.Values = item.Values;
                    entity.QuestionnaireId = item.QuestionnaireId;
                    entity.Remark = item.Remark;
                    await _entityRepository.InsertAsync(entity);
                }
                return new APIResultDto() { Code = 0, Msg = "保存成功" };
            }
            catch (Exception)
            {
                return new APIResultDto() { Code = 901, Msg = "保存失败，请重试" };

            }
        }


        /// <summary>
        /// 判断用户是否填写需求预测
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<bool> GetIsFillInQustionAsync(CreateWXAnswerDto input)
        {
            int num = await _entityRepository.CountAsync(v => v.QuestionRecordId == input.QuestionRecordId && v.OpenId == input.OpenId);
            if (num != 0)
            {
                return true;
            }
            return false;
        }
    }
}



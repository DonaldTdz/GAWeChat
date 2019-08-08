
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


using HC.WeChat.Questionnaires;
using HC.WeChat.Questionnaires.Dtos;
using HC.WeChat.Questionnaires.DomainService;
using HC.WeChat.Authorization;
using HC.WeChat.Dto;
using HC.WeChat.QuestionOptions;
using HC.WeChat.AnswerRecords;
using Senparc.Weixin.EntityUtility;

namespace HC.WeChat.Questionnaires
{
    /// <summary>
    /// Questionnaire应用层服务的接口实现方法  
    ///</summary>
    //[AbpAuthorize]

    [AbpAuthorize(AppPermissions.Pages)]
    public class QuestionnaireAppService : WeChatAppServiceBase, IQuestionnaireAppService
    {
        private readonly IRepository<Questionnaire, Guid> _entityRepository;
        private readonly IRepository<QuestionOption, Guid> _optionRepository;
        private readonly IRepository<AnswerRecord, Guid> _answerRecordRepository;

        private readonly IQuestionnaireManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public QuestionnaireAppService(
        IRepository<Questionnaire, Guid> entityRepository
        , IRepository<AnswerRecord, Guid> answerRecordRepository
        , IQuestionnaireManager entityManager
        , IRepository<QuestionOption, Guid> optionRepository
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
            _optionRepository = optionRepository;
            _answerRecordRepository = answerRecordRepository;
        }


        /// <summary>
        /// 获取Questionnaire的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<QuestionnaireListDto>> GetPaged(GetQuestionnairesInput input)
        {

            var query = _entityRepository.GetAll().WhereIf(input.type.HasValue, q => q.Type == input.type.Value);
            // TODO:根据传入的参数添加过滤条件


            var count = await query.CountAsync();

            var entityList = await (from q in query
                                    orderby input.Sorting
                                    select new QuestionnaireListDto
                                    {
                                        Id = q.Id,
                                        No = q.No,
                                        Type = q.Type,
                                        IsMultiple = q.IsMultiple,
                                        Question = q.Question,
                                        TypeName = q.Type.ToString(),
                                        CreationTime = q.CreationTime,
                                        CreatorUserId = q.CreatorUserId,
                                        DeleterUserId = q.DeleterUserId,
                                        DeletionTime = q.DeletionTime,
                                        IsDeleted = q.IsDeleted,
                                        LastModificationTime = q.LastModificationTime,
                                        LastModifierUserId = q.LastModifierUserId,
                                        Index = float.Parse(q.No.Replace("Q", ""))
                                    }).OrderBy(i => i.Index).PageBy(input).ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<QuestionnaireListDto>>(entityList);
            //var entityListDtos = entityList.MapTo<List<QuestionnaireListDto>>();

            return new PagedResultDto<QuestionnaireListDto>(count, entityList);
        }


        /// <summary>
        /// 通过指定id获取QuestionnaireListDto信息
        /// </summary>

        public async Task<QuestionnaireListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<QuestionnaireListDto>();
        }

        /// <summary>
        /// 获取编辑 Questionnaire
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetQuestionnaireForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetQuestionnaireForEditOutput();
            QuestionnaireEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<QuestionnaireEditDto>();

                //questionnaireEditDto = ObjectMapper.Map<List<questionnaireEditDto>>(entity);
            }
            else
            {
                editDto = new QuestionnaireEditDto();
            }

            output.Questionnaire = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Questionnaire的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task CreateOrUpdate(CreateOrUpdateQuestionnaireInput input)
        {

            if (input.Questionnaire.Id.HasValue)
            {
                await Update(input.Questionnaire);
            }
            else
            {
                await Create(input.Questionnaire);
            }
        }


        /// <summary>
        /// 新增Questionnaire
        /// </summary>

        protected virtual async Task<QuestionnaireEditDto> Create(QuestionnaireEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <Questionnaire>(input);
            var entity = input.MapTo<Questionnaire>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<QuestionnaireEditDto>();
        }

        /// <summary>
        /// 编辑Questionnaire
        /// </summary>

        protected virtual async Task Update(QuestionnaireEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除Questionnaire信息的方法
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
                return new APIResultDto
                {
                    Code = 999,
                    Msg = "删除异常,请重试."
                };
            }
        }



        /// <summary>
        /// 批量删除Questionnaire的方法
        /// </summary>

        public async Task BatchDelete(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        /// <summary>
        /// 导出Questionnaire为excel表,等待开发。
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
        /// 添加或者修改Questionnaire的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<APIResultDto> CreateOrUpdateQuestionnaire(QuestionnaireEditDto input)
        {
            if (await ValidNoExist(input))
            {
                return new APIResultDto
                {
                    Code = 999,
                    Msg = "问题编号已存在"
                };
            }
            Questionnaire entity = new Questionnaire();
            if (input.Id.HasValue)
            {
                entity = await _entityRepository.GetAsync(input.Id.Value);
            }
            input.MapTo(entity);
            entity = await _entityRepository.InsertOrUpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return new APIResultDto
            {
                Code = 0,
                Data = entity
            };
        }

        /// <summary>
        /// 验证编号是否重复
        /// </summary>
        /// <returns></returns>
        private async Task<bool> ValidNoExist(QuestionnaireEditDto input)
        {
            int count = await _entityRepository.GetAll().Where(i => i.No == input.No).CountAsync();
            if (input.Id.HasValue)
            {
                count = await _entityRepository.GetAll().Where(i => i.No == input.No && i.Id != input.Id.Value).CountAsync();
                //if (count > 1)
                //{
                //    return true;
                //}
                //return false;
            }
            if (count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取微信端调查问卷集合
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<List<WXQuestionnaireListDto>> GetWXQuestionnaireList()
        {
            var list = await (from q in _entityRepository.GetAll()
                              select new WXQuestionnaireListDto
                              {
                                  No = q.No,
                                  IsMultiple = q.IsMultiple,
                                  Type = q.Type,
                                  Question = q.Question,
                                  Id = q.Id,
                                  Index = float.Parse(q.No.Replace("Q", ""))
                              })
                        .OrderBy(i => i.Index)
                        .ToListAsync();
            foreach (var item in list)
            {
                item.QuestionOptions = await GetOptions(item.Id);
            }
            return list;
        }

        /// <summary>
        /// 通过问题主键获取答案配置选项
        /// </summary>
        /// <param name="questionnaireId"></param>
        /// <returns></returns>
        public async Task<List<QuestionOption>> GetOptions(Guid questionnaireId)
        {
            var entitys = await _optionRepository.GetAll()
                .Where(i => i.QuestionnaireId == questionnaireId)
                .OrderBy(i => i.Value)
                .ToListAsync();
            return entitys;
        }

        /// <summary>
        /// 获取当前用户问卷调查填写记录
        /// </summary>
        /// <param name="questionnaireId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<List<WXQuestionnaireListDto>> GetQuestionRecordWXByIdAsync(GetWXQuestionRecordInput input)
        {
            var record = _answerRecordRepository.GetAll().Where(v => v.OpenId == input.OpenId && v.QuestionRecordId == input.QuestionRecordId);
            var question = _entityRepository.GetAll();
            var query = await (from r in record
                               join q in question on r.QuestionnaireId equals q.Id
                               select new WXQuestionnaireListDto()
                               {
                                   Id = q.Id,
                                   Question = q.Question,
                                   No = q.No,
                                   Value = r.Values,
                                   RecordId = r.Id,
                                   Remark = r.Remark,
                                   Index = float.Parse(q.No.Replace("Q", ""))
                               }).OrderBy(v =>v.Index).ToListAsync();
            foreach (var item in query)
            {
                if (item.Value.Contains(','))
                {
                    string[] values = item.Value.Split(',');
                    foreach (var value in values)
                    {
                        var op = await _optionRepository.GetAll().Where(v => v.QuestionnaireId == item.Id && v.Value == value).Select(v => new QuestionOption()
                        {
                            Desc = v.Desc,
                            Value = v.Value
                        }).FirstOrDefaultAsync();
                        item.QuestionOptions.Add(op);
                    }
                }
                else
                {
                    var op = await _optionRepository.GetAll().Where(v => v.QuestionnaireId == item.Id && v.Value == item.Value).Select(v => new QuestionOption()
                    {
                        Desc = v.Desc,
                        Value = v.Value
                    }).FirstOrDefaultAsync();
                    item.QuestionOptions.Add(op);
                }
            }
            return query;
        }
    }
}



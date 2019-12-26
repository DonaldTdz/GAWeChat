
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


using HC.WeChat.LuckySigns;
using HC.WeChat.LuckySigns.Dtos;
using HC.WeChat.LuckySigns.DomainService;
using HC.WeChat.Prizes;
using HC.WeChat.WeChatUsers;
using HC.WeChat.Dto;

namespace HC.WeChat.LuckySigns
{
    /// <summary>
    /// LuckySign应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class LuckySignAppService : WeChatAppServiceBase, ILuckySignAppService
    {
        private readonly IRepository<LuckySign, Guid> _entityRepository;
        private readonly IRepository<WeChatUser, Guid> _wechatuserRepository;
        private readonly ILuckySignManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public LuckySignAppService(
        IRepository<LuckySign, Guid> entityRepository
        , ILuckySignManager entityManager
        , IRepository<WeChatUser, Guid> wechatuserRepository
        )
        {
            _entityRepository = entityRepository;
            _wechatuserRepository = wechatuserRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取LuckySign的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<LuckySignListDto>> GetPaged(GetLuckySignsInput input)
        {

            var query = _entityRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件


            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<LuckySignListDto>>(entityList);
            var entityListDtos = entityList.MapTo<List<LuckySignListDto>>();

            return new PagedResultDto<LuckySignListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取LuckySignListDto信息
        /// </summary>

        public async Task<LuckySignListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<LuckySignListDto>();
        }

        /// <summary>
        /// 获取编辑 LuckySign
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetLuckySignForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetLuckySignForEditOutput();
            LuckySignEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<LuckySignEditDto>();

                //luckySignEditDto = ObjectMapper.Map<List<luckySignEditDto>>(entity);
            }
            else
            {
                editDto = new LuckySignEditDto();
            }

            output.LuckySign = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改LuckySign的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task CreateOrUpdate(CreateOrUpdateLuckySignInput input)
        {

            if (input.LuckySign.Id.HasValue)
            {
                await Update(input.LuckySign);
            }
            else
            {
                await Create(input.LuckySign);
            }
        }


        /// <summary>
        /// 新增LuckySign
        /// </summary>

        protected virtual async Task<LuckySignEditDto> Create(LuckySignEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <LuckySign>(input);
            var entity = input.MapTo<LuckySign>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<LuckySignEditDto>();
        }

        /// <summary>
        /// 编辑LuckySign
        /// </summary>

        protected virtual async Task Update(LuckySignEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除LuckySign信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task Delete(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除LuckySign的方法
        /// </summary>

        public async Task BatchDelete(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 抽奖活动签到
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public async Task<APIResultDto> LuckySignByIdAsync(string openId)
        {
            if (string.IsNullOrEmpty(openId))
            {
                return new APIResultDto()
                {
                    Code = 401,
                    Msg = "未获取到当前用户信息，请重新进入公众号"
                };
            }

            var user = await _wechatuserRepository.FirstOrDefaultAsync(v => v.OpenId == openId);
            if (user == null)
            {
                return new APIResultDto()
                {
                    Code = 403,
                    Msg = "未获取到当前用户信息，请重新关注公众号"
                };
            }

            else if (user.UserType != WechatEnums.UserTypeEnum.内部员工)
            {
                return new APIResultDto()
                {
                    Code = 901,
                    Msg = "非内部员工，请前往绑定！"
                };
            }

            else if (!user.UserId.HasValue)
            {
                return new APIResultDto()
                {
                    Code = 902,
                    Msg = "内部员工信息获取异常，请重新绑定！"
                };
            }

            LuckySign entity = new LuckySign();
            entity.UserId = user.UserId.Value;
            await _entityRepository.InsertAsync(entity);
            return new APIResultDto()
            {
                Code = 0,
                Msg = "签到成功！"
            };
        }
    }
}
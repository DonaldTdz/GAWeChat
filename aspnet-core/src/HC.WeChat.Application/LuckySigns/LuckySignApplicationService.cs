
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
using HC.WeChat.Employees;
using Abp.Auditing;
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
        private readonly IRepository<Employee, Guid> _employeeRepository;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public LuckySignAppService(
        IRepository<LuckySign, Guid> entityRepository
        , ILuckySignManager entityManager
        , IRepository<WeChatUser, Guid> wechatuserRepository
        , IRepository<Employee, Guid> employeeRepository
        )
        {
            _entityRepository = entityRepository;
            _wechatuserRepository = wechatuserRepository;
            _entityManager = entityManager;
            _employeeRepository = employeeRepository;
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


        /// 通过openId获取个人抽奖状态  --抽奖
        /// 通过openId获取个人签到状态 
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<APIResultDto> GetLuckySignInfoAsync(string openId) {

            GetLuckySignInfoDto SignInfo = new GetLuckySignInfoDto();
            if (openId != null)
            {
                var userId = await _wechatuserRepository.GetAll().Where(v => v.OpenId == openId).Select(v => v.UserId).FirstOrDefaultAsync();
                if (userId != null)
                {
                    var isExsit = await _entityRepository.GetAll().Where(v => v.UserId == userId&&v.CreationTime.ToString("yyyyMMdd")==DateTime.Today.ToString("yyyyMMdd")).AnyAsync();

                    var employee = await _employeeRepository.GetAll().Where(v => v.Id == userId).FirstOrDefaultAsync();

                    return new APIResultDto
                    {
                        Code = 0,
                        Msg = "Success",
                        Data= new GetLuckySignInfoDto
                        {
                            Name = employee.Name,
                            Code = employee.Code,
                            LotteryState = isExsit,
                            DeptName = employee.DeptName
                        }
                    };
                }
                else 
                {
                    return new APIResultDto
                    {
                        Code = 902,
                        Msg = "未获取到个人信息，请重新关注公众号！"
                    };
                }
            }
            else 
            {
                return new APIResultDto
                {
                    Code = 901,
                    Msg = "未获取到个人信息，请重新进入公众号！"
                };
            }
        }
        /// <summary>
        /// 微信端签到
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<APIResultDto> GetCreateWXLuckyDrawAsync(string openId) 
        {
            var wechatEn = await _wechatuserRepository.GetAll().Where(v => v.OpenId == openId).FirstOrDefaultAsync();

            if (wechatEn != null)
            {
                var employee = await _employeeRepository.GetAll().Where(v => v.Id == wechatEn.UserId).FirstOrDefaultAsync();
                if (employee != null)
                {
                    await _entityRepository.InsertAsync(new LuckySign { 
                    UserId= employee.Id
                    });
                    return new APIResultDto
                    {
                        Msg = "签到成功",
                        Code = 0
                    };
                }
                else 
                {
                    return new APIResultDto
                    {
                        Code = 902,
                        Msg = "你不是内部员工!无法参与此次抽奖!请绑定工号"
                    };
                }
            }
            else 
            {
                return new APIResultDto
                {
                    Code = 901,
                    Msg = "你还未关注微信公众号!"
                };
            }

        }
        /// <summary>
        /// 获取签到人员数量和总数量
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<APIResultDto> GetSignInPeronNumAsync() 
        {
            //总人数
            var num_Total =await  _employeeRepository.CountAsync();

            var employeelist= await _employeeRepository.GetAll().Select(v=>v.Id).ToListAsync();

            //已签到人数
            var num_Signed =await _entityRepository.CountAsync(v=>employeelist.Contains(v.UserId)&&v.CreationTime.ToString("yyyyMMdd")==DateTime.Today.ToString("yyyyMMdd"));

            return new APIResultDto
            {
                Code = 0,
                Msg="获取成功!",
                Data=new SignInPeronNumDto 
                {
                    Num_Total=num_Total,
                    Num_UnSign= num_Signed
                }
            };            
        }
    }
}
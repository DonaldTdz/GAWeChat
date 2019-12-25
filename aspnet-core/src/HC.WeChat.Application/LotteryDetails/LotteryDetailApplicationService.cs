
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using HC.WeChat.Dto;
using HC.WeChat.Employees;
using HC.WeChat.LotteryDetails.DomainService;
using HC.WeChat.LotteryDetails.Dtos;
using HC.WeChat.LuckyDraws;
using HC.WeChat.LuckySigns;
using HC.WeChat.Prizes;
using HC.WeChat.WeChatUsers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace HC.WeChat.LotteryDetails
{
    /// <summary>
    /// LotteryDetail应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class LotteryDetailAppService : WeChatAppServiceBase, ILotteryDetailAppService
    {
        private readonly IRepository<LotteryDetail, Guid> _entityRepository;
        private readonly IRepository<LuckySign, Guid> _luckySignRepository;
        private readonly IRepository<Employee, Guid> _employeeRepository;
        private readonly IRepository<Prize, Guid> _prizeRepository;
        private readonly IRepository<WeChatUser, Guid> _wechatuserRepository;
        private readonly IRepository<LuckyDraw, Guid> _luckyDrawRepository;
        private readonly ILotteryDetailManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public LotteryDetailAppService(
        IRepository<LotteryDetail, Guid> entityRepository
        , IRepository<LuckySign, Guid> luckySignRepository
        , ILotteryDetailManager entityManager
        , IRepository<Employee, Guid> employeeRepository
        , IRepository<Prize, Guid> prizeRepository
        , IRepository<WeChatUser, Guid> wechatuserRepository
        , IRepository<LuckyDraw, Guid> luckyDrawRepository
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
            _luckySignRepository = luckySignRepository;
            _employeeRepository = employeeRepository;
            _prizeRepository = prizeRepository;
            _wechatuserRepository = wechatuserRepository;
            _luckyDrawRepository = luckyDrawRepository;
        }


        /// <summary>
        /// 获取LotteryDetail的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<LotteryDetailListDto>> GetPaged(GetLotteryDetailsInput input)
        {

            var query = _entityRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件


            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<LotteryDetailListDto>>(entityList);
            var entityListDtos = entityList.MapTo<List<LotteryDetailListDto>>();

            return new PagedResultDto<LotteryDetailListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取LotteryDetailListDto信息
        /// </summary>
        public async Task<LotteryDetailListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<LotteryDetailListDto>();
        }

        /// <summary>
        /// 获取编辑 LotteryDetail
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetLotteryDetailForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetLotteryDetailForEditOutput();
            LotteryDetailEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<LotteryDetailEditDto>();

                //lotteryDetailEditDto = ObjectMapper.Map<List<lotteryDetailEditDto>>(entity);
            }
            else
            {
                editDto = new LotteryDetailEditDto();
            }

            output.LotteryDetail = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改LotteryDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdate(CreateOrUpdateLotteryDetailInput input)
        {

            if (input.LotteryDetail.Id.HasValue)
            {
                await Update(input.LotteryDetail);
            }
            else
            {
                await Create(input.LotteryDetail);
            }
        }


        /// <summary>
        /// 新增LotteryDetail
        /// </summary>
        protected virtual async Task<LotteryDetailEditDto> Create(LotteryDetailEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <LotteryDetail>(input);
            var entity = input.MapTo<LotteryDetail>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<LotteryDetailEditDto>();
        }

        /// <summary>
        /// 编辑LotteryDetail
        /// </summary>
        protected virtual async Task Update(LotteryDetailEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }


        /// <summary>
        /// 删除LotteryDetail信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task Delete(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }


        /// <summary>
        /// 批量删除LotteryDetail的方法
        /// </summary>

        public async Task BatchDelete(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        /// <summary>
        /// 中奖逻辑
        /// </summary>
        /// <param name="luckyId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        private async Task<APIResultDto> GetLotteryLogicAsync(Guid luckyId)
        {
            var empList = await _luckySignRepository.GetAll().Where(v => v.CreationTime.Date == DateTime.Today).Select(v => v.UserId).ToArrayAsync();
            int empTotal = empList.Length;
            int prizeTotal = await _prizeRepository.GetAll().Where(v => v.LuckyDrawId == luckyId).SumAsync(v => v.Num);
            if (prizeTotal == 0)
            {
                return new APIResultDto()
                {
                    Code = 902,
                    Msg = "奖品总量必须大于0！"
                };
            }
            bool isFirstLottery = !await _entityRepository.GetAll().AnyAsync();
            if (isFirstLottery)
            {
                if (empTotal == 0)
                {
                    return new APIResultDto()
                    {
                        Code = 801,
                        Msg = "签到人数必须大于0！"
                    };
                }
                if (prizeTotal > empTotal)
                {
                    return new APIResultDto()
                    {
                        Code = 901,
                        Msg = "奖品总量大于签到人数，请重新分配奖品数量！"
                    };
                }

                var winEmpList = empList.OrderBy(v => Guid.NewGuid()).Take(prizeTotal).ToList();
                var prizeList = await _prizeRepository.GetAll().Where(v => v.LuckyDrawId == luckyId).Select(v => new { v.Id, v.Name }).ToListAsync();
                int i = 0;
                foreach (var item in winEmpList)
                {
                    LotteryDetail entity = new LotteryDetail();
                    entity.LuckyDrawId = luckyId;
                    entity.UserId = item;
                    entity.IsCanWin = true;
                    //entity.IsLottery = false;
                    entity.IsWin = true;
                    entity.PrizeId = prizeList[i].Id;
                    entity.PrizeName = prizeList[i].Name;
                    await _entityRepository.InsertAsync(entity);

                    var prize = await _prizeRepository.GetAsync(prizeList[i].Id);
                    prize.WinUserId = item;
                    i++;
                }
                var notWinEmpList = empList.Where(v => !winEmpList.Contains(v)).ToList();
                foreach (var item in notWinEmpList)
                {
                    LotteryDetail entity = new LotteryDetail();
                    entity.LuckyDrawId = luckyId;
                    entity.UserId = item;
                    entity.IsCanWin = true;
                    //entity.IsLottery = false;
                    //entity.IsWin = false;
                    await _entityRepository.InsertAsync(entity);
                }
            }
            else
            {
                Guid[] cantWinEmpIds = await _entityRepository.GetAll().Where(v => v.IsWin).Select(v => v.UserId).ToArrayAsync();
                //Guid[] canWinEmpList = await _luckySignRepository.GetAll().Where(v => !cantWinEmpIds.Contains(v.UserId)).Select(v=>v.UserId).ToArrayAsync();
                Guid[] canWinEmpList = empList.Where(v => !cantWinEmpIds.Contains(v)).ToArray();
                int canWinEmpTotal = canWinEmpList.Length;
                if (canWinEmpTotal == 0)
                {
                    return new APIResultDto()
                    {
                        Code = 701,
                        Msg = "可中奖的人数必须大于0！"
                    };
                }
                if (prizeTotal > canWinEmpTotal)
                {
                    return new APIResultDto()
                    {
                        Code = 903,
                        Msg = "奖品总量大于可中奖的人数，请重新分配奖品数量！"
                    };
                }
                var winEmpList = canWinEmpList.OrderBy(v => Guid.NewGuid()).Take(prizeTotal).ToList();
                var prizeList = await _prizeRepository.GetAll().Where(v => v.LuckyDrawId == luckyId).Select(v => new { v.Id, v.Name }).ToListAsync();
                int i = 0;

                //中奖人员
                foreach (var item in winEmpList)
                {
                    LotteryDetail entity = new LotteryDetail();
                    entity.LuckyDrawId = luckyId;
                    entity.UserId = item;
                    entity.IsCanWin = true;
                    //entity.IsLottery = false;
                    entity.IsWin = true;
                    entity.PrizeId = prizeList[i].Id;
                    entity.PrizeName = prizeList[i].Name;
                    await _entityRepository.InsertAsync(entity);

                    var prize = await _prizeRepository.GetAsync(prizeList[i].Id);
                    prize.WinUserId = item;
                    i++;
                }

                //无中奖权限人员
                foreach (var item in cantWinEmpIds)
                {
                    LotteryDetail entity = new LotteryDetail();
                    entity.LuckyDrawId = luckyId;
                    entity.UserId = item;
                    //entity.IsCanWin = false;
                    //entity.IsLottery = false;
                    //entity.IsWin = false;
                    await _entityRepository.InsertAsync(entity);
                }

                //未中奖人员
                var notWinEmpList = canWinEmpList.Where(v => !winEmpList.Contains(v)).ToList();
                foreach (var item in notWinEmpList)
                {
                    LotteryDetail entity = new LotteryDetail();
                    entity.LuckyDrawId = luckyId;
                    entity.UserId = item;
                    entity.IsCanWin = true;
                    //entity.IsLottery = false;
                    //entity.IsWin = false;
                    await _entityRepository.InsertAsync(entity);
                }
            }

            return new APIResultDto()
            {
                Code = 0,
                Msg = "抽奖活动发布成功"
            };
        }

        /// <summary>
        /// 当前用户抽奖
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="luckyId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<APIResultDto> GetCurLotteryDetailAsync(string openId, Guid luckyId)
        {
            if (string.IsNullOrEmpty(openId))
            {
                return new APIResultDto()
                {
                    Code = 401,
                    Msg = "未获取到当前用户信息，请重新进入公众号"
                };
            }
            DateTime curTime = DateTime.Now;
            var lottery = await _luckyDrawRepository.FirstOrDefaultAsync(v=>v.Id == luckyId && v.IsPublish == true);
            if (lottery == null)
            {
                return new APIResultDto()
                {
                    Code = 402,
                    Msg = "未获取到本轮活动信息，请重新进入公众号"
                };
            }
            if (curTime > lottery.EndTime)
            {
                return new APIResultDto()
                {
                    Code = 801,
                    Msg = "本轮抽奖活动已截止！"
                };
            }
            else if (curTime < lottery.CreationTime)
            {
                return new APIResultDto()
                {
                    Code = 802,
                    Msg = "本轮抽奖活动尚未开始！"
                };
            }
            var user = await _wechatuserRepository.FirstOrDefaultAsync(v => v.OpenId == openId);
            if (user.UserType != WechatEnums.UserTypeEnum.内部员工)
            {
                return new APIResultDto()
                {
                    Code = 901,
                    Msg = "非内部员工，请前往绑定！"
                };
            }

            bool isSign = await _luckySignRepository.GetAll().AnyAsync(v => v.UserId == user.UserId && v.CreationTime.Date == DateTime.Today);
            if (!isSign)
            {
                return new APIResultDto()
                {
                    Code = 902,
                    Msg = "请先签到！"
                };
            }

            var luckyDetail = await _entityRepository.FirstOrDefaultAsync(v => v.LuckyDrawId == luckyId && v.UserId == user.UserId);
            if (luckyDetail == null)
            {
                return new APIResultDto()
                {
                    Code = 701,
                    Msg = "很遗憾，奖品与你擦肩而过"
                };
            }
            else if(luckyDetail.IsLottery == true)
            {
                return new APIResultDto()
                {
                    Code = 704,
                    Msg = "您已抽过奖，等下一轮活动吧"
                };
            }

            luckyDetail.IsLottery = true;
            luckyDetail.LotteryTime = curTime;
            if (luckyDetail.PrizeId.HasValue)
            {
                return new APIResultDto()
                {
                    Code = 702,
                    Msg = "恭喜您中奖"
                };
            }
            return new APIResultDto()
            {
                Code = 703,
                Msg = "很遗憾，奖品与你擦肩而过"
            };
        }



        [AbpAllowAnonymous]
        public List<int> TestRadomNum()
        {
            var prizeIndex = Enumerable.Range(0, 399).OrderBy(v => Guid.NewGuid()).Take(30).ToList().OrderBy(v => v).ToList();
            return prizeIndex;
        }

        [AbpAllowAnonymous]
        public List<int> TestRadom2Num()
        {
            var num = Enumerable.Range(0, 399).Select(x => new { v = x, k = Guid.NewGuid().ToString() }).ToList().OrderBy(x => x.k).Select(x => x.v).Take(30).ToList();
            return num;
        }

        [AbpAllowAnonymous]
        public async Task TestAddSign()
        {
            for (int i = 1; i <= 400; i++)
            {
                var sign = new LuckySign();
                sign.UserId = Guid.NewGuid();
                await _luckySignRepository.InsertAsync(sign);
            }
        }
    }
}
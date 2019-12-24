
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


using HC.WeChat.LotteryDetails;
using HC.WeChat.LotteryDetails.Dtos;
using HC.WeChat.LotteryDetails.DomainService;
using HC.WeChat.Dto;
using HC.WeChat.LuckySigns;
using HC.WeChat.Employees;
using HC.WeChat.Prizes;

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
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
            _luckySignRepository = luckySignRepository;
            _employeeRepository = employeeRepository;
            _prizeRepository = prizeRepository;
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
        public async Task<APIResultDto> CreateLotteryDetailsAsync(Guid luckyId)
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
                foreach (var item in winEmpList)
                {
                    int i = 0;
                    LotteryDetail entity = new LotteryDetail();
                    entity.LuckyDrawId = luckyId;
                    entity.UserId = item;
                    entity.IsCanWin = true;
                    //entity.IsLottery = false;
                    entity.IsWin = true;
                    entity.PrizeId = prizeList[0].Id;
                    entity.PrizeName = prizeList[0].Name;
                    await _entityRepository.InsertAsync(entity);
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
                //var canWinEmpTotal = await _entityRepository.GetAll().CountAsync(v => empList.Contains(v.UserId) && !v.IsWin);
                //var canWinEmpList = await _entityRepository.GetAll().where(v => empList.Contains(v.UserId) && v.IsWin);
                //if (canWinEmpTotal == 0)
                //{
                //    return new APIResultDto()
                //    {
                //        Code = 701,
                //        Msg = "可中奖的人数必须大于0！"
                //    };
                //}
                //if (prizeTotal > canWinEmpTotal)
                //{
                //    return new APIResultDto()
                //    {
                //        Code = 903,
                //        Msg = "奖品总量大于可中奖的人数，请重新分配奖品数量！"
                //    };
                //}
                //foreach (var item in empList)
                //{
                //    var winIndex = Enumerable.Range(0, empList - 1).OrderBy(v => Guid.NewGuid()).Take(prizeTotal).ToList().OrderBy(v => v).ToList();
                //    bool isWin = _entityRepository.GetAll().Where(v => v.UserId == item && v.IsWin == true).Any();
                //    LotteryDetail entity = new LotteryDetail();
                //    if (!isWin)
                //    {
                //        entity.IsCanWin = true;
                //    }
                //    entity.LuckyDrawId = luckyId;
                //    entity.IsLottery = false;
                //}
            }

            return new APIResultDto()
            {
                Code = 0,
                Msg = "抽奖活动发布成功"
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
            var num = Enumerable.Range(0, 399).Select(x => new { v = x, k = Guid.NewGuid().ToString() }).ToList().OrderBy(x => x.k).Select(x => x.v).Take(10).ToList();
            return num;
        }
    }
}
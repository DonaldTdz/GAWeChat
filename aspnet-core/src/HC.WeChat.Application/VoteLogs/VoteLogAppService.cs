using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using System.Linq;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;

using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using HC.WeChat.VoteLogs.Authorization;
using HC.WeChat.VoteLogs.DomainServices;
using HC.WeChat.VoteLogs.Dtos;
using HC.WeChat.VoteLogs;
using System;
using HC.WeChat.Authorization;
using HC.WeChat.Exhibitions;
using HC.WeChat.Dto;
using HC.WeChat.WeChatUsers;
using HC.WeChat.ExhibitionShops;

namespace HC.WeChat.VoteLogs
{
    /// <summary>
    /// VoteLog应用层服务的接口实现方法
    /// </summary>
    //[AbpAuthorize(VoteLogAppPermissions.VoteLog)]
    [AbpAuthorize(AppPermissions.Pages)]
    public class VoteLogAppService : WeChatAppServiceBase, IVoteLogAppService
    {
        private readonly IRepository<VoteLog, Guid> _votelogRepository;
        private readonly IVoteLogManager _votelogManager;
        private readonly IRepository<Exhibition, Guid> _exhibitionRepository;
        private readonly IRepository<WeChatUser, Guid> _wechatuserRepository;
        private readonly IRepository<ExhibitionShop, Guid> _exhibitionshopRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public VoteLogAppService(
            IRepository<VoteLog, Guid> votelogRepository
      , IVoteLogManager votelogManager
            , IRepository<Exhibition, Guid> exhibitionRepository
            , IRepository<WeChatUser, Guid> wechatuserRepository
            , IRepository<ExhibitionShop, Guid> exhibitionshopRepository

        )
        {
            _votelogRepository = votelogRepository;
            _votelogManager = votelogManager;
            _exhibitionRepository = exhibitionRepository;
            _wechatuserRepository = wechatuserRepository;
            _exhibitionshopRepository = exhibitionshopRepository;
        }


        /// <summary>
        /// 获取VoteLog的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<VoteLogListDto>> GetPagedVoteLogs(GetVoteLogsInput input)
        {

            var query = _votelogRepository.GetAll();
            //TODO:根据传入的参数添加过滤条件

            var votelogCount = await query.CountAsync();

            var votelogs = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            //var votelogListDtos = ObjectMapper.Map<List <VoteLogListDto>>(votelogs);
            var votelogListDtos = votelogs.MapTo<List<VoteLogListDto>>();

            return new PagedResultDto<VoteLogListDto>(
                votelogCount,
                votelogListDtos
                );
        }

        /// <summary>
        /// 通过指定id获取VoteLogListDto信息
        /// </summary>
        public async Task<VoteLogListDto> GetVoteLogByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _votelogRepository.GetAsync(input.Id);

            return entity.MapTo<VoteLogListDto>();
        }






        /// <summary>
        /// 导出VoteLog为excel表
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetVoteLogsToExcel(){

        //var users = await UserManager.Users.ToListAsync();
        //var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
        //await FillRoleNames(userListDtos);

        //return _userListExcelExporter.ExportToFile(userListDtos);

        //}

        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetVoteLogForEditOutput> GetVoteLogForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetVoteLogForEditOutput();
            VoteLogEditDto votelogEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _votelogRepository.GetAsync(input.Id.Value);

                votelogEditDto = entity.MapTo<VoteLogEditDto>();

                //votelogEditDto = ObjectMapper.Map<List <votelogEditDto>>(entity);


            }
            else
            {
                votelogEditDto = new VoteLogEditDto();
            }

            output.VoteLog = votelogEditDto;
            return output;

        }


        /// <summary>
        /// 添加或者修改VoteLog的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateVoteLog(CreateOrUpdateVoteLogInput input)
        {

            if (input.VoteLog.Id.HasValue)
            {
                await UpdateVoteLogAsync(input.VoteLog);
            }
            else
            {
                await CreateVoteLogAsync(input.VoteLog);
            }
        }

        /// <summary>
        /// 新增VoteLog
        /// </summary>
        //[AbpAuthorize(VoteLogAppPermissions.VoteLog_CreateVoteLog)]
        protected virtual async Task<VoteLogEditDto> CreateVoteLogAsync(VoteLogEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<VoteLog>(input);

            entity = await _votelogRepository.InsertAsync(entity);
            return entity.MapTo<VoteLogEditDto>();
        }

        /// <summary>
        /// 编辑VoteLog
        /// </summary>
        [AbpAuthorize(VoteLogAppPermissions.VoteLog_EditVoteLog)]
        protected virtual async Task UpdateVoteLogAsync(VoteLogEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _votelogRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _votelogRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除VoteLog信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(VoteLogAppPermissions.VoteLog_DeleteVoteLog)]
        public async Task DeleteVoteLog(EntityDto<Guid> input)
        {

            //TODO:删除前的逻辑判断，是否允许删除
            await _votelogRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除VoteLog的方法
        /// </summary>
        [AbpAuthorize(VoteLogAppPermissions.VoteLog_BatchDeleteVoteLogs)]
        public async Task BatchDeleteVoteLogsAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _votelogRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 获取投票数
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<int> GetWXVotesCountAsync()
        {
                int total = await _votelogRepository.GetAll().CountAsync();
                return total;
        }

        /// <summary>
        /// 写入投票详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<APIResultDto> AddVoteLogAsync(VoteLogEditDto input)
        {
            Exhibition config = await _exhibitionRepository.GetAll().FirstOrDefaultAsync();
            if (DateTime.Now.Date>=config.BeginTime.Value.Date && DateTime.Now.Date<=config.EndTime.Value.Date)
            {
                var nickName = await _wechatuserRepository.GetAll().Where(v => v.OpenId == input.OpenId).Select(v => v.NickName).FirstOrDefaultAsync();
                input.CreateTime = DateTime.Now;
                input.UserName = nickName;
                var result = input.MapTo<VoteLog>();
                //var votecount = await _votelogRepository.GetAll().Where(v=>v.OpenId==input.OpenId&&config.BeginTime)
                int? votecount = await _votelogRepository.GetAll().Where(v => v.OpenId == input.OpenId && v.CreateTime.Date == DateTime.Now.Date).CountAsync();
                if (votecount < config.Frequency)
                {
                    await _votelogRepository.InsertAsync(result);
                    var entity = await _exhibitionshopRepository.GetAll().Where(v => v.Id == input.ExhibitionId).FirstOrDefaultAsync();
                    if (entity.Votes == null)
                    {
                        entity.Votes = 0;
                    }
                    entity.Votes++;
                    var exhibitionShopUpdate = await _exhibitionshopRepository.UpdateAsync(entity);
                }
                return new APIResultDto() { Code = 0, Msg = "成功" };
            }
            else if(DateTime.Now.Date< config.BeginTime.Value.Date)
            {
                return new APIResultDto() { Code = 888, Msg = "活动尚未开始" };
            }
            else
            {
                return new APIResultDto() { Code = 999, Msg = "活动已过期" };
            }
            
        }

        /// <summary>
        /// 获取此人当天投票次数
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<int?> GetCurrentDayVoteByIdAsync(string openId)
        {
            Exhibition config = await _exhibitionRepository.GetAll().FirstOrDefaultAsync();
            if (DateTime.Now.Date >= config.BeginTime.Value.Date && DateTime.Now.Date <= config.EndTime.Value.Date)
            {
                int? votecount = await _votelogRepository.GetAll().Where(v => v.OpenId == openId
            && v.CreateTime.Date == DateTime.Now.Date).CountAsync();
            return votecount;
            }
            else
            {
                return null;
            }
        }
    }
}



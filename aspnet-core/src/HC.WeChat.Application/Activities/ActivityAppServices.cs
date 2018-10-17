using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Linq;

using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using HC.WeChat.Activities.Authorization;
using HC.WeChat.Activities.Dtos;
using HC.WeChat.Activities.DomainServices;
using HC.WeChat.Activities;
using System;
using HC.WeChat.Authorization;
using HC.WeChat.ActivityGoodses;
using HC.WeChat.WechatEnums;
using Abp.Auditing;

namespace HC.WeChat.Activities
{
    /// <summary>
    /// Activity应用层服务的接口实现方法
    /// </summary>
    //[AbpAuthorize(ActivityAppPermissions.Activity)]
    [AbpAuthorize(AppPermissions.Pages)]
    public class ActivityAppService : WeChatAppServiceBase, IActivityAppService
    {
        private readonly IRepository<Activity, Guid> _activityRepository;
        private readonly IActivityManager _activityManager;

        private readonly IRepository<ActivityGoods, Guid> _activitygoodsRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ActivityAppService(IRepository<Activity, Guid> activityRepository
      , IActivityManager activityManager, IRepository<ActivityGoods, Guid> activitygoodsRepository
        )
        {
            _activityRepository = activityRepository;
            _activityManager = activityManager;
            _activitygoodsRepository = activitygoodsRepository;
            
        }

        /// <summary>
        /// 获取Activity的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ActivityListDto>> GetPagedActivities(GetActivitysInput input)
        {

            var query = _activityRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(input.Name), a => a.Name.Contains(input.Name))
                .WhereIf(input.Status.HasValue, a => a.Status == input.Status)
                .WhereIf(input.Type.HasValue, a => a.ActivityType == input.Type)
                .WhereIf(input.StartTime.HasValue, a => a.BeginTime >= input.StartTime)
                .WhereIf(input.EndTime.HasValue, a => a.EndTime <= input.EndTimeAddOne);
            //TODO:根据传入的参数添加过滤条件
            var activityCount = await query.CountAsync();

            var activitys = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            //var activityListDtos = ObjectMapper.Map<List <ActivityListDto>>(activitys);
            var activityListDtos = activitys.MapTo<List<ActivityListDto>>();

            return new PagedResultDto<ActivityListDto>(
                activityCount,
                activityListDtos
                );

        }

        /// <summary>
        /// 通过指定id获取ActivityListDto信息
        /// </summary>
        public async Task<ActivityListDto> GetActivityByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _activityRepository.GetAsync(input.Id);

            return entity.MapTo<ActivityListDto>();
        }

        /// <summary>
        /// 导出Activity为excel表
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetActivitysToExcel(){
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
        public async Task<GetActivityForEditOutput> GetActivityForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetActivityForEditOutput();
            ActivityEditDto activityEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _activityRepository.GetAsync(input.Id.Value);

                activityEditDto = entity.MapTo<ActivityEditDto>();

                //activityEditDto = ObjectMapper.Map<List <activityEditDto>>(entity);
            }
            else
            {
                activityEditDto = new ActivityEditDto();
            }

            output.Activity = activityEditDto;
            return output;

        }

        /// <summary>
        /// 添加或者修改Activity的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateActivity(CreateOrUpdateActivityInput input)
        {

            if (input.Activity.Id.HasValue)
            {
                await UpdateActivityAsync(input.Activity);
            }
            else
            {
                await CreateActivityAsync(input.Activity);
            }
        }

        /// <summary>
        /// 新增Activity
        /// </summary>
        //[AbpAuthorize(ActivityAppPermissions.Activity_CreateActivity)]
        protected virtual async Task<ActivityEditDto> CreateActivityAsync(ActivityEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var entity = ObjectMapper.Map<Activity>(input);
            entity.TenantId = AbpSession.TenantId;
            entity = await _activityRepository.InsertAsync(entity);
            return entity.MapTo<ActivityEditDto>();
        }

        /// <summary>
        /// 编辑Activity
        /// </summary>
        //[AbpAuthorize(ActivityAppPermissions.Activity_EditActivity)]
        protected virtual async Task<ActivityEditDto> UpdateActivityAsync(ActivityEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            var entity = await _activityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _activityRepository.UpdateAsync(entity);
            return entity.MapTo<ActivityEditDto>();
        }

        /// <summary>
        /// 删除Activity信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[AbpAuthorize(ActivityAppPermissions.Activity_DeleteActivity)]
        public async Task DeleteActivity(EntityDto<Guid> input)
        {

            //TODO:删除前的逻辑判断，是否允许删除
            await _activityRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除Activity的方法
        /// </summary>
        //[AbpAuthorize(ActivityAppPermissions.Activity_BatchDeleteActivities)]
        public async Task BatchDeleteActivitiesAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _activityRepository.DeleteAsync(s => input.Contains(s.Id));
        }
        /// <summary>
        /// 通过租户id获取
        /// </summary>
        /// <returns></returns>
        public async Task<ActivityListDto> GetActivityByIdDtoAsync(EntityDto<Guid> input)
        {
            var query =await _activityRepository.GetAll().Where(r => r.Id == input.Id).FirstOrDefaultAsync();
            return query.MapTo<ActivityListDto>();
        }

        /// <summary>
        /// 添加或者修改Activity的公共方法
        /// </summary>
        public async Task<ActivityEditDto> CreateOrUpdateActivityDto(ActivityEditDto input)
        {
            if (input.Id.HasValue)
            {
                return await UpdateActivityAsync(input);
            }
            else
            {
                return  await CreateActivityAsync(input);
            }
        }



        /// <summary>
        /// 联动删除商品信息
        /// </summary>
        /// <param name="input">活动id</param>
        /// <returns></returns>
        public async Task DeleteActivitiesAsyncDtos(EntityDto<Guid> input)
        {
            List<Guid> idList = new List<Guid>();
            var goodes = _activitygoodsRepository.GetAll().Where(g => g.ActivityId == input.Id).ToList();
            if (goodes.Count > 0)
            {
                foreach (ActivityGoods i in goodes)
                {
                    idList.Add(i.Id);
                }
            }
            await DeleteActivity(input);
            if (idList.Count > 0)
            {
                await _activitygoodsRepository.DeleteAsync(s => idList.Contains(s.Id));
            }
        }
        /// <summary>
        /// 发布是否可用
        /// </summary>
        /// <returns></returns>
        public bool IsPulish()
        {
            var count = _activityRepository.GetAll().Where(a => a.Status == ActivityStatusEnum.已发布 && a.IsDeleted == false).Count();
            if (count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<ActivityListDto> GetTenantWeChatActivityAsync(int? tenantId)
        {
            var activity = await _activityManager.GetTenantWeChatActivityAsync(tenantId);
            return activity.MapTo<ActivityListDto>();
        }
    }
}


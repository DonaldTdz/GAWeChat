using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;

using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using HC.WeChat.MemberConfigs.Authorization;
using HC.WeChat.MemberConfigs.Dtos;
using HC.WeChat.MemberConfigs.DomainServices;
using HC.WeChat.MemberConfigs;
using System;
using System.Linq;
using HC.WeChat.WechatEnums;
using HC.WeChat.Authorization;
using HC.WeChat.WeChatUsers;
using Abp.Auditing;

namespace HC.WeChat.MemberConfigs
{
    /// <summary>
    /// MemberConfig应用层服务的接口实现方法
    /// </summary>
    //[AbpAuthorize(MemberConfigAppPermissions.MemberConfig)]
    [AbpAuthorize(AppPermissions.Pages)]
    public class MemberConfigAppService : WeChatAppServiceBase, IMemberConfigAppService
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        private readonly IRepository<MemberConfig, Guid> _memberconfigRepository;
        private readonly IMemberConfigManager _memberconfigManager;
        private readonly IRepository<WeChatUser, Guid> _wechatuserRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        public MemberConfigAppService(IRepository<MemberConfig, Guid> memberconfigRepository
      , IMemberConfigManager memberconfigManager
            , IRepository<WeChatUser, Guid> wechatuserRepository
        )
        {
            _memberconfigRepository = memberconfigRepository;
            _memberconfigManager = memberconfigManager;
            _wechatuserRepository = wechatuserRepository;

        }

        /// <summary>
        /// 获取MemberConfig的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<MemberConfigListDto>> GetPagedMemberConfigs(GetMemberConfigsInput input)
        {

            var query = _memberconfigRepository.GetAll();
            //TODO:根据传入的参数添加过滤条件
            var memberconfigCount = await query.CountAsync();

            var memberconfigs = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            //var memberconfigListDtos = ObjectMapper.Map<List <MemberConfigListDto>>(memberconfigs);
            var memberconfigListDtos = memberconfigs.MapTo<List<MemberConfigListDto>>();

            return new PagedResultDto<MemberConfigListDto>(
                memberconfigCount,
                memberconfigListDtos
                );

        }

        /// <summary>
        /// 通过指定id获取MemberConfigListDto信息
        /// </summary>
        public async Task<MemberConfigListDto> GetMemberConfigByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _memberconfigRepository.GetAsync(input.Id);

            return entity.MapTo<MemberConfigListDto>();
        }

        /// <summary>
        /// 导出MemberConfig为excel表
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetMemberConfigsToExcel(){
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
        public async Task<GetMemberConfigForEditOutput> GetMemberConfigForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetMemberConfigForEditOutput();
            MemberConfigEditDto memberconfigEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _memberconfigRepository.GetAsync(input.Id.Value);

                memberconfigEditDto = entity.MapTo<MemberConfigEditDto>();

                //memberconfigEditDto = ObjectMapper.Map<List <memberconfigEditDto>>(entity);
            }
            else
            {
                memberconfigEditDto = new MemberConfigEditDto();
            }

            output.MemberConfig = memberconfigEditDto;
            return output;

        }

        /// <summary>
        /// 添加或者修改MemberConfig的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateMemberConfig(MemberConfigEditDto input)
        {

            if (input.Id.HasValue)
            {
                await UpdateMemberConfigAsync(input);
            }
            else
            {
                await CreateMemberConfigAsync(input);
            }
        }

        /// <summary>
        /// 新增MemberConfig
        /// </summary>
        //[AbpAuthorize(MemberConfigAppPermissions.MemberConfig_CreateMemberConfig)]
        protected virtual async Task<MemberConfigEditDto> CreateMemberConfigAsync(MemberConfigEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var entity = ObjectMapper.Map<MemberConfig>(input);
            entity = await _memberconfigRepository.InsertAsync(entity);
            return entity.MapTo<MemberConfigEditDto>();
        }

        /// <summary>
        /// 编辑MemberConfig
        /// </summary>
        //[AbpAuthorize(MemberConfigAppPermissions.MemberConfig_EditMemberConfig)]
        protected virtual async Task UpdateMemberConfigAsync(MemberConfigEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            var entity = await _memberconfigRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _memberconfigRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除MemberConfig信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[AbpAuthorize(MemberConfigAppPermissions.MemberConfig_DeleteMemberConfig)]
        public async Task DeleteMemberConfig(EntityDto<Guid> input)
        {

            //TODO:删除前的逻辑判断，是否允许删除
            await _memberconfigRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除MemberConfig的方法
        /// </summary>
        //[AbpAuthorize(MemberConfigAppPermissions.MemberConfig_BatchDeleteMemberConfigs)]
        public async Task BatchDeleteMemberConfigsAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _memberconfigRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 通过租户id获取积分配置
        /// </summary>
        /// <returns></returns>
        public async Task<List<MemberConfigListDto>> GetTenanMemberConfigAsync()
        {
            var entity = await _memberconfigRepository.GetAll().Where(u => u.TenantId == AbpSession.TenantId).ToListAsync();
            //return await Task.FromResult(entity.MapTo<List<MemberConfigListDto>>());
            return entity.MapTo<List<MemberConfigListDto>>();

        }

        /// <summary>
        /// 新增or修改积分配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateMemberConfigDtoAsync(MemberCodeEditDto input)
        {
            MemberConfigEditDto dto = new MemberConfigEditDto();
            if (input.ECode == 2 && input.EId.HasValue)
            {
                dto.Code = DeployCodeEnum.商品评价;
                dto.Value = input.EValue;
                dto.Type = DeployTypeEnum.积分配置;
                dto.CreationTime = DateTime.Now;
                dto.Id = input.EId;
                dto.Desc = "店铺评价积分配置";
                await UpdateMemberConfigAsync(dto);
            }
            else
            {
                dto.Code = DeployCodeEnum.商品评价;
                dto.Value = input.EValue;
                dto.Type = DeployTypeEnum.积分配置;
                dto.CreationTime = DateTime.Now;
                dto.Id = Guid.NewGuid();
                dto.Desc = "店铺评价积分配置";
                await CreateMemberConfigAsync(dto);
            }
            if (input.CCode == 1 && input.CId.HasValue)
            {
                dto.Code = DeployCodeEnum.商品购买;
                dto.Value = input.CValue;
                dto.Type = DeployTypeEnum.积分配置;
                dto.CreationTime = DateTime.Now;
                dto.Id = input.CId;
                dto.Desc = "商品购买积分配置";
                await UpdateMemberConfigAsync(dto);
            }
            else
            {
                dto.Code = DeployCodeEnum.商品购买;
                dto.Value = input.CValue;
                dto.Type = DeployTypeEnum.积分配置;
                dto.CreationTime = DateTime.Now;
                dto.Id = Guid.NewGuid();
                dto.Desc = "商品购买积分配置";
                await CreateMemberConfigAsync(dto);

            }
            if (input.RcCode == 3 && input.RcId.HasValue)
            {
                dto.Code = DeployCodeEnum.店铺扫码兑换;
                dto.Value = input.RcValue;
                dto.Type = DeployTypeEnum.积分配置;
                dto.CreationTime = DateTime.Now;
                dto.Id = input.RcId;
                dto.Desc = "店铺扫码积分配置";
                await UpdateMemberConfigAsync(dto);
            }
            else
            {
                dto.Code = DeployCodeEnum.店铺扫码兑换;
                dto.Value = input.RcValue;
                dto.Type = DeployTypeEnum.积分配置;
                dto.CreationTime = DateTime.Now;
                dto.Id = Guid.NewGuid();
                dto.Desc = "店铺扫码积分配置";
                await CreateMemberConfigAsync(dto);

            }
            if (input.FCode == 5 && input.FId.HasValue)
            {
                dto.Code = DeployCodeEnum.首次注册;
                dto.Value = input.FValue;
                dto.Type = DeployTypeEnum.积分配置;
                dto.CreationTime = DateTime.Now;
                dto.Id = input.FId;
                dto.Desc = "首次注册积分配置";
                await UpdateMemberConfigAsync(dto);
            }
            else
            {
                dto.Code = DeployCodeEnum.首次注册;
                dto.Value = input.FValue;
                dto.Type = DeployTypeEnum.积分配置;
                dto.Id = Guid.NewGuid();
                dto.CreationTime = DateTime.Now;
                dto.Desc = "首次注册积分配置";
                await CreateMemberConfigAsync(dto);

            }
        }

        /// <summary>
        /// 新增or修改微信通知用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateWXMemberConfigDtoAsync(MemberCodeEditDto input)
        {
            MemberConfigEditDto dto = new MemberConfigEditDto();
            if (input.UserCode == 4 && input.UserId.HasValue)
            {
                dto.Code = DeployCodeEnum.通知配置;
                dto.Value = input.UserValue;
                dto.Type = DeployTypeEnum.通知配置;
                dto.CreationTime = DateTime.Now;
                dto.Id = input.UserId;
                if (input.Desc.Length <= 0)
                {
                    dto.Desc = null;
                }
                else
                {
                    dto.Desc = input.Desc;
                }
                await UpdateMemberConfigAsync(dto);
            }
            else
            {
                dto.Code = DeployCodeEnum.通知配置;
                dto.Value = input.UserValue;
                dto.Type = DeployTypeEnum.通知配置;
                dto.CreationTime = DateTime.Now;
                dto.Id = Guid.NewGuid();
                if (input.Desc.Length > 0)
                {
                    dto.Desc = input.Desc;
                }
                await CreateMemberConfigAsync(dto);
            }
        }

        /// <summary>
        /// 微信获取积分配置
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<List<MemberConfigListDto>> GetWXMemberConfigByTenantIdAsync(int? tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var entity = await _memberconfigRepository.GetAll().Where(u => u.TenantId == AbpSession.TenantId).ToListAsync();
                return entity.MapTo<List<MemberConfigListDto>>();
            }
        }

        /// <summary>
        /// 获取job配置信息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public MemberConfigListDto GetJobConfig()
        {
            var entity = _memberconfigRepository.GetAll().Where(m => m.Type == DeployTypeEnum.job配置 && m.Code == DeployCodeEnum.jo启动状态).FirstOrDefault();
            return entity.MapTo<MemberConfigListDto>();
        }

        /// <summary>
        /// 获取预设商品搜索配置
        /// </summary>
        /// <returns></returns>
        public async Task<MemberConfigListDto> GetPreProductConfigAsync()
        {
            var entity = await _memberconfigRepository.GetAll().Where(u => u.TenantId == AbpSession.TenantId && u.Code == DeployCodeEnum.预设商品搜索 && u.Type == DeployTypeEnum.预设商品配置).FirstOrDefaultAsync();
            return entity.MapTo<MemberConfigListDto>();
        }

        /// <summary>
        /// 新增Or修改预设搜索
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdatePreProductConfig(MemberConfigEditDto input)
        {
            input.Desc = input.Value;
            input.Code = DeployCodeEnum.预设商品搜索;
            input.Type = DeployTypeEnum.预设商品配置;
            if (input.Id.HasValue)
            {
                await UpdateMemberConfigAsync(input);
            }
            else
            {
                await CreateMemberConfigAsync(input);
            }
        }


        /// <summary>
        /// 微信获取预设商品配置
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<string> GetWXPreProductConfigAsync(int? tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                //string[] preProducts =null;
                var entity = await _memberconfigRepository.GetAll().Where(u => u.TenantId == AbpSession.TenantId && u.Code == DeployCodeEnum.预设商品搜索 && u.Type == DeployTypeEnum.预设商品配置).Select(v=>v.Value).FirstOrDefaultAsync();
                //if (entity!=null)
                //{
                //  preProducts = entity.Value.Split(',');
                //}
                return entity;
            }
        }

        /// <summary>
        /// 获取限制次数
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<int> GetLimitFrequencyAsync()
        {
            var limitFrequency = await _memberconfigRepository.GetAll().Where(v => v.Type == DeployTypeEnum.扫码限制配置 && v.Code == DeployCodeEnum.次数限制).Select(v=>v.Value).FirstOrDefaultAsync();
            if (limitFrequency ==null)
            {
                return 5;
            }
            else
            {
                return Convert.ToInt32(limitFrequency);
            }
        }


        /// <summary>
        /// 获取抽奖活动管理员
        /// </summary>
        /// <returns></returns>
        public async Task<MemberConfigListDto> GetLotteryConfigAsync()
        {
            var entity = await _memberconfigRepository.GetAll().Where(v=>v.Type == DeployTypeEnum.抽奖配置 && v.Code == DeployCodeEnum.抽奖活动管理员).FirstOrDefaultAsync();
            return entity.MapTo<MemberConfigListDto>();
        }
        

        /// <summary>
        /// 判断用户是否是抽奖活动管理员
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<bool> GetIsLotteryAdminAsync(string openId)
        {
            if (string.IsNullOrEmpty(openId))
            {
                return false;
            }
            var isAdmin = await _memberconfigRepository.GetAll().Where(v => v.Code == DeployCodeEnum.抽奖活动管理员 && v.Type == DeployTypeEnum.抽奖配置 && v.Value.Contains(openId)).AnyAsync();
            return isAdmin;
        }
    }
}
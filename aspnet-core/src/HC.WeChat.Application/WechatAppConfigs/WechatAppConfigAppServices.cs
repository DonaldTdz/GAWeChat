using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;

using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using HC.WeChat.WechatAppConfigs.Authorization;
using HC.WeChat.WechatAppConfigs.Dtos;
using HC.WeChat.WechatAppConfigs.DomainServices;
using HC.WeChat.WechatAppConfigs;
using HC.WeChat.Authorization;
using Abp.Auditing;
using System.Linq;
using Abp.Domain.Uow;

namespace HC.WeChat.WechatAppConfigs
{
    /// <summary>
    /// WechatAppConfig应用层服务的接口实现方法
    /// </summary>
    //[AbpAuthorize(WechatAppConfigAppPermissions.WechatAppConfig)]
    [AbpAuthorize(AppPermissions.Pages)]
    public class WechatAppConfigAppService : WeChatAppServiceBase, IWechatAppConfigAppService
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        private readonly IRepository<WechatAppConfig, int> _wechatappconfigRepository;
        private readonly IWechatAppConfigManager _wechatappconfigManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;


        /// <summary>
        /// 构造函数
        /// </summary>
        public WechatAppConfigAppService(IRepository<WechatAppConfig, int> wechatappconfigRepository
        , IWechatAppConfigManager wechatappconfigManager
        , IUnitOfWorkManager unitOfWorkManager
        )
        {
            _wechatappconfigRepository = wechatappconfigRepository;
            _wechatappconfigManager = wechatappconfigManager;
            _unitOfWorkManager = unitOfWorkManager;

        }

        /// <summary>
        /// 获取WechatAppConfig的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<WechatAppConfigListDto>> GetPagedWechatAppConfigs(GetWechatAppConfigsInput input)
        {

            var query = _wechatappconfigRepository.GetAll();
            //TODO:根据传入的参数添加过滤条件
            var wechatappconfigCount = await query.CountAsync();

            var wechatappconfigs = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            //var wechatappconfigListDtos = ObjectMapper.Map<List <WechatAppConfigListDto>>(wechatappconfigs);
            var wechatappconfigListDtos = wechatappconfigs.MapTo<List<WechatAppConfigListDto>>();

            return new PagedResultDto<WechatAppConfigListDto>(
                wechatappconfigCount,
                wechatappconfigListDtos
                );

        }

        /// <summary>
        /// 通过指定id获取WechatAppConfigListDto信息
        /// </summary>
        public async Task<WechatAppConfigListDto> GetWechatAppConfigByIdAsync(EntityDto<int> input)
        {
            var entity = await _wechatappconfigRepository.GetAsync(input.Id);

            return entity.MapTo<WechatAppConfigListDto>();
        }

        /// <summary>
        /// 导出WechatAppConfig为excel表
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetWechatAppConfigsToExcel(){
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
        public async Task<GetWechatAppConfigForEditOutput> GetWechatAppConfigForEdit(NullableIdDto<int> input)
        {
            var output = new GetWechatAppConfigForEditOutput();
            WechatAppConfigEditDto wechatappconfigEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _wechatappconfigRepository.GetAsync(input.Id.Value);

                wechatappconfigEditDto = entity.MapTo<WechatAppConfigEditDto>();

                //wechatappconfigEditDto = ObjectMapper.Map<List <wechatappconfigEditDto>>(entity);
            }
            else
            {
                wechatappconfigEditDto = new WechatAppConfigEditDto();
            }

            output.WechatAppConfig = wechatappconfigEditDto;
            return output;

        }

        /// <summary>
        /// 添加或者修改WechatAppConfig的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateWechatAppConfig(CreateOrUpdateWechatAppConfigInput input)
        {

            if (input.WechatAppConfig.Id.HasValue)
            {
                await UpdateWechatAppConfigAsync(input.WechatAppConfig);
            }
            else
            {
                await CreateWechatAppConfigAsync(input.WechatAppConfig);
            }
        }

        /// <summary>
        /// 新增WechatAppConfig
        /// </summary>
        //[AbpAuthorize(WechatAppConfigAppPermissions.WechatAppConfig_CreateWechatAppConfig)]
        protected virtual async Task<WechatAppConfigEditDto> CreateWechatAppConfigAsync(WechatAppConfigEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var entity = ObjectMapper.Map<WechatAppConfig>(input);
            entity.TenantId = AbpSession.TenantId;
            entity = await _wechatappconfigRepository.InsertAsync(entity);
            return entity.MapTo<WechatAppConfigEditDto>();
        }

        /// <summary>
        /// 编辑WechatAppConfig
        /// </summary>
        //[AbpAuthorize(WechatAppConfigAppPermissions.WechatAppConfig_EditWechatAppConfig)]
        protected virtual async Task UpdateWechatAppConfigAsync(WechatAppConfigEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            var entity = await _wechatappconfigRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _wechatappconfigRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除WechatAppConfig信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[AbpAuthorize(WechatAppConfigAppPermissions.WechatAppConfig_DeleteWechatAppConfig)]
        public async Task DeleteWechatAppConfig(EntityDto<int> input)
        {

            //TODO:删除前的逻辑判断，是否允许删除
            await _wechatappconfigRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除WechatAppConfig的方法
        /// </summary>
        //[AbpAuthorize(WechatAppConfigAppPermissions.WechatAppConfig_BatchDeleteWechatAppConfigs)]
        public async Task BatchDeleteWechatAppConfigsAsync(List<int> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _wechatappconfigRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 通过租户ID获取微信配置
        /// </summary>
        [AbpAllowAnonymous]
        [DisableAuditing]
        [UnitOfWork]
        public Task<WechatAppConfigInfo> GetWechatAppConfig(int? tenantId)
        {
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var info = _wechatappconfigRepository.GetAll().Where(w => w.TenantId == tenantId).FirstOrDefault();
                if (info != null)
                {
                    return Task.FromResult(info.MapTo<WechatAppConfigInfo>());
                }
                return Task.FromResult(new WechatAppConfigInfo());
            }
        }

        /// <summary>
        /// 通过租户id获取微信配置
        /// </summary>
        /// <returns></returns>
        public async Task<WechatAppConfigListDto> GetTenantWechatAppConfigAsync()
        {
            var entity = await _wechatappconfigRepository.GetAll().Where(w => w.TenantId == AbpSession.TenantId).FirstOrDefaultAsync();
            return  entity.MapTo<WechatAppConfigListDto>();
        }

        /// <summary>
        /// 添加或者修改WechatAppConfig的方法
        /// </summary>
        /// <param name="input">微信配置实体</param>
        /// <returns></returns>
        public async Task CreateOrUpdateWechatAppConfigDto(WechatAppConfigEditDto input)
        {
            if (input.Id.HasValue)
            {
                await UpdateWechatAppConfigAsync(input);
            }
            else {
                await CreateWechatAppConfigAsync(input);
            }
        }

        /// <summary>
        /// 新增并获取永久素材Media_Id
        /// </summary>
        /// <returns></returns>
        public string UploadForeverPic(string appId)
        {
            var file = @"C:\SYQSubscribePic.jpg";
            var uploadResult = Senparc.Weixin.MP.AdvancedAPIs.MediaApi.UploadForeverMedia(appId,
                file);
            Logger.InfoFormat("APPId:{0}", appId);
            //设置图片信息
            Logger.InfoFormat("上传的meadia_Id:{0}", uploadResult.media_id);
            return uploadResult.media_id;
        }
    }
}


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
using HC.WeChat.WeChatGroups.Authorization;
using HC.WeChat.WeChatGroups.Dtos;
using HC.WeChat.WeChatGroups.DomainServices;
using HC.WeChat.WeChatGroups;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.CommonAPIs;
using Senparc.Weixin.MP.Containers;
using HC.WeChat.Authorization;
using Senparc.Weixin.Entities;
using HC.WeChat.WechatAppConfigs;
using HC.WeChat.WechatAppConfigs.Dtos;
using HC.WeChat.WeChatUsers;
using System;
using HC.WeChat.Dto;
using HC.WeChat.WechatEnums;
using Senparc.Weixin.MP.AdvancedAPIs.UserTag;
using Abp.Auditing;

namespace HC.WeChat.WeChatGroups
{
    /// <summary>
    /// WeChatGroup应用层服务的接口实现方法
    /// </summary>
    //[AbpAuthorize(WeChatGroupAppPermissions.WeChatGroup)]
    [AbpAuthorize(AppPermissions.Pages)]
    public class WeChatGroupAppService : WeChatAppServiceBase, IWeChatGroupAppService
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        private readonly IRepository<WeChatGroup, int> _wechatgroupRepository;
        private readonly IWeChatGroupManager _wechatgroupManager;
        IWechatAppConfigAppService _wechatAppConfigAppService;
        private readonly IRepository<WeChatUser, Guid> _wechatuserRepository;

        private int? TenantId { get; set; }
        private WechatAppConfigInfo AppConfig { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public WeChatGroupAppService(IRepository<WeChatGroup, int> wechatgroupRepository
      , IWeChatGroupManager wechatgroupManager, IWechatAppConfigAppService wechatAppConfigAppService
      , IRepository<WeChatUser, Guid> wechatuserRepository
        )
        {
            _wechatgroupRepository = wechatgroupRepository;
            _wechatgroupManager = wechatgroupManager;
            _wechatAppConfigAppService = wechatAppConfigAppService;
            TenantId = null;
            AppConfig = _wechatAppConfigAppService.GetWechatAppConfig(TenantId).Result;
            _wechatuserRepository = wechatuserRepository;
        }

        /// <summary>
        /// 获取WeChatGroup的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<WeChatGroupListDto>> GetPagedWeChatGroups(GetWeChatGroupsInput input)
        {

            var query = _wechatgroupRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(input.Name), g => g.TagName.Contains(input.Name));
            //TODO:根据传入的参数添加过滤条件
            var wechatgroupCount = await query.CountAsync();

            var wechatgroups = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            //var wechatgroupListDtos = ObjectMapper.Map<List <WeChatGroupListDto>>(wechatgroups);
            var wechatgroupListDtos = wechatgroups.MapTo<List<WeChatGroupListDto>>();

            return new PagedResultDto<WeChatGroupListDto>(
                wechatgroupCount,
                wechatgroupListDtos
                );

        }

        /// <summary>
        /// 通过指定id获取WeChatGroupListDto信息
        /// </summary>
        public async Task<WeChatGroupListDto> GetWeChatGroupByIdAsync(EntityDto<int> input)
        {
            var entity = await _wechatgroupRepository.GetAsync(input.Id);

            return entity.MapTo<WeChatGroupListDto>();
        }

        /// <summary>
        /// 导出WeChatGroup为excel表
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetWeChatGroupsToExcel(){
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
        public async Task<GetWeChatGroupForEditOutput> GetWeChatGroupForEdit(NullableIdDto<int> input)
        {
            var output = new GetWeChatGroupForEditOutput();
            WeChatGroupEditDto wechatgroupEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _wechatgroupRepository.GetAsync(input.Id.Value);

                wechatgroupEditDto = entity.MapTo<WeChatGroupEditDto>();

                //wechatgroupEditDto = ObjectMapper.Map<List <wechatgroupEditDto>>(entity);
            }
            else
            {
                wechatgroupEditDto = new WeChatGroupEditDto();
            }

            output.WeChatGroup = wechatgroupEditDto;
            return output;

        }

        /// <summary>
        /// 添加或者修改WeChatGroup的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateWeChatGroup(CreateOrUpdateWeChatGroupInput input)
        {

            if (input.WeChatGroup.Id.HasValue)
            {
                await UpdateWeChatGroupAsync(input.WeChatGroup);
            }
            else
            {
                await CreateWeChatGroupAsync(input.WeChatGroup);
            }
        }

        /// <summary>
        /// 新增WeChatGroup
        /// </summary>
        //[AbpAuthorize(WeChatGroupAppPermissions.WeChatGroup_CreateWeChatGroup)]
        [AbpAllowAnonymous]
        protected virtual async Task<WeChatGroupEditDto> CreateWeChatGroupAsync(WeChatGroupEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var entity = ObjectMapper.Map<WeChatGroup>(input);

            entity = await _wechatgroupRepository.InsertAsync(entity);
            return entity.MapTo<WeChatGroupEditDto>();
        }

        /// <summary>
        /// 编辑WeChatGroup
        /// </summary>
        //[AbpAuthorize(WeChatGroupAppPermissions.WeChatGroup_EditWeChatGroup)]
        protected virtual async Task UpdateWeChatGroupAsync(WeChatGroupEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            var entity = await _wechatgroupRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _wechatgroupRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除WeChatGroup信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[AbpAuthorize(WeChatGroupAppPermissions.WeChatGroup_DeleteWeChatGroup)]
        public async Task DeleteWeChatGroup(EntityDto<int> input)
        {

            //TODO:删除前的逻辑判断，是否允许删除
            await _wechatgroupRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除WeChatGroup的方法
        /// </summary>
        [AbpAuthorize(WeChatGroupAppPermissions.WeChatGroup_BatchDeleteWeChatGroups)]
        public async Task BatchDeleteWeChatGroupsAsync(List<int> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _wechatgroupRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 获取所有分组信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<WeChatGroupListDto>> GetAllWeChatGroupAsync()
        {
            var weChatGroupList = await _wechatgroupRepository.GetAllListAsync();
            return weChatGroupList.MapTo<List<WeChatGroupListDto>>();
        }

        /// <summary>
        /// 检查组名是否重复
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task GetCheckWeChatGroup(string tagName, int? tagId)
        {
            var result = new CheckResult();
            var weChatGroup = await _wechatgroupRepository.GetAll().Where(g => g.TagName == tagName).SingleOrDefaultAsync();
            if (weChatGroup != null) { }
        }

        /// <summary>
        ///检查在微信端是否已存在此标签
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public async Task<CheckResult> CheckTagName(string tagName)
        {
            var tags = await UserTagApi.GetAsync(AppConfig.AppId);
            var result = new CheckResult();
            foreach (var item in tags.tags)
            {
                if (item.name == tagName)
                {
                    result.IsExist = true;
                    result.TagId = item.id;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 创建分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task CreateWeChatGroup(WeChatGroupListDto input)
        {

            await GetTagIdAsync(input.TypeCode);
        }

        /// <summary>
        /// 修改分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateWeChatGroup(WeChatGroupListDto input)
        {
            var group = await UserTagApi.UpdateAsync(AppConfig.AppId, input.TagId, input.TagName);
            if (group.errcode == 0)
            {
                await UpdateWeChatGroupAsync(input.MapTo<WeChatGroupEditDto>());
            }
        }

        /// <summary>
        ///  删除分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteWeChatGroupAsync(WeChatGroupListDto input)
        {
            var group = await UserTagApi.DeleteAsync(AppConfig.AppId, input.TagId);
            if (group.errcode == 0)
            {
                await _wechatgroupRepository.DeleteAsync(input.Id);
            }
        }

        /// <summary>
        /// 单个标记用户分组
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="tagId">标签id</param>
        /// <param name="tagName">标签名</param>
        /// <returns></returns>
        public async Task MarkWeChatGroup(string openId, int tagId)
        {
            List<string> openIds = new List<string>();
            openIds.Add(openId);
            await UserTagApi.BatchTaggingAsync(AppConfig.AppId, tagId, openIds);
        }

        /// <summary>
        /// 批量标记用户分组
        /// </summary>
        /// <returns></returns>
        public async Task BatchMarkWeChatGroup()
        {
            //try
            //{
            //}
            //catch (Exception ex)
            //{

            //}
            var weChatGroupList = await _wechatgroupRepository.GetAllListAsync();
            foreach (var item in weChatGroupList)
            {
                //if(item.TypeCode!= UserTypeEnum.消费者)
                //{
                var count = await _wechatuserRepository.GetAll().Where(g => g.UserType == item.TypeCode && g.BindStatus == BindStatusEnum.已绑定).Select(g => g.OpenId).CountAsync();
                if (count > 0)
                {
                    int cycleCount = count / 50 + (count % 50 == 0 ? 0 : 1);
                    for (var i = 0; i < cycleCount; i++)
                    {
                        var weChatUser = await _wechatuserRepository.GetAll().Where(g => g.UserType == item.TypeCode && g.BindStatus == BindStatusEnum.已绑定).Skip(i * 50).Take(50).Select(g => g.OpenId).ToListAsync();

                        var result = await UserTagApi.BatchTaggingAsync(AppConfig.AppId, item.TagId, weChatUser);
                        //if (result.errcode != 0)
                        //{
                        //    return new APIResultDto() { Code = 0, Msg = "标记失败" };
                        //}
                    }
                }
                //}
            }
            //return new APIResultDto() { Code = 0, Msg = "标记成功" };
        }

        /// <summary>
        /// 获取在微信中的分组（标签）
        /// </summary>
        /// <returns></returns>
        public async Task GetWeChatGroupFromWeChat()
        {
            await UserTagApi.GetAsync(AppConfig.AppId);
        }

        /// <summary>
        /// 获取用户上的标签
        /// </summary>
        /// <returns></returns>
        public async Task GetUserForWeChatUser()
        {
            var openId = "";
            await UserTagApi.UserTagListAsync(AppConfig.AppId, openId);
        }

        /// <summary>
        /// 获取微信分组（标签）id
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<int> GetTagIdAsync(UserTypeEnum typeSCode)
        {
            int tagId = 0;
            string tagName = "";
            //var fans1 = await UserTagApi.UserTagListAsync(AppConfig.AppId, "oPM5Uv81jfyJqWbVxWAH-RUqsCAs");

            var tags = await UserTagApi.GetAsync(AppConfig.AppId);
            var groupSe = await _wechatgroupRepository.GetAll().Where(G => G.TypeName == typeSCode.ToString()).FirstOrDefaultAsync();
            foreach (var item in tags.tags)
            {
                if (item.name == typeSCode.ToString())
                {
                    tagId = item.id;
                    tagName = item.name;
                }
            }
            if (string.IsNullOrEmpty(tagName))
            {
                var result = await UserTagApi.CreateAsync(AppConfig.AppId, typeSCode.ToString());
                if (groupSe == null)
                {
                    WeChatGroupListDto group = new WeChatGroupListDto();
                    if (result.errcode == 0)
                    {
                        group.TagId = result.tag.id;
                        group.TagName = result.tag.name;
                        group.TypeName = typeSCode.ToString();
                        group.TypeCode = typeSCode;
                        await CreateWeChatGroupAsync(group.MapTo<WeChatGroupEditDto>());
                    }
                }
                return result.tag.id;
            }
            else
            {
                if (groupSe == null)
                {
                    WeChatGroupListDto group = new WeChatGroupListDto();
                    group.TagId = tagId;
                    group.TagName = tagName;
                    group.TypeName = typeSCode.ToString();
                    group.TypeCode = typeSCode;
                    await CreateWeChatGroupAsync(group.MapTo<WeChatGroupEditDto>());
                }
                return tagId;
            }
        }

        /// <summary>
        /// 为用户取消标签
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task CancelTagAsync(int tagId,List<string> openIds)
        {
            //List<string> openIds = new List<string>();
            //openIds.Add(openId);
            var result = await UserTagApi.BatchUntaggingAsync(AppConfig.AppId, tagId, openIds);
        }

        /// <summary>
        /// 通过用户类型获取分组信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<WeChatGroupListDto> GetWeChatGroupByUserType(UserTypeEnum code)
        {
            var result = await _wechatgroupRepository.GetAll().Where(g => g.TypeCode == code).FirstOrDefaultAsync();
            return result.MapTo<WeChatGroupListDto>();
        }


        /// <summary>
        /// 获取微信分组（标签）id
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<int> GetTagIdByNameAsync(string groupName,int groupCode)
        {
            int tagId = 0;
            string tagName = "";
            var tags = await UserTagApi.GetAsync(AppConfig.AppId);
            var groupSe = await _wechatgroupRepository.GetAll().Where(G => G.TypeName == groupName).FirstOrDefaultAsync();
            foreach (var item in tags.tags)
            {
                if (item.name == groupName)
                {
                    tagId = item.id;
                    tagName = item.name;
                }
            }
            if (string.IsNullOrEmpty(tagName))
            {
                var result = await UserTagApi.CreateAsync(AppConfig.AppId, groupName);
                if (groupSe == null)
                {
                    WeChatGroupByNameDto group = new WeChatGroupByNameDto();
                    if (result.errcode == 0)
                    {
                        group.TagId = result.tag.id;
                        group.TagName = result.tag.name;
                        group.TypeName = groupName;
                        group.TypeCode = groupCode;
                        await CreateWeChatGroupByNameAsync(group.MapTo<WeChatGroupByNameDto>());
                    }
                }
                return result.tag.id;
            }
            else
            {
                if (groupSe == null)
                {
                    WeChatGroupByNameDto group = new WeChatGroupByNameDto();
                    group.TagId = tagId;
                    group.TagName = tagName;
                    group.TypeName = groupName;
                    group.TypeCode = groupCode;
                    await CreateWeChatGroupByNameAsync(group.MapTo<WeChatGroupByNameDto>());
                }
                return tagId;
            }
        }

        /// <summary>
        /// 根据name创建标签
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        protected virtual async Task<WeChatGroupEditDto> CreateWeChatGroupByNameAsync(WeChatGroupByNameDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var entity = ObjectMapper.Map<WeChatGroup>(input);

            entity = await _wechatgroupRepository.InsertAsync(entity);
            return entity.MapTo<WeChatGroupEditDto>();
        }
    }
}


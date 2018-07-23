using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;

using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using HC.WeChat.WeChatUsers.Dtos;
using HC.WeChat.WeChatUsers.DomainServices;
using System;
using HC.WeChat.Authorization;
using HC.WeChat.Dto;
using HC.WeChat.WechatEnums;
using HC.WeChat.Retailers;
using HC.WeChat.Employees;
using System.Linq;
using Abp.Domain.Uow;
using HC.WeChat.WeChatGroups;
using HC.WeChat.WechatAppConfigs.Dtos;
using HC.WeChat.WechatAppConfigs;
using Senparc.Weixin.MP.AdvancedAPIs;
using HC.WeChat.Retailers.Dtos;
using HC.WeChat.Employees.Dtos;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using HC.WeChat.MemberConfigs;
using HC.WeChat.Helpers;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Microsoft.AspNetCore.Hosting;
using HC.WeChat.IntegralDetails;

namespace HC.WeChat.WeChatUsers
{
    /// <summary>
    /// WeChatUser应用层服务的接口实现方法
    /// </summary>
    //[AbpAuthorize(WeChatUserAppPermissions.WeChatUser)]
    [AbpAuthorize(AppPermissions.Pages)]
    public class WeChatUserAppService : WeChatAppServiceBase, IWeChatUserAppService
    {
        private readonly IRepository<WeChatUser, Guid> _wechatuserRepository;
        private readonly IWeChatUserManager _wechatuserManager;
        private readonly IRepository<Retailer, Guid> _retailerRepository;
        private readonly IRepository<Employee, Guid> _employeeRepository;
        private readonly IRepository<WeChatGroup, int> _wechatgroupRepository;
        private readonly IRepository<MemberConfig, Guid> _memberconfigRepository;
        private readonly IRepository<WechatAppConfig, int> _wechatappconfigRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IRepository<IntegralDetail, Guid> _integraldetailRepository;
        private readonly IRepository<MemberConfig, Guid> _memberConfigRepository;


        public int? TenantId { get; set; }
        public WechatAppConfigInfo AppConfig { get; set; }

        IWechatAppConfigAppService _wechatAppConfigAppService;
        IWeChatGroupAppService _wechatGroupAppService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WeChatUserAppService(IRepository<WeChatUser, Guid> wechatuserRepository,
       IWeChatUserManager wechatuserManager,
       IRepository<Retailer, Guid> retailerRepository,
       IRepository<Employee, Guid> employeeRepository,
       IRepository<WeChatGroup, int> wechatgroupRepository,
       IWechatAppConfigAppService wechatAppConfigAppService,
        IWeChatGroupAppService wechatGroupAppService,
        IRepository<MemberConfig, Guid> memberconfigRepository,
            IRepository<WechatAppConfig, int> wechatappconfigRepository
              , IHostingEnvironment hostingEnvironment
                        , IRepository<IntegralDetail, Guid> integraldetailRepository
                        , IRepository<MemberConfig, Guid> memberConfigRepository
        )
        {
            _hostingEnvironment = hostingEnvironment;
            _memberconfigRepository = memberconfigRepository;
            _wechatuserRepository = wechatuserRepository;
            _wechatuserManager = wechatuserManager;
            _retailerRepository = retailerRepository;
            _employeeRepository = employeeRepository;
            _wechatgroupRepository = wechatgroupRepository;
            _wechatAppConfigAppService = wechatAppConfigAppService;
            TenantId = null;
            AppConfig = _wechatAppConfigAppService.GetWechatAppConfig(TenantId).Result;
            _wechatGroupAppService = wechatGroupAppService;
            _wechatappconfigRepository = wechatappconfigRepository;
            _integraldetailRepository = integraldetailRepository;
            _memberConfigRepository = memberConfigRepository;
        }

        /// <summary>
        /// 获取WeChatUser的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<WeChatUserListDto>> GetPagedWeChatUsers(GetWeChatUsersInput input)
        {

            var query = _wechatuserRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(input.UserName), u => u.UserName.Contains(input.UserName))
                .WhereIf(!string.IsNullOrEmpty(input.Name), u => u.NickName.Contains(input.Name) || u.UserName.Contains(input.Name) || u.Phone.Contains(input.Name))
                .WhereIf(input.UserType.HasValue, u => u.UserType == input.UserType);

            //TODO:根据传入的参数添加过滤条件
            var wechatuserCount = await query.CountAsync();

            var wechatusers = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            //var wechatuserListDtos = ObjectMapper.Map<List <WeChatUserListDto>>(wechatusers);
            var wechatuserListDtos = wechatusers.MapTo<List<WeChatUserListDto>>();

            return new PagedResultDto<WeChatUserListDto>(
                wechatuserCount,
                wechatuserListDtos
                );

        }

        /// <summary>
        /// 通过指定id获取WeChatUserListDto信息
        /// </summary>
        public async Task<WeChatUserListDto> GetWeChatUserByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _wechatuserRepository.GetAsync(input.Id);

            return entity.MapTo<WeChatUserListDto>();
        }

        /// <summary>
        /// 导出WeChatUser为excel表
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetWeChatUsersToExcel(){
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
        public async Task<GetWeChatUserForEditOutput> GetWeChatUserForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetWeChatUserForEditOutput();
            WeChatUserEditDto wechatuserEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _wechatuserRepository.GetAsync(input.Id.Value);

                wechatuserEditDto = entity.MapTo<WeChatUserEditDto>();

                //wechatuserEditDto = ObjectMapper.Map<List <wechatuserEditDto>>(entity);
            }
            else
            {
                wechatuserEditDto = new WeChatUserEditDto();
            }

            output.WeChatUser = wechatuserEditDto;
            return output;

        }

        /// <summary>
        /// 添加或者修改WeChatUser的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateWeChatUser(CreateOrUpdateWeChatUserInput input)
        {

            if (input.WeChatUser.Id.HasValue)
            {
                await UpdateWeChatUserAsync(input.WeChatUser);
            }
            else
            {
                await CreateWeChatUserAsync(input.WeChatUser);
            }
        }

        /// <summary>
        /// 新增WeChatUser
        /// </summary>
        //[AbpAuthorize(WeChatUserAppPermissions.WeChatUser_CreateWeChatUser)]
        protected virtual async Task<WeChatUserEditDto> CreateWeChatUserAsync(WeChatUserEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var entity = ObjectMapper.Map<WeChatUser>(input);
            entity.TenantId = AbpSession.TenantId;
            entity = await _wechatuserRepository.InsertAsync(entity);
            return entity.MapTo<WeChatUserEditDto>();
        }

        /// <summary>
        /// 编辑WeChatUser
        /// </summary>
        //[AbpAuthorize(WeChatUserAppPermissions.WeChatUser_EditWeChatUser)]
        protected virtual async Task UpdateWeChatUserAsync(WeChatUserEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            var entity = await _wechatuserRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _wechatuserRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除WeChatUser信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[AbpAuthorize(WeChatUserAppPermissions.WeChatUser_DeleteWeChatUser)]
        public async Task DeleteWeChatUser(EntityDto<Guid> input)
        {

            //TODO:删除前的逻辑判断，是否允许删除
            await _wechatuserRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除WeChatUser的方法
        /// </summary>
        //[AbpAuthorize(WeChatUserAppPermissions.WeChatUser_BatchDeleteWeChatUsers)]
        public async Task BatchDeleteWeChatUsersAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _wechatuserRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 用户绑定
        /// </summary>
        [AbpAllowAnonymous]
        public async Task<APIResultDto> BindWeChatUserAsync(UserBindDto input)
        {
            //if (string.IsNullOrEmpty(input.Phone))
            //{
            //    return new APIResultDto() { Code = 904, Msg = "绑定电话不能为空" };
            //}
            //Logger.InfoFormat("UserBindDto:", Newtonsoft.Json.Linq.JObject.FromObject(input).ToString());
            var entity = await _wechatuserManager.GetWeChatUserAsync(input.OpenId, input.TenantId);
            if (entity == null)
            {
                //entity = input.MapTo<WeChatUser>();
                return new APIResultDto() { Code = 902, Msg = "用户不存在" };
            }
            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                if (input.UserType == UserTypeEnum.零售客户)
                {
                    //验证零售户
                    var retaliler = await _retailerRepository.GetAll().Where(r => r.IsAction && r.LicenseKey == input.LicenseKey).FirstOrDefaultAsync();
                    var retalilerDto = retaliler.MapTo<RetailerListDto>();
                    if (retalilerDto != null)
                    {
                        if (retalilerDto.RetailerVerificationCode != input.VerificationCode)
                        {
                            return new APIResultDto() { Code = 901, Msg = "零售户验证未通过" };
                        }
                        //测试验证码
                        //if (input.VerificationCode != "123321" && input.VerificationCode != "668899")
                        //{
                        //    return new APIResultDto() { Code = 901, Msg = "零售户验证未通过" };
                        //}
                    }
                    else
                    {
                        return new APIResultDto() { Code = 902, Msg = "零售户验证未通过" };
                    }

                    //验证微信用户是否绑定过其他零售客户 前端规避重复绑定 暂时注释
                    //if (_wechatuserRepository.GetAll().Any(w => w.OpenId == input.OpenId && w.UserType == UserTypeEnum.零售客户))
                    //{
                    //    return new APIResultDto() { Code = 903, Msg = "该用户已经绑定过零售客户" };
                    //}

                    entity.UserId = retaliler.Id;
                    entity.UserName = retaliler.Name;
                    //检查是否是第一个绑定 是为店铺管理员 不是需要店铺管理员审核
                    var isExistsBind = await _wechatuserRepository.GetAll().AnyAsync(r => r.UserId == retaliler.Id && r.UserType == UserTypeEnum.零售客户);
                    if (!isExistsBind)
                    {
                        entity.IsShopkeeper = true;
                        entity.Status = UserAuditStatus.已审核;
                    }
                    else
                    {
                        entity.IsShopkeeper = false;
                        entity.Status = UserAuditStatus.未审核;
                        //发送审核通知
                        var retalilerOpenId = await _wechatuserRepository.GetAll().Where(r => r.UserId == entity.UserId).Select(v => v.OpenId).FirstOrDefaultAsync();
                        var currentName = await _wechatuserRepository.GetAll().Where(r => r.OpenId == input.OpenId).Select(v => v.NickName).FirstOrDefaultAsync();
                        await SendCheckMesssage(retalilerOpenId, input.host, currentName);
                    }
                }
                else if (input.UserType == UserTypeEnum.内部员工)
                {
                    //验证客户经理
                    var employee = await _employeeRepository.GetAll().Where(e => e.IsAction && e.Code == input.Code).FirstOrDefaultAsync();
                    var employeeDto = employee.MapTo<EmployeeListDto>();
                    if (employeeDto != null)
                    {
                        if (employeeDto.EmployeeVerificationCode != input.VerificationCode)
                        {
                            return new APIResultDto() { Code = 903, Msg = "内部员工验证未通过" };
                        }
                        //if (input.VerificationCode != "123321" && input.VerificationCode != "668899")
                        //{
                        //    return new APIResultDto() { Code = 901, Msg = "内部员工验证未通过" };
                        //}
                    }
                    else
                    {
                        return new APIResultDto() { Code = 904, Msg = "内部员工验证未通过" };
                    }

                    //验证该内部员工是否被绑定过 2018-6-4
                    if (await _wechatuserRepository.GetAll().AnyAsync(w => w.UserId == employeeDto.Id && w.UserType == UserTypeEnum.内部员工))
                    {
                        return new APIResultDto() { Code = 905, Msg = "该员工账号已被绑定" };
                    }

                    entity.UserId = employee.Id;
                    entity.UserName = employee.Name;
                }
                else
                {
                    return new APIResultDto() { Code = 903, Msg = "用户类型不支持" };
                }

                //entity.UserName = input.UserName;
                entity.UserType = input.UserType;
                entity.BindStatus = BindStatusEnum.已绑定;
                entity.BindTime = DateTime.Now;
                entity.OpenId = input.OpenId;
                entity.TenantId = input.TenantId;
                var result = await _wechatuserManager.BindWeChatUserAsync(entity);

                //绑定成功后打标签
                if (result.BindStatus == BindStatusEnum.已绑定)
                {
                    await TagForWechatAsync(entity);
                }
                return new APIResultDto() { Code = 0, Msg = "绑定成功", Data = entity.MapTo<WeChatUserListDto>() };
            }
        }

        /// <summary>
        /// 发送审核通知
        /// </summary>
        /// <param name="OpenId"></param>
        /// <param name="host"></param>
        /// <param name="currentName"></param>
        /// <returns></returns>
        public async Task SendCheckMesssage(string OpenId, string host, string currentName)
        {
            try
            {
                string templateId = await _wechatappconfigRepository.GetAll().Select(v => v.TemplateIds).FirstOrDefaultAsync();
                if (templateId != null || templateId.Length != 0)
                {
                    string[] ids = templateId.Split(',');
                    string appId = AppConfig.AppId;
                    string openId = OpenId;
                    //string templateId = "qvt7CNXBY4FzfzdX54TvMUaOi9jZ3-tdsb2NRhVp0yg";//模版id  
                    host = host ?? "http://ga.intcov.com";//host配置
                    string url = host + "/GAWX/Authorization?page=302";
                    object data = new
                    {
                        first = new TemplateDataItem("店员审核通知，请您尽快审核", "#FF0000"),
                        keyword1 = new TemplateDataItem(currentName.ToString()),
                        keyword2 = new TemplateDataItem(DateTime.Now.ToString("yyyy-MM-dd HH:mm"))
                    };
                    await TemplateApi.SendTemplateMessageAsync(appId, openId, ids[2], url, data);
                }
            }
            catch (Exception ex)
            {

                Logger.ErrorFormat("审核通知发送失败 error：{0} Exception：{1}", ex.Message, ex);
            }
        }

        /// <summary>
        /// 微信用户绑定成功后打标签
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task TagForWechatAsync(WeChatUser entity)
        {
            try
            {
                //var weChatGroup = _wechatgroupRepository.GetAll().Where(g => g.TypeCode == entity.UserType).FirstOrDefaultAsync();
                List<string> openId_list = new List<string>();
                openId_list.Add(entity.OpenId);
                var tagId = await _wechatGroupAppService.GetTagIdAsync(entity.UserType);
                await UserTagApi.BatchTaggingAsync(AppConfig.AppId, tagId, openId_list);
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("TagForWechatAsync-打标签失败:{0},Exception:{1}", e.Message, e);
            }

        }

        [AbpAllowAnonymous]
        [UnitOfWork(isTransactional: false)]
        public async Task<WeChatUserListDto> GetWeChatUserAsync(string openId, int? tenantId)
        {
            var user = await _wechatuserManager.GetWeChatUserAsync(openId, tenantId);
            var userDto = user.MapTo<WeChatUserListDto>();
            return userDto;
        }

        /// <summary>
        /// 添加或者修改WeChatUser的方法
        /// </summary>
        /// <param name="input">微信用户实体</param>
        /// <returns></returns>
        public async Task CreateOrUpdateWeChatUserDto(WeChatUserEditDto input)
        {

            if (input.Id.HasValue)
            {
                await UpdateWeChatUserAsync(input);
            }
            else
            {
                await CreateWeChatUserAsync(input);
            }
        }

        [AbpAllowAnonymous]
        public async Task<APIResultDto> BindMemberAsync(MemberBindDto input)
        {
            if (string.IsNullOrEmpty(input.Phone))
            {
                return new APIResultDto() { Code = 901, Msg = "绑定电话不能为空" };
            }
            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                var entity = await _wechatuserManager.GetWeChatUserAsync(input.OpenId, input.TenantId);
                if (entity == null)
                {
                    return new APIResultDto() { Code = 902, Msg = "用户不存在" };
                }
                entity.Phone = input.Phone;
                entity.MemberBarCode = entity.MemberBarCode ?? GenerateMemberBarCode();
                if (entity.UserType == UserTypeEnum.消费者)
                {
                    entity.BindStatus = BindStatusEnum.已绑定;
                    entity.BindTime = DateTime.Now;
                }
                await _wechatuserRepository.UpdateAsync(entity);
                //首次绑定手机号获赠积分
                await GivenIntegral(input.OpenId, input.TenantId, input.Host);
                return new APIResultDto() { Code = 0, Msg = "绑定成功", Data = entity.MapTo<WeChatUserListDto>() };
            }
        }

        /// <summary>
        /// 首次绑定手机号获赠积分
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        private async Task GivenIntegral(string openId, int? tenantId, string host)
        {
            try
            {
                string phone = await _wechatuserRepository.GetAll().Where(v => v.OpenId == openId).Select(v => v.Phone).FirstOrDefaultAsync();
                if (phone == null || phone.Length == 0)
                {
                    //新增积分详情
                    var config = await GetIntegralConfig(tenantId);
                    var user = await _wechatuserRepository.GetAll().Where(u => u.OpenId == openId).FirstOrDefaultAsync();
                    var intDetail = new IntegralDetail();
                    intDetail.InitialIntegral = user.IntegralTotal;
                    intDetail.Integral = int.Parse(config);
                    intDetail.FinalIntegral = user.IntegralTotal + intDetail.Integral;
                    intDetail.OpenId = user.OpenId;
                    intDetail.RefId = user.OpenId;//自身赠送
                    intDetail.TenantId = tenantId;
                    intDetail.Type = IntegralTypeEnum.首次注册赠送;
                    intDetail.Desc = "首次绑定电话号码注册会员";
                    await _integraldetailRepository.InsertAsync(intDetail);
                    //更新用户总积分
                    user.IntegralTotal = intDetail.FinalIntegral.Value;
                    await _wechatuserRepository.UpdateAsync(user);
                    await CurrentUnitOfWork.SaveChangesAsync(); // 先更新用户总积分
                    int finalIntegral = user.IntegralTotal; // 再传出最终积分
                    await GivenIntegralSendMessage(host, openId, user.MemberBarCode, config, finalIntegral);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("注册赠送积分失败 error：{0} Exception：{1}", ex.Message, ex);
            }
        }

        /// <summary>
        /// 注册积分通知
        /// </summary>
        /// <param name="host"></param>
        /// <param name="openId"></param>
        /// <param name="memberBarCode"></param>
        /// <param name="config"></param>
        /// <param name="finalIntegral"></param>
        /// <returns></returns>
        private async Task GivenIntegralSendMessage(string host, string openId, string memberBarCode, string config, int finalIntegral)
        {
            try
            {
                string templateId = await _wechatappconfigRepository.GetAll().Select(v => v.TemplateIds).FirstOrDefaultAsync();
                if (templateId != null || templateId.Length != 0)
                {
                    string[] ids = templateId.Split(',');
                    //发送微信模板通知-消费者
                    string appId = AppConfig.AppId;
                    //string templateId = "3Dgkz89yi8e0jXtwBUhdMSgHeZwPvHi2gz8WrD-CUA4";//模版id  
                    host = host ?? "http://ga.intcov.com";//host配置
                    string url = host + "/GAWX/Authorization?page=301";
                    object data = new
                    {
                        keyword1 = new TemplateDataItem(memberBarCode),
                        keyword2 = new TemplateDataItem(finalIntegral.ToString() + "积分"),
                        keyword3 = new TemplateDataItem(config + "积分"),
                        keyword4 = new TemplateDataItem(DateTime.Now.ToString("yyyy-MM-dd HH:mm"))
                    };
                    await TemplateApi.SendTemplateMessageAsync(appId, openId, ids[0], url, data);
                }
            }
            catch (Exception ex)
            {

                Logger.ErrorFormat("注册积分消息通知失败 error：{0} Exception：{1}", ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取积分配置
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        private async Task<string> GetIntegralConfig(int? tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                //获取积分配置
                var configValue = await _memberConfigRepository.GetAll().Where(c => c.Type == DeployTypeEnum.积分配置 && c.Code == DeployCodeEnum.首次注册).Select(c => c.Value).FirstOrDefaultAsync();
                if (configValue == null)
                {
                    configValue = "10";
                }
                return configValue;
            }
        }

        /// <summary>
        /// 生成会员卡条形码
        /// </summary>
        private string GenerateMemberBarCode()
        {
            GenerateCode gserver = new GenerateCode(0, 0);
            return gserver.nextId().ToString();
        }

        /// <summary>
        /// 获取店员信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<WeChatUserListDto>> GetPagedShopWeChatUsers(GetShopWeChatUsersInput input)
        {

            var query = _wechatuserRepository.GetAll()
                .Where(w => w.UserId == input.ShopOwnerId)
                .Where(w => w.UserType == UserTypeEnum.零售客户);

            //TODO:根据传入的参数添加过滤条件
            var wechatuserCount = await query.CountAsync();

            var wechatusers = await query
                .OrderByDescending(w => w.IsShopkeeper)
                .ThenBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            //var wechatuserListDtos = ObjectMapper.Map<List <WeChatUserListDto>>(wechatusers);
            var wechatuserListDtos = wechatusers.MapTo<List<WeChatUserListDto>>();

            return new PagedResultDto<WeChatUserListDto>(
                wechatuserCount,
                wechatuserListDtos
                );

        }

        [AbpAllowAnonymous]
        [UnitOfWork(isTransactional: false)]
        public async Task<WeChatUserListDto> GetWeChatUserByMemberBarCodeAsync(string memberBarCode, int? tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var entity = await _wechatuserRepository.GetAll().Where(u => u.MemberBarCode == memberBarCode).FirstOrDefaultAsync();
                return entity.MapTo<WeChatUserListDto>();
            }
        }

        /// <summary>
        /// 获取单个微信用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<WeChatUserListDto> GetSingleWeChatUser(Guid userId, int? tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var entity = await _wechatuserRepository.GetAll().Where(w => w.UserId == userId).SingleOrDefaultAsync();
                return entity.MapTo<WeChatUserListDto>();
            }
        }

        /// <summary>
        /// 解除绑定
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task CheckWeChatUserBindStatusAsync(WeChatUserEditDto input)
        {
            //取消该功能
            //if (input.UserType == UserTypeEnum.内部员工)
            //{
            //    await DealMemeberConfigValueAndDesc(input);
            //}
            await CancelTagAsync(input.UserType, input.OpenId);
            //input.UserType = UserTypeEnum.消费者;
            //input.BindStatus = BindStatusEnum.未绑定;
            //input.UserId = null;
            //input.UnBindTime = DateTime.Now;
            //input.Status = null;
            var entity = await _wechatuserRepository.GetAsync(input.Id.Value);
            entity.UserType = entity.UserType == UserTypeEnum.取消关注 ? UserTypeEnum.取消关注 : UserTypeEnum.消费者;
            entity.BindStatus = BindStatusEnum.未绑定;
            entity.UserId = null;
            entity.UnBindTime = DateTime.Now;
            entity.Status = null;
            //input.MapTo(entity);
            await _wechatuserRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 解绑移除会员配置员工信息 (取消该功能)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task DealMemeberConfigValueAndDesc(WeChatUserEditDto input)
        {
            try
            {
                MemberConfig memeberConfig = await _memberconfigRepository.GetAll().Where(r => r.Code == DeployCodeEnum.通知配置 && r.Type == DeployTypeEnum.通知配置).FirstOrDefaultAsync();
                if (memeberConfig.Desc != null || memeberConfig.Value != null)
                {
                    string newDesc = null;
                    string newValue = null;
                    if (memeberConfig.Desc != null || memeberConfig.Desc.Length != 0)
                    {
                        string[] descIds = memeberConfig.Desc.Split(',');
                        for (int i = 0; i < descIds.Length; i++)
                        {
                            if (descIds[i] != input.UserName)
                            {
                                newDesc += descIds[i] + ",";
                            }
                            else
                            {
                                descIds[i] = null;
                            }
                        }
                        if (newDesc != null)
                        {
                            newDesc = newDesc.TrimStart(',').TrimEnd(',');
                        }
                    }
                    if (memeberConfig.Value != null || memeberConfig.Value.Length != 0)
                    {
                        string[] valueIds = memeberConfig.Value.Split(',');
                        for (int i = 0; i < valueIds.Length; i++)
                        {
                            if (valueIds[i] != input.OpenId)
                            {
                                newValue += valueIds[i] + ",";
                            }
                            else
                            {
                                valueIds[i] = null;
                            }
                        }
                        if (newValue != null)
                        {
                            newValue = newValue.TrimStart(',').TrimEnd(',');
                        }
                    }
                    memeberConfig.Value = newValue;
                    memeberConfig.Desc = newDesc;
                    await _memberconfigRepository.UpdateAsync(memeberConfig);
                }
                return;

            }
            catch (Exception ex)
            {

                Logger.ErrorFormat("删除配置人员信息失败 error：{0} Exception：{1}", ex.Message, ex);
            }
        }
        /// 取消标签
        /// </summary>
        /// <param name="code"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task CancelTagAsync(UserTypeEnum code, string openId)
        {
            try
            {
                var wechatGroup = await _wechatGroupAppService.GetWeChatGroupByUserType(code);
                if (wechatGroup != null)
                {
                    List<string> openIds = new List<string>();
                    openIds.Add(openId);
                    await _wechatGroupAppService.CancelTagAsync(wechatGroup.TagId, openIds);
                }
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("取消标签失败,error:{0},Exception:{1}", e.Message, e);
            }
        }

        /// <summary>
        /// 获取店员信息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<List<WeChatUserListDto>> GetShopEmployeesAsync(int? tenantId, Guid userId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var result = await _wechatuserRepository.GetAll().Where(w => w.UserId == userId && w.UserType == UserTypeEnum.零售客户 && w.BindStatus == BindStatusEnum.已绑定).OrderByDescending(w => w.IsShopkeeper).ToListAsync();
                return result.MapTo<List<WeChatUserListDto>>();
            }

        }

        /// <summary>
        /// 审核店员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<APIResultDto> CheckShopEmployeeAsync(WeChatUserEditDto input)
        {
            input.BindTime = DateTime.Now;
            input.Status = UserAuditStatus.已审核;
            var entity = await _wechatuserRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);
            await _wechatuserRepository.UpdateAsync(entity);
            //反馈通知
            await WXMessageToShopKeeper(input.OpenId);
            return new APIResultDto() { Code = 0, Msg = "提交成功，我们会尽快处理" };
        }

        /// <summary>
        /// 反馈通知
        /// </summary>
        /// <param name="OpenId"></param>
        /// <returns></returns>
        public async Task WXMessageToShopKeeper(string OpenId)
        {
            try
            {
                string templateId = await _wechatappconfigRepository.GetAll().Select(v => v.TemplateIds).FirstOrDefaultAsync();
                if (templateId != null || templateId.Length != 0)
                {
                    string[] ids = templateId.Split(',');
                    string appId = AppConfig.AppId;
                    string openId = OpenId;
                    //string templateId = "7I2cswoMRn0P_DsAYz-DCigntaGKJn-XUx6lMowDYRY";//模版id  
                    string url = "";
                    object data = new
                    {
                        first = new TemplateDataItem("您已通过店员审核"),
                        keyword1 = new TemplateDataItem("通过审核"),
                        keyword2 = new TemplateDataItem(DateTime.Now.ToString("yyyy-MM-dd HH:mm"))
                    };
                    await TemplateApi.SendTemplateMessageAsync(appId, openId, ids[1], url, data);
                }
            }
            catch (Exception ex)
            {

                Logger.ErrorFormat("审核店员发送消息通知失败 error：{0} Exception：{1}", ex.Message, ex);
            }
        }


        /// <summary>
        /// 获取未审核店员人数
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<int> GetShopEmployeesNoCheckCountAsync(int? tenantId, Guid userId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var result = await _wechatuserRepository.GetAll().Where(w => w.UserId == userId && w.UserType == UserTypeEnum.零售客户 && w.BindStatus == BindStatusEnum.已绑定 && w.Status == UserAuditStatus.未审核).CountAsync();
                return result;
            }

        }

        /// <summary>
        /// 会员信息Excel导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(isTransactional: false)]
        public async Task<APIResultDto> ExportWeChatUsersExcel(GetWeChatUsersInput input)
        {
            try
            {
                var exportData = await GetWeChatUsersAsync(input);
                var result = new APIResultDto();
                result.Code = 0;
                result.Data = SaveWeChatUsersExcel("会员信息.xlsx", exportData);
                return result;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("ExportPostInfoExcel errormsg:{0} Exception:{1}", ex.Message, ex);
                return new APIResultDto() { Code = 901, Msg = "网络忙... 请待会重试！" };
            }
        }
        private async Task<List<WeChatUserListDto>> GetWeChatUsersAsync(GetWeChatUsersInput input)
        {
            //var mid = UserManager.GetControlEmployeeId();
            //var query = _wechatuserRepository.GetAll()
            //    .WhereIf(!string.IsNullOrEmpty(input.UserName), u => u.UserName.Contains(input.UserName))
            //    .WhereIf(!string.IsNullOrEmpty(input.Name), u => u.NickName.Contains(input.Name) || u.UserName.Contains(input.Name) || u.Phone.Contains(input.Name))
            //    .WhereIf(input.UserType.HasValue, u => u.UserType == input.UserType);
            //var user = await query
            //    .OrderBy(input.Sorting)
            //    .ToListAsync();

            var queryWe = _wechatuserRepository.GetAll()
               .WhereIf(!string.IsNullOrEmpty(input.UserName), u => u.UserName.Contains(input.UserName))
               .WhereIf(!string.IsNullOrEmpty(input.Name), u => u.NickName.Contains(input.Name) || u.UserName.Contains(input.Name) || u.Phone.Contains(input.Name))
               .WhereIf(input.UserType.HasValue, u => u.UserType == input.UserType);
            var queryRe = _retailerRepository.GetAll();
            var queryEm = _employeeRepository.GetAll();
            var query = await (from w in queryWe
                               join r in queryRe on w.UserId equals r.Id into wr
                               from rw in wr.DefaultIfEmpty()
                               join e in queryEm on w.UserId equals e.Id into we
                               from ew in we.DefaultIfEmpty()
                               select new WeChatUserListDto
                               {
                                   Id = w.Id,
                                   NickName = w.NickName,
                                   OpenId = w.OpenId,
                                   UserType = w.UserType,
                                   UserName = w.UserName,
                                   BindStatus = w.BindStatus,
                                   BindTime = w.BindTime,
                                   UnBindTime = w.UnBindTime,
                                   AttentionTime = w.AttentionTime,
                                   UnfollowTime = w.UnfollowTime,
                                   Phone = w.Phone,
                                   MemberBarCode = w.MemberBarCode,
                                   IntegralTotal = w.IntegralTotal,
                                   IsShopkeeper = w.IsShopkeeper,
                                   Status = w.Status,
                                   Code = rw != null ? rw.Code : (ew != null ? ew.Code : "")
                               }).WhereIf(!string.IsNullOrEmpty(input.Code), wre => wre.Code.Contains(input.Code)).ToListAsync();
            var UserDtos = query.MapTo<List<WeChatUserListDto>>();
            return UserDtos;
        }
        private string SaveWeChatUsersExcel(string fileName, List<WeChatUserListDto> data)
        {
            var fullPath = ExcelHelper.GetSavePath(_hostingEnvironment.WebRootPath) + fileName;
            using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("WeChatUser");
                var rowIndex = 0;
                IRow titleRow = sheet.CreateRow(rowIndex);
                string[] titles = { "微信OpenId", "微信昵称", "用户类型", "用户名", "绑定状态", "绑定时间", "解绑时间", "绑定电话", "会员卡条形码", "用户总积分", "是否是店主", "审核状态", "关注时间", "取消关注时间", "零售户/员工编码" };
                var fontTitle = workbook.CreateFont();
                fontTitle.IsBold = true;
                for (int i = 0; i < titles.Length; i++)
                {
                    var cell = titleRow.CreateCell(i);
                    cell.CellStyle.SetFont(fontTitle);
                    cell.SetCellValue(titles[i]);
                }

                var font = workbook.CreateFont();
                foreach (var item in data)
                {
                    rowIndex++;
                    IRow row = sheet.CreateRow(rowIndex);
                    ExcelHelper.SetCell(row.CreateCell(0), font, item.OpenId);
                    ExcelHelper.SetCell(row.CreateCell(1), font, item.NickName);
                    ExcelHelper.SetCell(row.CreateCell(2), font, item.UserTypeName);
                    ExcelHelper.SetCell(row.CreateCell(3), font, item.UserName);
                    ExcelHelper.SetCell(row.CreateCell(4), font, item.BindStatusName);
                    ExcelHelper.SetCell(row.CreateCell(5), font, item.BindTime.ToString());
                    ExcelHelper.SetCell(row.CreateCell(6), font, item.UnBindTime.ToString());
                    ExcelHelper.SetCell(row.CreateCell(7), font, item.Phone);
                    ExcelHelper.SetCell(row.CreateCell(8), font, item.MemberBarCode);
                    ExcelHelper.SetCell(row.CreateCell(9), font, item.IntegralTotal);
                    ExcelHelper.SetCell(row.CreateCell(10), font, item.IsShopkeeper.ToString());
                    ExcelHelper.SetCell(row.CreateCell(11), font, item.StatusName);
                    ExcelHelper.SetCell(row.CreateCell(12), font, item.AttentionTime.ToString());
                    ExcelHelper.SetCell(row.CreateCell(13), font, item.UnfollowTime.ToString());
                    ExcelHelper.SetCell(row.CreateCell(14), font, item.Code.ToString());
                }
                workbook.Write(fs);
            }
            return "/files/downloadtemp/" + fileName;
        }

        /// <summary>
        /// 查询微信用户联合零售户表和员工表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<WeChatUserListDto>> GetPagedWeChatUsersReEmAndRE(GetWeChatUsersInput input)
        {

            var queryWe = _wechatuserRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(input.UserName), u => u.UserName.Contains(input.UserName))
                .WhereIf(!string.IsNullOrEmpty(input.Name), u => u.NickName.Contains(input.Name) || u.UserName.Contains(input.Name) || u.Phone.Contains(input.Name))
                .WhereIf(input.UserType.HasValue, u => u.UserType == input.UserType);
            var queryRe = _retailerRepository.GetAll();
            var queryEm = _employeeRepository.GetAll();
            var query = (from w in queryWe
                         join r in queryRe on w.UserId equals r.Id into wr
                         from rw in wr.DefaultIfEmpty()
                         join e in queryEm on w.UserId equals e.Id into we
                         from ew in we.DefaultIfEmpty()
                         select new WeChatUserListDto
                         {
                             Id = w.Id,
                             NickName = w.NickName,
                             OpenId = w.OpenId,
                             UserType = w.UserType,
                             UserName = w.UserName,
                             BindStatus = w.BindStatus,
                             BindTime = w.BindTime,
                             UnBindTime = w.UnBindTime,
                             AttentionTime = w.AttentionTime,
                             UnfollowTime = w.UnfollowTime,
                             Phone = w.Phone,
                             Status = w.Status,
                             Code = rw != null ? rw.Code : (ew != null ? ew.Code : "")
                         }).WhereIf(!string.IsNullOrEmpty(input.Code), wre => wre.Code.Contains(input.Code));

            //TODO:根据传入的参数添加过滤条件
            var wechatuserCount = await query.CountAsync();

            var wechatusers = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            //var wechatuserListDtos = ObjectMapper.Map<List <WeChatUserListDto>>(wechatusers);
            var wechatuserListDtos = wechatusers.MapTo<List<WeChatUserListDto>>();

            return new PagedResultDto<WeChatUserListDto>(
                wechatuserCount,
                wechatuserListDtos
                );

        }

        /// <summary>
        /// 会员积分Excel导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(isTransactional: false)]
        public async Task<APIResultDto> ExportWeChatUsersIntegralExcelAsync(GetWeChatUsersInput input)
        {
            try
            {
                var exportData = await GetWeChatUsersIntegralAsync(input);
                var result = new APIResultDto();
                result.Code = 0;
                result.Data = SaveWeChatUsersIntegralExcel("会员积分.xlsx", exportData);
                return result;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("ExportPostInfoExcel errormsg:{0} Exception:{1}", ex.Message, ex);
                return new APIResultDto() { Code = 901, Msg = "网络忙... 请待会重试！" };
            }
        }

        private async Task<List<WeChatUserListDto>> GetWeChatUsersIntegralAsync(GetWeChatUsersInput input)
        {
            var queryIntegral = _wechatuserRepository.GetAll().Where(v => v.IntegralTotal > 0)
                .WhereIf(!string.IsNullOrEmpty(input.Name), v => v.NickName.Contains(input.Name))
                .WhereIf(input.UserType.HasValue, u => u.UserType == input.UserType)
                .WhereIf(!string.IsNullOrEmpty(input.Phone), u => u.Phone.Contains(input.Phone));
            var retailer = _retailerRepository.GetAll();
            var employee = _employeeRepository.GetAll();
            var query = (from w in queryIntegral
                         join r in retailer on w.UserId equals r.Id into wr
                         from table in wr.DefaultIfEmpty()
                         join e in employee on w.UserId equals e.Id into wre
                         from result in wre.DefaultIfEmpty()
                         select new WeChatUserListDto()
                         {
                             Id = w.Id,
                             OpenId = w.OpenId,
                             NickName = w.NickName,
                             UserType = w.UserType,
                             Code = table != null ? table.Code : (result != null ? result.Code : ""),
                             Phone = w.Phone,
                             IntegralTotal = w.IntegralTotal,
                             UserName = w.UserName
                         }).WhereIf(!string.IsNullOrEmpty(input.Code), v => v.Code.Contains(input.Code));
            if (input.SortValue != null && input.SortValue == "ascend")
            {
                var intergral = await query
                .OrderByDescending(v => v.IntegralTotal)
                .ThenBy(input.Sorting)
                .ToListAsync();
                var intergralListDtos = intergral.MapTo<List<WeChatUserListDto>>();
                return intergralListDtos;
            }
            else if (input.SortValue != null && input.SortValue == "descend")
            {
                var intergral = await query
                 .OrderBy(v => v.IntegralTotal)
                 .ThenBy(input.Sorting)
                 .ToListAsync();
                var intergralListDtos = intergral.MapTo<List<WeChatUserListDto>>();
                return intergralListDtos;
            }
            else
            {
                var intergral = await query
                .OrderByDescending(v => v.IntegralTotal)
                .ThenBy(input.Sorting)
                .ToListAsync();
                var intergralListDtos = intergral.MapTo<List<WeChatUserListDto>>();
                return intergralListDtos;
            }
        }

        private string SaveWeChatUsersIntegralExcel(string fileName, List<WeChatUserListDto> data)
        {
            var fullPath = ExcelHelper.GetSavePath(_hostingEnvironment.WebRootPath) + fileName;
            using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("WeChatUser");
                var rowIndex = 0;
                IRow titleRow = sheet.CreateRow(rowIndex);
                string[] titles = { "会员姓名", "用户类型", "用户名(编码)", "电话号码", "积分" };
                var fontTitle = workbook.CreateFont();
                fontTitle.IsBold = true;
                for (int i = 0; i < titles.Length; i++)
                {
                    var cell = titleRow.CreateCell(i);
                    cell.CellStyle.SetFont(fontTitle);
                    cell.SetCellValue(titles[i]);
                }

                var font = workbook.CreateFont();
                foreach (var item in data)
                {
                    rowIndex++;
                    IRow row = sheet.CreateRow(rowIndex);
                    ExcelHelper.SetCell(row.CreateCell(0), font, item.NickName);
                    ExcelHelper.SetCell(row.CreateCell(1), font, item.UserTypeName);
                    if (item.Code != "")
                    {
                        ExcelHelper.SetCell(row.CreateCell(2), font, item.UserName + "(" + item.Code + ")");
                    }
                    else
                    {
                        ExcelHelper.SetCell(row.CreateCell(2), font, item.UserName);
                    }
                    ExcelHelper.SetCell(row.CreateCell(3), font, item.Phone);
                    ExcelHelper.SetCell(row.CreateCell(4), font, item.IntegralTotal);
                }
                workbook.Write(fs);
            }

            return "/files/downloadtemp/" + fileName;
        }
        /// 微信用户统计（按零售户分公司统计）
        /// </summary>
        /// <returns></returns>
        public async Task<WeChatUserStatisticLiDto> GetWeChatUserStatistic()
        {
            //var weChat = _wechatuserRepository.GetAll().Where(w => w.UserType != UserTypeEnum.取消关注);
            //var retail = _retailerRepository.GetAll();
            var query = (from w in _wechatuserRepository.GetAll().Where(w => w.UserType != UserTypeEnum.取消关注)
                        join r in _retailerRepository.GetAll() on w.UserId equals r.Id into g
                        from wr in g.DefaultIfEmpty()
                        select new { wr.BranchCompany });
            //group new { wr.BranchCompany } by wr.BranchCompany into m
            //select new WeChatUserStatisticDto
            //{
            //    Company = m.Key, // == null ? "其它" : m.Key,
            //    Count = m.Count(),
            //GroupId = m.Key == null ? 2 : 1,
            //};

            //var total = await query.SumAsync(w => w.Count);
            var list = await query.GroupBy(q => q.BranchCompany).Select(g => new WeChatUserStatisticDto() { Company = g.Key, Count = g.Count() }).ToListAsync();
            var total = list.Sum(w => w.Count);
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.Company))
                {
                    item.Company = "其它";
                    item.GroupId = 1;
                    break;
                }
            }
            var result = new WeChatUserStatisticLiDto();
            result.WechatUserStaDto = list.OrderBy(l => l.GroupId).ThenByDescending(l => l.Count).ToList(); ;
            result.Total = total;
            return result;
        }

        /// <summary>
        /// 是否存在该微信用户
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<bool> GetWeChatUserIsExsit(string openId)
        {
            return await _wechatuserRepository.GetAll().AnyAsync(w => w.OpenId == openId && w.UserType!=UserTypeEnum.取消关注);
        }


        /// <summary>
        /// 判断用户是否关注
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<bool> GetIsAttentionByOpenIdAsync(string openId)
        {
            int weChat = await _wechatuserRepository.GetAll().Where(v=>v.OpenId == openId && v.UserType!= UserTypeEnum.取消关注).CountAsync();
            if (weChat == 0)
            {
                return false;
            }
            return true;
        }

        [AbpAllowAnonymous]
        [UnitOfWork(isTransactional: false)]
        public Task SubscribeAsync(string openId, string nickName, string headImgUrl, string scene, string ticket)
        {
            var ta = _wechatuserManager.SubscribeAsync(openId, nickName, headImgUrl, null, scene, ticket);
            return ta;
        }

        [AbpAllowAnonymous]
        public Task UnsubscribeAsync(string openId)
        {
            var ta = _wechatuserManager.UnsubscribeAsync(openId, null);
            return ta;
        }
    }
}


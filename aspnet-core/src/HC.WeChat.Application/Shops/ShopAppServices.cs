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
using HC.WeChat.Shops.Dtos;
using HC.WeChat.Shops.DomainServices;
using System;
using HC.WeChat.Retailers;
using HC.WeChat.WeChatUsers.DomainServices;
using HC.WeChat.WechatEnums;
using HC.WeChat.ShopProducts;
using HC.WeChat.Products;
using HC.WeChat.Helpers;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using HC.WeChat.WechatAppConfigs.Dtos;
using HC.WeChat.WechatAppConfigs;
using Senparc.Weixin.MP.AdvancedAPIs;
using HC.WeChat.MemberConfigs;
using HC.WeChat.WeChatUsers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Abp.Domain.Uow;
using HC.WeChat.Dto;
using Senparc.Weixin.MP;
using System.Net;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksum;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;
using SixLabors.ImageSharp.Processing.Drawing;
using SixLabors.Primitives;
using HC.WeChat.Authorization;
using HC.WeChat.Authorization.WeChatOAuth;
using QRCoder;
using SixLabors.ImageSharp.Processing.Text;
using Abp.Auditing;

namespace HC.WeChat.Shops
{
    /// <summary>
    /// Shop应用层服务的接口实现方法
    /// </summary>
    //[AbpAuthorize(ShopAppPermissions.Shop)]
    [AbpAuthorize(AppPermissions.Pages)]
    public class ShopAppService : WeChatAppServiceBase, IShopAppService
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        private readonly IRepository<Shop, Guid> _shopRepository;
        private readonly IShopManager _shopManager;
        private readonly IRepository<Retailer, Guid> _retailerRepository;
        private readonly IWeChatUserManager _wechatuserManager;
        private readonly IRepository<ShopProduct, Guid> _shopProductRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        IWechatAppConfigAppService _wechatAppConfigAppService;
        private readonly IRepository<MemberConfig, Guid> _memberconfigRepository;
        private readonly IRepository<WechatAppConfig, int> _wechatappconfigRepository;

        private int? TenantId { get; set; }
        private WechatAppConfigInfo AppConfig { get; set; }
        private readonly IRepository<WeChatUser, Guid> _wechatuserRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        IWeChatOAuthAppService _weChatOAuthAppService;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ShopAppService(IRepository<Shop, Guid> shopRepository
        , IShopManager shopManager, IRepository<Retailer, Guid> retailerRepository
        , IWeChatUserManager wechatuserManager
        , IRepository<ShopProduct, Guid> shopProductRepository
        , IRepository<Product, Guid> productRepository
            , IWechatAppConfigAppService wechatAppConfigAppService
         , IRepository<MemberConfig, Guid> memberconfigRepository
            , IRepository<WeChatUser, Guid> wechatuserRepository
            , IRepository<WechatAppConfig, int> wechatappconfigRepository
            , IHostingEnvironment hostingEnvironment, IWeChatOAuthAppService weChatOAuthAppService)
        {
            _shopRepository = shopRepository;
            _shopManager = shopManager;
            _retailerRepository = retailerRepository;
            _wechatuserManager = wechatuserManager;
            _shopProductRepository = shopProductRepository;
            _productRepository = productRepository;
            _wechatAppConfigAppService = wechatAppConfigAppService;
            TenantId = null;
            AppConfig = _wechatAppConfigAppService.GetWechatAppConfig(TenantId).Result;
            _memberconfigRepository = memberconfigRepository;
            _wechatuserRepository = wechatuserRepository;
            _wechatappconfigRepository = wechatappconfigRepository;
            _hostingEnvironment = hostingEnvironment;
            _weChatOAuthAppService = weChatOAuthAppService;
            _weChatOAuthAppService.WechatAppConfig = AppConfig;
        }

        /// <summary>
        /// 获取Shop的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ShopListDto>> GetPagedShops(GetShopsInput input)
        {

            var query = _shopRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(input.Name), s => s.Name.Contains(input.Name))
                .WhereIf(input.Status.HasValue, s => s.Status == input.Status);
            //TODO:根据传入的参数添加过滤条件
            var shopCount = await query.CountAsync();

            var shops = await query
                .OrderByDescending(s => s.CreationTime)
                .ThenBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            //var shopListDtos = ObjectMapper.Map<List <ShopListDto>>(shops);
            var shopListDtos = shops.MapTo<List<ShopListDto>>();

            return new PagedResultDto<ShopListDto>(
                shopCount,
                shopListDtos
                );

        }

        /// <summary>
        /// 通过指定id获取ShopListDto信息
        /// </summary>
        public async Task<ShopListDto> GetShopByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _shopRepository.GetAsync(input.Id);

            return entity.MapTo<ShopListDto>();
        }

        /// <summary>
        /// 导出Shop为excel表
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetShopsToExcel(){
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
        public async Task<GetShopForEditOutput> GetShopForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetShopForEditOutput();
            ShopEditDto shopEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _shopRepository.GetAsync(input.Id.Value);

                shopEditDto = entity.MapTo<ShopEditDto>();

                //shopEditDto = ObjectMapper.Map<List <shopEditDto>>(entity);
            }
            else
            {
                shopEditDto = new ShopEditDto();
            }

            output.Shop = shopEditDto;
            return output;

        }

        /// <summary>
        /// 添加或者修改Shop的公共方法
        /// </summary>
        public async Task CreateOrUpdateShop(CreateOrUpdateShopInput input)
        {
            if (input.Shop.Id.HasValue)
            {
                await UpdateShopAsync(input);
            }
            else
            {
                await CreateShopAsync(input.Shop);
            }
        }

        [AbpAllowAnonymous]
        public async Task WechatCreateOrUpdateShop(CreateOrUpdateShopInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                //更新
                if (input.Shop.Id.HasValue)
                {
                    input.Shop.Status = WechatEnums.ShopAuditStatus.待审核;
                    await UpdateShopAsync(input);
                }
                else//新增
                {
                    var user = await _wechatuserRepository.GetAll().Where(w => w.OpenId == input.OpenId).FirstOrDefaultAsync();//_wechatuserManager.GetWeChatUserAsync(input.OpenId, input.TenantId);
                    input.Shop.TenantId = input.TenantId;
                    input.Shop.RetailerId = user.UserId;
                    input.Shop.Status = WechatEnums.ShopAuditStatus.待审核;
                    input.Shop.ReadTotal = 0;
                    input.Shop.SaleTotal = 0;
                    input.Shop.SingleTotal = 0;
                    input.Shop.Evaluation = "0,0,0";
                    input.Shop.FansNum = 0;
                    var entity = await CreateShopAsync(input.Shop);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    input.Shop.Id = entity.Id;//获取审核信息的shop Id

                    await SendAuditNotice(input);
                }
            }
        }
        /// <summary>
        /// 发送审核通知
        /// </summary>
        private async Task SendAuditNotice(CreateOrUpdateShopInput input)
        {
            try
            {
                string templateId = await _wechatappconfigRepository.GetAll().Select(v => v.TemplateIds).FirstOrDefaultAsync();
                if (templateId != null || templateId.Length != 0)
                {
                    string[] ids = templateId.Split(',');
                    //发送微信模板通知-后台配置内部员工
                    string memberConfig = await _memberconfigRepository.GetAll().Where(v => v.Code == DeployCodeEnum.通知配置).Select(v => v.Value).FirstOrDefaultAsync();
                    if (memberConfig.Length != 0 || memberConfig != null)
                    {
                        var openIdIds = memberConfig.Split(',');
                        if (openIdIds.Length != 0)
                        {
                            foreach (var item in openIdIds)
                            {
                                string appId = AppConfig.AppId;
                                string openId = item;
                                //string templateId = "qvt7CNXBY4FzfzdX54TvMUaOi9jZ3-tdsb2NRhVp0yg";//模版id  
                                input.Host = input.Host ?? "http://ga.intcov.com";//host配置
                                string url = input.Host + "/GAWX/Authorization?page=303&param=" + input.Shop.Id.ToString();
                                object data = new
                                {
                                    first = new TemplateDataItem("有店铺资料提交或更改，请您尽快审核", "#FF0000"),
                                    keyword1 = new TemplateDataItem(input.Shop.Name.ToString()),
                                    keyword2 = new TemplateDataItem(DateTime.Now.ToString("yyyy-MM-dd HH:mm"))
                                };
                                await TemplateApi.SendTemplateMessageAsync(appId, openId, ids[2], url, data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("店铺审核通知失败 error：{0} Exception：{1}", ex.Message, ex);
            }
        }


        /// <summary>
        /// 新增Shop
        /// </summary>
        //[AbpAuthorize(ShopAppPermissions.Shop_CreateShop)]

        [AbpAllowAnonymous]
        protected virtual async Task<ShopEditDto> CreateShopAsync(ShopEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var entity = ObjectMapper.Map<Shop>(input);

            entity = await _shopRepository.InsertAsync(entity);
            return entity.MapTo<ShopEditDto>();
        }

        /// <summary>
        /// 编辑Shop
        /// </summary>
        //[AbpAuthorize(ShopAppPermissions.Shop_EditShop)]
        [AbpAllowAnonymous]
        protected virtual async Task UpdateShopAsync(CreateOrUpdateShopInput input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            var entity = await _shopRepository.GetAsync(input.Shop.Id.Value);
            var orgStatus = entity.Status;
            input.Shop.MapTo(entity);
            // ObjectMapper.Map(input, entity);
            await _shopRepository.UpdateAsync(entity);
            //当审核状态改为待审核
            if ((orgStatus == ShopAuditStatus.已审核 || orgStatus == ShopAuditStatus.已拒绝) && input.Shop.Status == ShopAuditStatus.待审核)
            {
                //发送审核通知
                await SendAuditNotice(input);
            }
        }

        /// <summary>
        /// 删除Shop信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[AbpAuthorize(ShopAppPermissions.Shop_DeleteShop)]
        public async Task DeleteShop(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _shopRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除Shop的方法
        /// </summary>
        //[AbpAuthorize(ShopAppPermissions.Shop_BatchDeleteShops)]
        public async Task BatchDeleteShopsAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _shopRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        /// <summary>
        /// 添加或者修改Shop的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateShopDto(ShopEditDto input)
        {
            var entity = new ShopEditDto();
            if (input.Id.HasValue)
            {
                await UpdateShopAsync(new CreateOrUpdateShopInput() { Shop = input });
            }
            else
            {
                await CreateShopAsync(input);
            }
            //return await GetShopByIdRetailerAsync(entity.Id);

        }

        /// <summary>
        /// 获取Shop的分页列表信息(连接零售客户表)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ShopListDto>> GetPagedShopsByRetailer(GetShopsInput input)
        {
            var mid = UserManager.GetControlEmployeeId();
            var queryShop = _shopRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(input.Name), s => s.Name.Contains(input.Name))
                .WhereIf(input.Status.HasValue, s => s.Status == input.Status)
                .WhereIf(!string.IsNullOrEmpty(input.Tel), s => s.Tel.Contains(input.Tel));
            var queryRetailer = _retailerRepository.GetAll()
                .WhereIf(mid.HasValue, r => r.EmployeeId == mid)
                .WhereIf(!string.IsNullOrEmpty(input.RetailCode), r => r.Code.Contains(input.RetailCode));
            var query = from s in queryShop
                        join r in queryRetailer on s.RetailerId equals r.Id
                        //into queryS
                        //from sr in queryS.DefaultIfEmpty()
                        select new ShopListDto
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Address = s.Address,
                            Desc = s.Desc,
                            RetailerId = s.RetailerId,
                            CoverPhoto = s.CoverPhoto,
                            SaleTotal = s.SaleTotal,
                            ReadTotal = s.ReadTotal,
                            Evaluation = s.Evaluation,
                            Longitude = s.Longitude,
                            Latitude = s.Latitude,
                            Status = s.Status,
                            AuditTime = s.AuditTime,
                            CreationTime = s.CreationTime,
                            TenantId = s.TenantId,
                            Tel = s.Tel,
                            SingleTotal = s.SingleTotal,
                            //RetailerName = r != null ? r.Name : "",
                            RetailerName = r.Name,
                            RetailerCode = r.Code,
                            QRUrl = s.QRUrl,
                            FansNum = s.FansNum
                        };

            //TODO:根据传入的参数添加过滤条件
            var shopCount = await query.CountAsync();
            if (input.SortSaleTotal != null && input.SortSaleTotal == "ascend")
            {
                var shops = await query
                    .OrderByDescending(s => s.SaleTotal)
                    .ThenBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                //var shopListDtos = ObjectMapper.Map<List <ShopListDto>>(shops);
                var shopListDtos = shops.MapTo<List<ShopListDto>>();

                return new PagedResultDto<ShopListDto>(
                    shopCount,
                    shopListDtos
                    );
            }
            else if (input.SortSaleTotal != null && input.SortSaleTotal == "descend")
            {
                var shops = await query
                    .OrderBy(s => s.SaleTotal)
                    .ThenBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                //var shopListDtos = ObjectMapper.Map<List <ShopListDto>>(shops);
                var shopListDtos = shops.MapTo<List<ShopListDto>>();

                return new PagedResultDto<ShopListDto>(
                    shopCount,
                    shopListDtos
                    );
            }
            else if (input.SortReadTotal != null && input.SortReadTotal == "ascend")
            {
                var shops = await query
                    .OrderByDescending(s => s.ReadTotal)
                    .ThenBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                //var shopListDtos = ObjectMapper.Map<List <ShopListDto>>(shops);
                var shopListDtos = shops.MapTo<List<ShopListDto>>();

                return new PagedResultDto<ShopListDto>(
                    shopCount,
                    shopListDtos
                    );
            }
            else if (input.SortReadTotal != null && input.SortReadTotal == "descend")
            {
                var shops = await query
                  .OrderBy(s => s.ReadTotal)
                  .ThenBy(input.Sorting)
                  .PageBy(input)
                  .ToListAsync();

                //var shopListDtos = ObjectMapper.Map<List <ShopListDto>>(shops);
                var shopListDtos = shops.MapTo<List<ShopListDto>>();

                return new PagedResultDto<ShopListDto>(
                    shopCount,
                    shopListDtos
                    );
            }
            else if (input.SortSingleTotal != null && input.SortSingleTotal == "ascend")
            {
                var shops = await query
                  .OrderByDescending(s => s.SingleTotal)
                  .ThenBy(input.Sorting)
                  .PageBy(input)
                  .ToListAsync();

                //var shopListDtos = ObjectMapper.Map<List <ShopListDto>>(shops);
                var shopListDtos = shops.MapTo<List<ShopListDto>>();

                return new PagedResultDto<ShopListDto>(
                    shopCount,
                    shopListDtos
                    );
            }
            else if (input.SortSingleTotal != null && input.SortSingleTotal == "descend")
            {
                var shops = await query
                  .OrderBy(s => s.SingleTotal)
                  .ThenBy(input.Sorting)
                  .PageBy(input)
                  .ToListAsync();

                //var shopListDtos = ObjectMapper.Map<List <ShopListDto>>(shops);
                var shopListDtos = shops.MapTo<List<ShopListDto>>();

                return new PagedResultDto<ShopListDto>(
                    shopCount,
                    shopListDtos
                    );
            }
            else if (input.SortFansTotal != null && input.SortFansTotal == "ascend")
            {
                var shops = await query
                  .OrderByDescending(s => s.FansNum)
                  .ThenBy(input.Sorting)
                  .PageBy(input)
                  .ToListAsync();

                //var shopListDtos = ObjectMapper.Map<List <ShopListDto>>(shops);
                var shopListDtos = shops.MapTo<List<ShopListDto>>();

                return new PagedResultDto<ShopListDto>(
                    shopCount,
                    shopListDtos
                    );
            }
            else if (input.SortFansTotal != null && input.SortFansTotal == "descend")
            {
                var shops = await query
                  .OrderBy(s => s.FansNum)
                  .ThenBy(input.Sorting)
                  .PageBy(input)
                  .ToListAsync();

                //var shopListDtos = ObjectMapper.Map<List <ShopListDto>>(shops);
                var shopListDtos = shops.MapTo<List<ShopListDto>>();

                return new PagedResultDto<ShopListDto>(
                    shopCount,
                    shopListDtos
                    );
            }
            else
            {
                var shops = await query
                .OrderByDescending(s => s.CreationTime)
                .ThenBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

                //var shopListDtos = ObjectMapper.Map<List <ShopListDto>>(shops);
                var shopListDtos = shops.MapTo<List<ShopListDto>>();

                return new PagedResultDto<ShopListDto>(
                    shopCount,
                    shopListDtos
                    );
            }
        }

        /// <summary>
        /// 获取单个店铺信息（连接零售客户表）
        /// </summary>
        /// <param name="input">零售户Id</param>
        /// <returns></returns>
        public async Task<ShopListDto> GetShopByIdRetailerAsync(Guid? id)
        {
            var queryShop = _shopRepository.GetAll()
                .Where(s => s.Id == id);
            //当店铺二维码不存在时，去新生成二维码
            //var shop = queryShop.SingleOrDefault();
            //if (string.IsNullOrEmpty(shop.WechatUrl))
            //{
            //    var qrResult = await GenerateShopCodeAsync(shop.Id);
            //}
            var queryRetailer = _retailerRepository.GetAll();
            var entity = await (from s in queryShop
                                join r in queryRetailer on s.RetailerId equals r.Id into queryS
                                from sr in queryS.DefaultIfEmpty()
                                select new ShopListDto
                                {
                                    Id = s.Id,
                                    Name = s.Name,
                                    Address = s.Address,
                                    Desc = s.Desc,
                                    RetailerId = s.RetailerId,
                                    CoverPhoto = s.CoverPhoto,
                                    SaleTotal = s.SaleTotal,
                                    ReadTotal = s.ReadTotal,
                                    Evaluation = s.Evaluation,
                                    Longitude = s.Longitude,
                                    Latitude = s.Latitude,
                                    QqLatitude = s.QqLatitude,
                                    QqLongitude = s.QqLongitude,
                                    Status = s.Status,
                                    AuditTime = s.AuditTime,
                                    CreationTime = s.CreationTime,
                                    TenantId = s.TenantId,
                                    Tel = s.Tel,
                                    RetailerName = sr != null ? sr.Name : "",
                                    QRUrl = s.QRUrl,
                                    FansNum = s.FansNum
                                }).SingleOrDefaultAsync();
            //当店铺二维码不存在时，去新生成二维码
            if (string.IsNullOrEmpty(entity.QRUrl) && entity.Status == ShopAuditStatus.已审核)
            {
                var qrResult = await GenerateShopCodeAsync(entity.Id);
                entity.WechatUrl = qrResult.Url;
                entity.Ticket = qrResult.Ticket;
                entity.QRUrl = qrResult.QRUrl;
            }
            return entity;
        }


        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<ShopListDto> GetShopByOpenId(int? tenantId, string openId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var user = await _wechatuserRepository.GetAll().Where(w => w.OpenId == openId).FirstOrDefaultAsync();//_wechatuserManager.GetWeChatUserAsync(openId, tenantId);
                var shop = await _shopRepository.GetAll().Where(s => s.RetailerId == user.UserId).FirstOrDefaultAsync();
                return shop.MapTo<ShopListDto>();
            }
        }

        /// <summary>
        /// 店铺审核
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task CheckShop(CheckShopDto input)
        {
            var entity = await _shopRepository.GetAsync(input.Id);
            entity.Status = input.Status;
            entity.AuditTime = DateTime.Now;
            entity.Reason = input.Reason;
            var result = await _shopRepository.UpdateAsync(entity);
            if (result.Status == ShopAuditStatus.已审核 && string.IsNullOrEmpty(result.QRUrl))
            {
                await GenerateShopCodeAsync(result.Id);
            }
            //审核通知
            var ShopOpenId = await _wechatuserRepository.GetAll().Where(r => r.UserId == entity.RetailerId).Select(v => v.OpenId).FirstOrDefaultAsync();
            try
            {
                string templateId = await _wechatappconfigRepository.GetAll().Select(v => v.TemplateIds).FirstOrDefaultAsync();
                if (templateId != null || templateId.Length != 0)
                {
                    string[] ids = templateId.Split(',');
                    if (input.Status == ShopAuditStatus.已审核)
                    {
                        string appId = AppConfig.AppId;
                        string openId = ShopOpenId;
                        //string templateId = "7I2cswoMRn0P_DsAYz-DCigntaGKJn-XUx6lMowDYRY";//模版id  
                        string url = "";
                        object data = new
                        {
                            first = new TemplateDataItem("您所提交的店铺资料已通过审核"),
                            keyword1 = new TemplateDataItem("通过审核"),
                            keyword2 = new TemplateDataItem(DateTime.Now.ToString("yyyy-MM-dd HH:mm"))
                        };
                        await TemplateApi.SendTemplateMessageAsync(appId, openId, ids[1], url, data);
                    }
                    else
                    {
                        string appId = AppConfig.AppId;
                        string openId = ShopOpenId;
                        //string templateId = "n325dGQOYvNMZ46eFDIlFo5jWXSr-P3jNMDubXZ3Sbw";//模版id  
                        string url = "";
                        object data = new
                        {
                            keyword1 = new TemplateDataItem("审核未通过"),
                            keyword2 = new TemplateDataItem(DateTime.Now.ToString("yyyy-MM-dd HH:mm")),
                            keyword3 = new TemplateDataItem(string.Format("未通过原因：{0},请修改重新提交", input.Reason)),
                        };
                        await TemplateApi.SendTemplateMessageAsync(appId, openId, ids[3], url, data);
                    }
                }
            }
            catch (Exception ex)
            {

                Logger.ErrorFormat("审核通知发送失败 error：{0} Exception：{1}", ex.Message, ex);
            }

            //return result.MapTo<ShopEditDto>();
        }

        /// <summary>
        /// 根据当前位置获取附近店铺
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<List<NearbyShopDto>> GetNearbyShopByLocationAsync(double latitude, double longitude, int? tenantId, string openId)
        {
            var mbr = new MapMBR(latitude, longitude, 3.1);//确定搜索范围3.1公里 搜索范围扩大0.1公里
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                //演示注释 后面需要放开
                //根据经纬度范围过滤数据
                var dataList = await _shopRepository.GetAll()
                    .Where(s => s.Status == ShopAuditStatus.已审核
                    && s.Latitude > mbr.MinLatitude
                    && s.Latitude < mbr.MaxLatitude
                    && s.Longitude > mbr.MinLongitude
                    && s.Longitude < mbr.MaxLongitude).ToListAsync();

                var resultList = dataList.MapTo<List<NearbyShopDto>>();
                foreach (var item in resultList)
                {
                    if (item.Latitude.HasValue && item.Longitude.HasValue)
                    {
                        item.Distance = Math.Round(AbpMapByGoogle.GetDistance(latitude, longitude, item.Latitude.Value, item.Longitude.Value), 0);//不保留小数
                    }
                    else
                    {
                        item.Distance = 4000;//后面会被过滤
                    }
                }

                //var resultList = (await _shopRepository.GetAll().Where(s => s.Status == ShopAuditStatus.已审核).ToListAsync()).MapTo<List<NearbyShopDto>>();
                //int[] rd = { 92, 108, 201, 255, 374, 488, 509 };
                //int i = 0;
                //foreach (var item in resultList)
                //{
                //    item.Distance = 100 + rd[i];
                //    i++;
                //    if (i == rd.Length)
                //    {
                //        i = 0;
                //    }
                //}

                return resultList.Where(r => r.Distance <= 3000).OrderBy(r => r.Distance).ToList();
            }
        }

        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<List<NearbyShopDto>> GetShopListByGoodsIdAsync(int? tenantId, Guid goodsId, double latitude, double longitude)
        {
            var mbr = new MapMBR(latitude, longitude, 3.1);//确定搜索范围3.1公里 搜索范围扩大0.1公里
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var product = await _productRepository.GetAsync(goodsId);
                product.SearchCount = product.SearchCount ?? 0;
                product.SearchCount++;
                var shopIds = await _shopProductRepository.GetAll()
                    .Where(s => s.ProductId == goodsId)
                    .Select(s => s.ShopId).ToArrayAsync();
                //var shops = await _shopRepository.GetAll().Where(s => shopIds.Contains(s.Id)).ToListAsync();
                //根据经纬度范围过滤数据
                var dataList = await _shopRepository.GetAll()
                    .Where(s => s.Status == ShopAuditStatus.已审核
                    && shopIds.Contains(s.Id)
                    && s.Latitude > mbr.MinLatitude
                    && s.Latitude < mbr.MaxLatitude
                    && s.Longitude > mbr.MinLongitude
                    && s.Longitude < mbr.MaxLongitude).ToListAsync();

                var resultList = dataList.MapTo<List<NearbyShopDto>>();
                foreach (var item in resultList)
                {
                    if (item.Latitude.HasValue && item.Longitude.HasValue)
                    {
                        item.Distance = Math.Round(AbpMapByGoogle.GetDistance(latitude, longitude, item.Latitude.Value, item.Longitude.Value), 0);//不保留小数
                    }
                    else
                    {
                        item.Distance = 4000;//后面会被过滤
                    }
                }
                return resultList.Where(r => r.Distance <= 3000).OrderBy(r => r.Distance).ToList();
            }
        }

        public async Task<HomeInfo> GetHomeInfo()
        {
            HomeInfo homeInfo = new HomeInfo();

            homeInfo.ShopCount = await _shopRepository.GetAll().Where(s => s.Status == ShopAuditStatus.已审核).CountAsync();
            //homeInfo.PendingShopCount = await _shopRepository.GetAll().Where(s => s.Status == ShopAuditStatus.待审核).CountAsync();
            var goodsCount = await _productRepository.GetAll().SumAsync(s => s.SearchCount);
            homeInfo.GoodsSearchCount = goodsCount.HasValue ? goodsCount.Value : 0;
            homeInfo.IntegralTotal = await _wechatuserRepository.GetAll().SumAsync(u => u.IntegralTotal);
            homeInfo.WechatUserCount = await _wechatuserRepository.GetAll().Where(u => u.UserType != UserTypeEnum.取消关注).CountAsync();

            return homeInfo;
        }

        public async Task<APIResultDto> GetPendingShopList()
        {
            var query = _shopRepository.GetAll().Where(s => s.Status == ShopAuditStatus.待审核);
            var count = await query.CountAsync();
            var shopList = await query.OrderByDescending(s => s.CreationTime).Take(5).ToListAsync();
            return new APIResultDto()
            {
                Code = 0,
                Data = new { ShopList = shopList.MapTo<List<ShopListDto>>(), Count = count }
            };
        }

        #region 店铺导出

        /// <summary>
        /// 获取Excel数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<List<ShopListDto>> GeShopNoPage(GetShopsInput input)
        {
            var mid = UserManager.GetControlEmployeeId();
            var queryShop = _shopRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(input.Name), s => s.Name.Contains(input.Name))
                .WhereIf(input.Status.HasValue, s => s.Status == input.Status)
                .WhereIf(!string.IsNullOrEmpty(input.Tel), s => s.Tel.Contains(input.Tel));
            var queryRetailer = _retailerRepository.GetAll().WhereIf(mid.HasValue, r => r.EmployeeId == mid);
            var query = from s in queryShop
                        join r in queryRetailer on s.RetailerId equals r.Id
                        //into queryS
                        //from sr in queryS.DefaultIfEmpty()
                        select new ShopListDto
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Address = s.Address,
                            Desc = s.Desc,
                            RetailerId = s.RetailerId,
                            CoverPhoto = s.CoverPhoto,
                            SaleTotal = s.SaleTotal,
                            ReadTotal = s.ReadTotal,
                            Evaluation = s.Evaluation,
                            Longitude = s.Longitude,
                            Latitude = s.Latitude,
                            Status = s.Status,
                            AuditTime = s.AuditTime,
                            CreationTime = s.CreationTime,
                            TenantId = s.TenantId,
                            Tel = s.Tel,
                            SingleTotal = s.SingleTotal,
                            //RetailerName = r != null ? r.Name : "",
                            RetailerName = r.Name,
                            RetailerCode = r.Code,
                            FansNum = s.FansNum,
                            BranchCompany = r.BranchCompany,
                            Manager = r.Manager,
                            Area = r.Area
                        };

            //TODO:根据传入的参数添加过滤条件
            //var shopCount = await query.CountAsync();
            if (input.SortSaleTotal != null && input.SortSaleTotal == "ascend")
            {
                var shops = await query
                    .OrderByDescending(s => s.SaleTotal)
                    .ThenBy(input.Sorting)
                    .ToListAsync();
                var shopListDtos = shops.MapTo<List<ShopListDto>>();
                return shopListDtos;
            }
            else if (input.SortSaleTotal != null && input.SortSaleTotal == "descend")
            {
                var shops = await query
                    .OrderBy(s => s.SaleTotal)
                    .ThenBy(input.Sorting)
                    .ToListAsync();
                var shopListDtos = shops.MapTo<List<ShopListDto>>();
                return shopListDtos;
            }
            else if (input.SortReadTotal != null && input.SortReadTotal == "ascend")
            {
                var shops = await query
                    .OrderByDescending(s => s.ReadTotal)
                    .ThenBy(input.Sorting)
                    .ToListAsync();
                var shopListDtos = shops.MapTo<List<ShopListDto>>();
                return shopListDtos;
            }
            else if (input.SortReadTotal != null && input.SortReadTotal == "descend")
            {
                var shops = await query
                    .OrderBy(s => s.ReadTotal)
                    .ThenBy(input.Sorting)
                    .ToListAsync();
                var shopListDtos = shops.MapTo<List<ShopListDto>>();
                return shopListDtos;
            }
            else if (input.SortSingleTotal != null && input.SortSingleTotal == "ascend")
            {
                var shops = await query
                    .OrderByDescending(s => s.SingleTotal)
                    .ThenBy(input.Sorting)
                    .ToListAsync();
                var shopListDtos = shops.MapTo<List<ShopListDto>>();
                return shopListDtos;
            }
            else if (input.SortSingleTotal != null && input.SortSingleTotal == "descend")
            {
                var shops = await query
                    .OrderBy(s => s.SingleTotal)
                    .ThenBy(input.Sorting)
                    .ToListAsync();
                var shopListDtos = shops.MapTo<List<ShopListDto>>();
                return shopListDtos;
            }
            else if (input.SortFansTotal != null && input.SortFansTotal == "ascend")
            {
                var shops = await query
                    .OrderByDescending(s => s.FansNum)
                    .ThenBy(input.Sorting)
                    .ToListAsync();
                var shopListDtos = shops.MapTo<List<ShopListDto>>();
                return shopListDtos;
            }
            else if (input.SortFansTotal != null && input.SortFansTotal == "descend")
            {
                var shops = await query
                    .OrderBy(s => s.FansNum)
                    .ThenBy(input.Sorting)
                    .ToListAsync();
                var shopListDtos = shops.MapTo<List<ShopListDto>>();
                return shopListDtos;
            }
            else
            {
                var shops = await query
                              .OrderByDescending(s => s.CreationTime)
                              .ThenBy(input.Sorting)
                              .ToListAsync();
                var shopListDtos = shops.MapTo<List<ShopListDto>>();
                return shopListDtos;
            }
        }

        /// <summary>
        /// 创建Excel
        /// </summary>
        /// <param name="fileName">表名</param>
        /// <param name="data">表数据</param>
        /// <returns></returns>
        private string SaveShopExcel(string fileName, List<ShopListDto> data)
        {
            var fullPath = ExcelHelper.GetSavePath(_hostingEnvironment.WebRootPath) + fileName;
            using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Employees");
                var rowIndex = 0;
                IRow titleRow = sheet.CreateRow(rowIndex);
                string[] titles = { "店铺名称", "店铺地址", "店铺描述", "零售客户", "客户编码", "客户经理", "单位", "店铺销量", "店铺浏览量", "店铺用户量", "粉丝数", "店铺电话", "审核状态", "审核时间", "店铺评价", "经度", "纬度", "片区" };
                var fontTitle = workbook.CreateFont();
                fontTitle.IsBold = true;
                for (int i = 0; i < titles.Length; i++)
                {
                    var cell = titleRow.CreateCell(i);
                    cell.CellStyle.SetFont(fontTitle);
                    cell.SetCellValue(titles[i]);
                    //ExcelHelper.SetCell(titleRow.CreateCell(i), fontTitle, titles[i]);
                }
                var font = workbook.CreateFont();
                foreach (var item in data)
                {

                    rowIndex++;
                    var evaluationStr = "";
                    if (string.IsNullOrEmpty(item.Evaluation))
                    {
                        var assess = item.Evaluation.Split(",");
                        evaluationStr = "好评:" + assess[0] + "中评:" + assess[1] + "差评:" + assess[2];
                    }
                    IRow row = sheet.CreateRow(rowIndex);
                    ExcelHelper.SetCell(row.CreateCell(0), font, item.Name);
                    ExcelHelper.SetCell(row.CreateCell(1), font, item.Address);
                    ExcelHelper.SetCell(row.CreateCell(2), font, item.Desc);
                    ExcelHelper.SetCell(row.CreateCell(3), font, item.RetailerName);
                    ExcelHelper.SetCell(row.CreateCell(4), font, item.RetailerCode);
                    ExcelHelper.SetCell(row.CreateCell(5), font, item.Manager);
                    ExcelHelper.SetCell(row.CreateCell(6), font, item.BranchCompany);
                    ExcelHelper.SetCell(row.CreateCell(7), font, item.SaleTotal.ToString());
                    ExcelHelper.SetCell(row.CreateCell(8), font, item.ReadTotal.ToString());
                    ExcelHelper.SetCell(row.CreateCell(9), font, item.SingleTotal.ToString());
                    ExcelHelper.SetCell(row.CreateCell(10), font, item.FansNum.ToString());
                    ExcelHelper.SetCell(row.CreateCell(11), font, item.Tel);
                    ExcelHelper.SetCell(row.CreateCell(12), font, item.StatusName);
                    ExcelHelper.SetCell(row.CreateCell(13), font, item.AuditTime.ToString());
                    ExcelHelper.SetCell(row.CreateCell(14), font, evaluationStr);
                    ExcelHelper.SetCell(row.CreateCell(15), font, item.Longitude.ToString());
                    ExcelHelper.SetCell(row.CreateCell(16), font, item.Latitude.ToString());
                    ExcelHelper.SetCell(row.CreateCell(17), font, item.Area);

                }
                workbook.Write(fs);
            }
            return "/files/downloadtemp/" + fileName;
        }

        /// <summary>
        /// 导出员工Excel
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns></returns>
        [UnitOfWork(isTransactional: false)]
        public async Task<APIResultDto> ExportShopExcel(GetShopsInput input)
        {
            try
            {
                var exportData = await GeShopNoPage(input);
                var result = new APIResultDto();
                result.Code = 0;
                result.Data = SaveShopExcel("店铺.xlsx", exportData);
                return result;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("ExportShopExcel errormsg{0} Exception{1}", ex.Message, ex);
                return new APIResultDto() { Code = 901, Msg = "网络忙...请待会儿再试！" };
            }
        }

        #endregion

        #region 生成店铺二维码

        /// <summary>
        /// 批量生成二维码
        /// </summary>
        /// <returns></returns>
        public async Task BatchCreateQRCodeAsync()
        {
            var shops = await _shopRepository.GetAll().ToListAsync();
            foreach (var item in shops)
            {
                if (string.IsNullOrEmpty(item.QRUrl) && item.Status == ShopAuditStatus.已审核)
                {
                    //生成二维码 
                    var retailer = await _retailerRepository.GetAll().Where(r => r.Id == item.RetailerId).SingleOrDefaultAsync();
                    var result = await QrCodeApi.CreateAsync(AppConfig.AppId, 0, 0, QrCode_ActionName.QR_LIMIT_STR_SCENE, (int)SceneType.店铺 + "_" + item.Id.ToString());

                    //下载二维码到本地
                    var imgurl = QrCodeApi.GetShowQrCodeUrl(result.ticket);
                    var imgeName = retailer.Code + retailer.Name;
                    var img = ImgeFilesDonload(imgurl, imgeName, "shopqr");

                    //更新二维码数据到数据库
                    item.Ticket = result.ticket;
                    item.WechatUrl = result.url;
                    item.QRUrl = img;
                    _shopRepository.Update(item);
                }
            }
        }

        /// <summary>
        /// 生成店码
        /// </summary>
        public async Task<CreateQRResult> GenerateShopCodeAsync(Guid shopId)
        {
            //生成二维码
            var qrResult = await QrCodeApi.CreateAsync(AppConfig.AppId, 0, 0, QrCode_ActionName.QR_LIMIT_STR_SCENE, (int)SceneType.店铺 + "_" + shopId.ToString());
            var shop = await _shopRepository.GetAll().Where(s => s.Id == shopId).SingleOrDefaultAsync();
            var retailer = await _retailerRepository.GetAll().Where(r => r.Id == shop.RetailerId).SingleOrDefaultAsync();

            //下载二维码到本地
            var imgurl = QrCodeApi.GetShowQrCodeUrl(qrResult.ticket);
            var imgeName = retailer.Code + retailer.Name;
            var img = ImgeFilesDonload(imgurl, imgeName, "shopqr");

            //更新二维码数据到数据库
            shop.Ticket = qrResult.ticket;
            shop.WechatUrl = qrResult.url;
            shop.QRUrl = img;

            //生成二维码结果
            var result = new CreateQRResult();
            result.Ticket = qrResult.ticket;
            result.Url = qrResult.url;
            result.QRUrl = img;

            return result;
        }

        /// <summary>
        /// 二维码保存到本地
        /// </summary>
        /// <param name="url">下载地址</param>
        /// <param name="imgName">图片名</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public string ImgeFilesDonload(string url, string imgName, string fileName)
        {
            WebClient web = new WebClient();
            string html = web.DownloadString(url);
            var location = "";
            string webRootPath = _hostingEnvironment.WebRootPath;
            var endRote = string.Format("/upload/{0}/", fileName);
            var fileDire = webRootPath + endRote;
            if (!Directory.Exists(fileDire))
            {
                Directory.CreateDirectory(fileDire);
            }

            var filePath = fileDire + imgName + ".jpg";
            web.DownloadFile(url, filePath);
            location = filePath.Substring(webRootPath.Length);
            imgName = filePath.Substring(fileDire.Length);
            var imgPath = QrcodeImgLogo(filePath, imgName);
            return imgPath;
        }
        #endregion

        [UnitOfWork(isTransactional: false)]
        public APIResultDto PromotionCodeZip(GetShopsInput input)
        {
            try
            {
                var exportData = PackageImgeFilesDownload(input.Url, input.FileName);
                var result = new APIResultDto();
                result.Code = 0;
                result.Data = exportData;
                return result;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("ExportShopExcel errormsg{0} Exception{1}", ex.Message, ex);
                return new APIResultDto() { Code = 901, Msg = "网络忙...请待会儿再试！" };
            }
        }
        /// <summary>
        /// 上传至服务器并压缩Zip
        /// </summary>
        /// <param name="remoteUrl"></param>
        /// <param name="imgName"></param>
        /// <returns></returns>
        private string PackageImgeFilesDownload(string remoteUrl, string imgName)
        {
            remoteUrl = remoteUrl.TrimStart(',').TrimEnd(',');
            imgName = imgName.TrimStart(',').TrimEnd(',');
            string fileName = "PromotionCode";
            string downFileName = "downloadtemp";
            string webRootPath = _hostingEnvironment.WebRootPath;
            var endRote = string.Format("/upload/{0}/", fileName);
            var endRoteZip = string.Format("/upload/{0}", fileName);
            var downEndRoteZip = string.Format("/files/{0}/", downFileName);
            var fileDire = webRootPath + endRote;
            var fileDireZip = webRootPath + endRoteZip;
            var downFileDireZip = webRootPath + downEndRoteZip;
            if (Directory.Exists(fileDire))
            {
                Directory.Delete(fileDire, true);// 先删除之前的目录
            }
            if (remoteUrl.Contains(','))
            {
                string[] urlIds = remoteUrl.Split(',');
                string[] nameIds = imgName.Split(',');
                int i = 0;
                foreach (var item in urlIds)
                {
                    WebClient web = new WebClient();
                    string html = web.DownloadString(webRootPath + item);
                    if (!Directory.Exists(fileDire))
                    {
                        Directory.CreateDirectory(fileDire);
                    }
                    var filePath = fileDire + nameIds[i] + ".jpg";
                    web.DownloadFile(webRootPath + item, filePath);
                    i++;
                }
                ZipFileDirectory(fileDireZip, "店铺推广码.zip", downFileDireZip);
                return "/files/downloadtemp/店铺推广码.zip";
            }
            else
            {
                WebClient web = new WebClient();
                string html = web.DownloadString(webRootPath + remoteUrl);
                if (!Directory.Exists(fileDire))
                {
                    Directory.CreateDirectory(fileDire);
                }
                var filePath = fileDire + imgName + ".jpg";
                web.DownloadFile(webRootPath + remoteUrl, filePath);
                ZipFileDirectory(fileDireZip, "店铺推广码.zip", downFileDireZip);
                return "/files/downloadtemp/店铺推广码.zip";
            }
        }
        public static void ZipFileDirectory(string strDirectory, string zipedFile, string downFileName)
        {
            //如果目录不存在，则报错
            if (!System.IO.Directory.Exists(strDirectory))
            {
                throw new System.IO.FileNotFoundException("指定的目录: " + strDirectory + " 不存在!");
            }
            using (System.IO.FileStream ZipFile = System.IO.File.Create(downFileName + zipedFile))
            {
                using (ZipOutputStream s = new ZipOutputStream(ZipFile))
                {
                    ZipSetp(strDirectory, s, "");
                }
            }
        }

        /// <summary>
        /// 递归遍历目录
        /// add yuangang by 2016-06-13
        /// </summary>
        private static void ZipSetp(string strDirectory, ZipOutputStream s, string parentPath)
        {
            if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
            {
                strDirectory += Path.DirectorySeparatorChar;
            }
            Crc32 crc = new Crc32();

            string[] filenames = Directory.GetFileSystemEntries(strDirectory);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {

                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string pPath = parentPath;
                    pPath += file.Substring(file.LastIndexOf("\\") + 1);
                    pPath += "\\";
                    ZipSetp(file, s, pPath);
                }

                else // 否则直接压缩文件
                {
                    //打开压缩文件
                    using (FileStream fs = File.OpenRead(file))
                    {

                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);

                        string fileName = parentPath + file.Substring(file.LastIndexOf("\\") + 1);
                        ZipEntry entry = new ZipEntry(fileName);

                        entry.DateTime = DateTime.Now;
                        entry.Size = fs.Length;
                        entry.IsUnicodeText = true;
                        fs.Close();

                        crc.Reset();
                        crc.Update(buffer);

                        entry.Crc = crc.Value;
                        s.PutNextEntry(entry);

                        s.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }


        /// <summary>
        /// 二维码添加logo
        /// </summary>
        /// <param name="filePath">二维码路径</param>
        /// <param name="imgName">图片名称</param>
        /// <returns></returns>
        public string QrcodeImgLogo(string filePath, string imgName)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            using (FileStream stream = File.OpenRead(filePath))
            using (Image<Rgba32> logo = Image.Load(webRootPath + "/upload/shopqr/logo.jpg"))
            using (Image<Rgba32> image = Image.Load(stream))
            {
                logo.Mutate(x => x
                     .Resize(90, 90));
                image.Mutate(x => x.DrawImage(logo, PixelBlenderMode.Src, 1, new Point(image.Width / 2 - 45, image.Height / 2 - 45)));
                var endRote = string.Format("/upload/{0}/", "qrlogo");
                var fileDire = webRootPath + endRote;
                if (!Directory.Exists(fileDire))
                {
                    Directory.CreateDirectory(fileDire);
                }
                var path = fileDire + imgName;
                image.Save(path);
                var imgPath = path.Substring(webRootPath.Length);
                return imgPath;
            }
        }

        /// <summary>
        /// 微信获取店铺推广码url
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<string> GetQRUrlByShopId(Guid shopId)
        {
            string url = await _shopRepository.GetAll().Where(v => v.Id == shopId).Select(v => v.QRUrl).FirstOrDefaultAsync();
            return url;
        }
        #region 店铺数据统计

        /// <summary>
        /// 店铺入驻数统计（按分公司统计）
        /// </summary>
        /// <returns></returns>
        public async Task<ShopStatisticLiDto> GetShopStatisticsByCompany()
        {
            //var shop = _shopRepository.GetAll().Where(s => s.Status == ShopAuditStatus.已审核);
            //var retail = _retailerRepository.GetAll();
            var query = (from s in _shopRepository.GetAll().Where(s => s.Status == ShopAuditStatus.已审核)
                         join r in _retailerRepository.GetAll() on s.RetailerId equals r.Id
                         select new { r.BranchCompany });
            //into g
            //from sr in g.DefaultIfEmpty()
            //where sr.BranchCompany != null
            //group new { r.BranchCompany } by r.BranchCompany into m
            //select new ShopStatisticDto
            //{
            //    Company = m.Key,//== null ? "其它" : m.Key,
            //    Count = m.Count(),
            //GroupId = m.Key != null ? 1 : 2
            //});
            //var total = await query.SumAsync(s => s.Count);
            var list = await query.GroupBy(q => q.BranchCompany).Select(q => new ShopStatisticDto() { Company = q.Key, Count = q.Count() }).ToListAsync();
            var total = list.Sum(s => s.Count);
            //foreach (var item in list)
            //{
            //    if (string.IsNullOrEmpty(item.Company))
            //    {
            //        item.Company = "其它";
            //        item.GroupId = 1;
            //        break;
            //    }
            //}
            var result = new ShopStatisticLiDto();
            result.ShopStaDto = list.OrderByDescending(l => l.Count).ToList();
            result.Total = total;
            return result;
        }

        #endregion

        /// <summary>
        /// 批量压缩图片
        /// </summary>
        public Task<APIResultDto> BatchCompressionPictures(string fromPath, string toPaht, int type, int val)
        {
            if (!Directory.Exists(fromPath))
            {
                return Task.FromResult(new APIResultDto() { Code = 701, Msg = string.Format("fromPath:{0}不存在", fromPath) });
            }

            if (!Directory.Exists(toPaht))
            {
                return Task.FromResult(new APIResultDto() { Code = 702, Msg = string.Format("toPaht:{0}不存在", toPaht) });
            }

            DirectoryInfo dinfo = new DirectoryInfo(fromPath);
            foreach (var item in dinfo.GetFiles())
            {
                using (Image<Rgba32> image = Image.Load(item.OpenRead()))
                {
                    var fileName = item.Name.Split('.')[0] + ".jpg";
                    if (type == 1)//按宽度值压缩
                    {
                        if (image.Width > val)
                        {
                            var height = (int)((val / image.Width) * image.Height);
                            image.Mutate(x => x.Resize(val, height));
                            image.Save(toPaht + "/" + fileName);
                        }
                        else
                        {
                            image.Save(toPaht + "/" + fileName);
                        }
                    }
                    else //按高度值压缩
                    {
                        if (image.Height > val)
                        {
                            var width = (int)((val / image.Height) * image.Width);
                            image.Mutate(x => x.Resize(width, val));
                            image.Save(toPaht + "/" + fileName);
                        }
                        else
                        {
                            image.Save(toPaht + "/" + fileName);
                        }
                    }

                }
            }

            return Task.FromResult(new APIResultDto() { Code = 1, Msg = "压缩图片成功" });
        }

        /// <summary>
        /// 获取进入店铺二维码url
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public string GetQrCodeUrl(Guid shopId, string host)
        {
            var url = host + "/GAWX/ShopAuth";
            var qrUrl = _weChatOAuthAppService.GetAuthorizeUrl(url, shopId.ToString(), OAuthScope.snsapi_base);
            return qrUrl;
        }
        /// <summary>
        /// 获取店铺推广码（关注公众号二维码）
        /// </summary>
        /// <param name="shopId">店铺id</param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<string> GetShopQrCodeURL(Guid shopId)
        {
            return await _shopRepository.GetAll().Where(s => s.Id == shopId).Select(s => s.QRUrl).FirstOrDefaultAsync();
        }

        [AbpAllowAnonymous]
        public Task<APIResultDto> GenerateShopQrCode(List<ShopUrlQrCodeDto> input)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            foreach (var item in input)
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(string.Format("http://ga.intcov.com/GAWX/ShopAuth?state={0}", item.Id), QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                System.Drawing.Bitmap qrCodeImage = qrCode.GetGraphic(20);
                MemoryStream stream = new MemoryStream();
                qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                //using (FileStream stream = File.OpenRead(filePath))
                using (Image<Rgba32> logo = Image.Load(webRootPath + "/upload/shopqr/logo.jpg"))
                using (Image<Rgba32> qrimage = Image.Load(stream))
                using (Image<Rgba32> orgimage = new Image<Rgba32>(SixLabors.ImageSharp.Configuration.Default, 430, 520, Rgba32.White))
                {
                    //画文字
                    var fontCollection = new SixLabors.Fonts.FontCollection();
                    //var fontPath = "C:/Windows/Fonts/simkai.ttf";
                    //var fontPath = "C:/Windows/Fonts/STXINWEI.TTF";
                    var fontPath = "C:/Windows/Fonts/simkai.ttf";
                    var font = new SixLabors.Fonts.Font(fontCollection.Install(fontPath), 20f, SixLabors.Fonts.FontStyle.Bold);
                    orgimage.Mutate(x => x.DrawText("单    位: " + item.Company, font, Rgba32.Black, new PointF(36, 20)));
                    orgimage.Mutate(x => x.DrawText("客户姓名: " + item.UserName, font, Rgba32.Black, new PointF(36, 45)));
                    orgimage.Mutate(x => x.DrawText("编    码: " + item.UserCode, font, Rgba32.Black, new PointF(36, 70)));
                    //画logo
                    logo.Mutate(x => x.Resize(180, 180));
                    qrimage.Mutate(x => x.DrawImage(logo, PixelBlenderMode.Src, 1, new Point(qrimage.Width / 2 - 90, qrimage.Height / 2 - 90)));
                    //画二维码
                    qrimage.Mutate(x => x.Resize(430, 430));
                    orgimage.Mutate(x => x.DrawImage(qrimage, PixelBlenderMode.Src, 1, new Point(0, 90)));
                    var endRote = "/upload/shopUrlQrCodes/";
                    var fileDire = webRootPath + endRote;
                    if (!Directory.Exists(fileDire))
                    {
                        Directory.CreateDirectory(fileDire);
                    }
                    var path = fileDire + item.UserCode + '-' + item.UserName + ".jpg";
                    orgimage.Save(path);
                }
            }
            return Task.FromResult(new APIResultDto() { Code = 0, Msg = "生成成功" });
        }

        /// <summary>
        /// 判断用户是否是当前店铺店主or管理员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<bool> GetIsCurShopKeeper(GetShopsInput input)
        {
            var sRId = await _shopRepository.GetAll().Where(v => v.Id == input.ShopId).Select(v => v.RetailerId).FirstOrDefaultAsync();
            var uRId = await _wechatuserRepository.GetAll().Where(v => v.OpenId == input.OpenId).Select(v => v.UserId).FirstOrDefaultAsync();
            if (sRId == uRId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 判断用户是否在扫码范围
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<bool> GetShopWithRangeAsync(double latitude, double longitude, Guid shopId)
        {
            var config = await _memberconfigRepository.GetAll().Where(v => v.Type == DeployTypeEnum.扫码限制配置 && v.Code == DeployCodeEnum.店铺距离).Select(v => v.Value).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(config))
            {
                config = "200";
            }
            var entity = await _shopRepository.GetAll().Where(s => s.Id == shopId).FirstOrDefaultAsync();
            var result = entity.MapTo<NearbyShopDto>();
            //Logger.Info($"店铺位置信息{result}");
            if (result.Latitude.HasValue && result.Longitude.HasValue)
            {
                result.Distance = Math.Round(AbpMapByGoogle.GetDistance(latitude, longitude, result.Latitude.Value, result.Longitude.Value), 0);//不保留小数
                //Logger.Info($"店铺位置信息{result.Distance}");

                if (result.Distance <= Convert.ToDouble(config))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}


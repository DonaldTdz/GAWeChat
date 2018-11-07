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
using HC.WeChat.ExhibitionShops.Authorization;
using HC.WeChat.ExhibitionShops.DomainServices;
using HC.WeChat.ExhibitionShops.Dtos;
using System;
using HC.WeChat.Authorization;
using HC.WeChat.Shops;
using HC.WeChat.Retailers;
using HC.WeChat.Exhibitions.Dtos;
using HC.WeChat.Exhibitions;
using HC.WeChat.Dto;
using Abp.Domain.Uow;
using HC.WeChat.Helpers;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using HC.WeChat.Authorization.WeChatOAuth;
using HC.WeChat.WechatAppConfigs;
using Abp.Auditing;

namespace HC.WeChat.ExhibitionShops
{
    /// <summary>
    /// ExhibitionShop应用层服务的接口实现方法
    /// </summary>
    //[AbpAuthorize(ExhibitionShopAppPermissions.ExhibitionShop)]
    [AbpAuthorize(AppPermissions.Pages)]
    public class ExhibitionShopAppService : WeChatAppServiceBase, IExhibitionShopAppService
    {
        private readonly IRepository<ExhibitionShop, Guid> _exhibitionshopRepository;
        private readonly IExhibitionShopManager _exhibitionshopManager;
        private readonly IRepository<Shop, Guid> _shopRepository;
        private readonly IRepository<Retailer, Guid> _retailerRepository;
        private readonly IRepository<Exhibition, Guid> _exhibitionRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IWeChatOAuthAppService _weChatOAuthAppService;
        private readonly IWechatAppConfigAppService _wechatAppConfigAppService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ExhibitionShopAppService(
            IRepository<ExhibitionShop, Guid> exhibitionshopRepository
      , IExhibitionShopManager exhibitionshopManager
            , IRepository<Shop, Guid> shopRepository
            , IRepository<Retailer, Guid> retailerRepository
            , IRepository<Exhibition, Guid> exhibitionRepository
                , IHostingEnvironment hostingEnvironment
            , IWeChatOAuthAppService weChatOAuthAppService
            , IWechatAppConfigAppService wechatAppConfigAppService
        )
        {
            _retailerRepository = retailerRepository;
            _shopRepository = shopRepository;
            _exhibitionshopRepository = exhibitionshopRepository;
            _exhibitionshopManager = exhibitionshopManager;
            _exhibitionRepository = exhibitionRepository;
            _hostingEnvironment = hostingEnvironment;
            _weChatOAuthAppService = weChatOAuthAppService;
            _wechatAppConfigAppService = wechatAppConfigAppService;
            _weChatOAuthAppService.WechatAppConfig = _wechatAppConfigAppService.GetWechatAppConfig(null).Result;
        }


        /// <summary>
        /// 获取ExhibitionShop的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ExhibitionShopListDto>> GetPagedExhibitionShops(GetExhibitionShopsInput input)
        {

            var query = _exhibitionshopRepository.GetAll();
            //TODO:根据传入的参数添加过滤条件

            var exhibitionshopCount = await query.CountAsync();

            var exhibitionshops = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            //var exhibitionshopListDtos = ObjectMapper.Map<List <ExhibitionShopListDto>>(exhibitionshops);
            var exhibitionshopListDtos = exhibitionshops.MapTo<List<ExhibitionShopListDto>>();

            return new PagedResultDto<ExhibitionShopListDto>(
                exhibitionshopCount,
                exhibitionshopListDtos
                );
        }

        /// <summary>
        /// 通过指定id获取ExhibitionShopListDto信息
        /// </summary>
        public async Task<ExhibitionShopListDto> GetExhibitionShopByIdAsync(Guid id)
        {
            //var entity = await _exhibitionshopRepository.GetAsync(id);
            //return entity.MapTo<ExhibitionShopListDto>();
            var exhibitons = _exhibitionshopRepository.GetAll().Where(v => v.Id == id);
            var retailer = _retailerRepository.GetAll();
            var shop = _shopRepository.GetAll();
            var result = (from e in exhibitons
                          join r in retailer on e.RetailerId equals r.Id
                          join s in shop on e.ShopId equals s.Id
                          select new ExhibitionShopListDto()
                          {
                              Id = e.Id,
                              ShopName = e.ShopName,
                              CustCode = r.Code,
                              CustName = r.Name,
                              Area = r.Area,
                              ShopAddress = e.ShopAddress,
                              Phone = s.Tel,
                              Votes = e.Votes != null ? e.Votes : 0,
                              FansNum = s.FansNum,
                              PicPath = e.PicPath,
                              NoId =e.NoId
                          });
            return await result.FirstOrDefaultAsync();
        }


        /// <summary>
        /// 导出ExhibitionShop为excel表
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetExhibitionShopsToExcel(){

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
        public async Task<GetExhibitionShopForEditOutput> GetExhibitionShopForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetExhibitionShopForEditOutput();
            ExhibitionShopEditDto exhibitionshopEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _exhibitionshopRepository.GetAsync(input.Id.Value);

                exhibitionshopEditDto = entity.MapTo<ExhibitionShopEditDto>();

                //exhibitionshopEditDto = ObjectMapper.Map<List <exhibitionshopEditDto>>(entity);


            }
            else
            {
                exhibitionshopEditDto = new ExhibitionShopEditDto();
            }

            output.ExhibitionShop = exhibitionshopEditDto;
            return output;

        }


        /// <summary>
        /// 添加或者修改ExhibitionShop的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateExhibitionShop(CreateOrUpdateExhibitionShopInput input)
        {

            if (input.ExhibitionShop.Id.HasValue)
            {
                await UpdateExhibitionShopAsync(input.ExhibitionShop);
            }
            else
            {
                await CreateExhibitionShopAsync(input.ExhibitionShop);
            }
        }

        /// <summary>
        /// 新增ExhibitionShop
        /// </summary>
        //[AbpAuthorize(ExhibitionShopAppPermissions.ExhibitionShop_CreateExhibitionShop)]
        protected virtual async Task<ExhibitionShopEditDto> CreateExhibitionShopAsync(ExhibitionShopEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<ExhibitionShop>(input);

            entity = await _exhibitionshopRepository.InsertAsync(entity);
            return entity.MapTo<ExhibitionShopEditDto>();
        }

        /// <summary>
        /// 编辑ExhibitionShop
        /// </summary>
        //[AbpAuthorize(ExhibitionShopAppPermissions.ExhibitionShop_EditExhibitionShop)]
        protected virtual async Task UpdateExhibitionShopAsync(ExhibitionShopEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _exhibitionshopRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _exhibitionshopRepository.UpdateAsync(entity);
        }




        /// <summary>
        /// 删除ExhibitionShop信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(ExhibitionShopAppPermissions.ExhibitionShop_DeleteExhibitionShop)]
        public async Task DeleteExhibitionShop(EntityDto<Guid> input)
        {

            //TODO:删除前的逻辑判断，是否允许删除
            await _exhibitionshopRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除ExhibitionShop的方法
        /// </summary>
        [AbpAuthorize(ExhibitionShopAppPermissions.ExhibitionShop_BatchDeleteExhibitionShops)]
        public async Task BatchDeleteExhibitionShopsAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _exhibitionshopRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 分页获取陈列活动列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ExhibitionShopListDto>> GetPagedExhibitionShopsAsync(GetExhibitionShopsInput input)
        {
            var exhibitons = _exhibitionshopRepository.GetAll()
                 .WhereIf(!string.IsNullOrEmpty(input.ShopName), v => v.ShopName.Contains(input.ShopName) || v.NoId.ToString().Contains(input.ShopName));
            var retailer = _retailerRepository.GetAll();
            var shop = _shopRepository.GetAll();
            var result = (from e in exhibitons
                          join r in retailer on e.RetailerId equals r.Id
                          join s in shop on e.ShopId equals s.Id
                          select new ExhibitionShopListDto()
                          {
                              Id = e.Id,
                              ShopName = e.ShopName,
                              CustCode = r.Code,
                              CustName = r.Name,
                              Area = r.Area,
                              ShopAddress = e.ShopAddress,
                              Phone = s.Tel,
                              Votes = e.Votes != null ? e.Votes : 0,
                              FansNum = s.FansNum,
                              NoId =e.NoId
                          }).WhereIf(!string.IsNullOrEmpty(input.Phone), v => v.Phone.Contains(input.Phone))
                 .WhereIf(!string.IsNullOrEmpty(input.CustCode), v => v.CustCode.Contains(input.CustCode))
                 .WhereIf(!string.IsNullOrEmpty(input.CustName), v => v.CustName.Contains(input.CustName));
            var exhibitionshopCount = await result.CountAsync();
            if (input.SortFansTotal != null && input.SortFansTotal == "ascend")
            {
                var exhibitionshops = await result
                    .OrderByDescending(v => v.FansNum).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();
                var exhibitionshopListDtos = exhibitionshops.MapTo<List<ExhibitionShopListDto>>();
                return new PagedResultDto<ExhibitionShopListDto>(
                    exhibitionshopCount,
                    exhibitionshopListDtos
                    );
            }
            else if (input.SortFansTotal != null && input.SortFansTotal == "descend")
            {
                var exhibitionshops = await result
                    .OrderBy(v => v.FansNum).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();
                var exhibitionshopListDtos = exhibitionshops.MapTo<List<ExhibitionShopListDto>>();
                return new PagedResultDto<ExhibitionShopListDto>(
                    exhibitionshopCount,
                    exhibitionshopListDtos
                    );
            }
            else if (input.SortVotesTotal != null && input.SortVotesTotal == "ascend")
            {
                var exhibitionshops = await result
                    .OrderByDescending(v => v.Votes).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

                var exhibitionshopListDtos = exhibitionshops.MapTo<List<ExhibitionShopListDto>>();
                return new PagedResultDto<ExhibitionShopListDto>(
                    exhibitionshopCount,
                    exhibitionshopListDtos
                    );
            }

            else if (input.SortVotesTotal != null && input.SortVotesTotal == "descend")
            {
                var exhibitionshops = await result
                    .OrderBy(v => v.Votes).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

                var exhibitionshopListDtos = exhibitionshops.MapTo<List<ExhibitionShopListDto>>();
                return new PagedResultDto<ExhibitionShopListDto>(
                    exhibitionshopCount,
                    exhibitionshopListDtos
                    );
            }
            else
            {
                var exhibitionshops = await result
                    .OrderBy(v => v.NoId).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();
                var exhibitionshopListDtos = exhibitionshops.MapTo<List<ExhibitionShopListDto>>();
                return new PagedResultDto<ExhibitionShopListDto>(
                    exhibitionshopCount,
                    exhibitionshopListDtos
                    );
            }
        }

        /// <summary>
        /// 根据id获取陈列活动详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ExhibitionShopListDto> GetPagedExhibitionShopsByIdAsync(Guid id)
        {
            var exhibitons = _exhibitionshopRepository.GetAll().Where(v => v.Id == id);
            var retailer = _retailerRepository.GetAll();
            var shop = _shopRepository.GetAll();
            var result = (from e in exhibitons
                          join r in retailer on e.RetailerId equals r.Id
                          join s in shop on e.ShopId equals s.Id
                          select new ExhibitionShopListDto()
                          {
                              Id = e.Id,
                              ShopName = e.ShopName,
                              CustCode = r.Code,
                              CustName = r.Name,
                              Area = r.Area,
                              ShopAddress = e.ShopAddress,
                              Phone = s.Tel,
                              Votes = e.Votes != null ? e.Votes : 0,
                              FansNum = s.FansNum.Value,
                              PicPath = e.PicPath
                          }).FirstOrDefaultAsync();
            return await result;
        }

        /// <summary>
        /// 通过指定id获取ExhibitionListDto信息
        /// </summary>
        private async Task<ExhibitionListDto> GetExhibitionByIdAsync()
        {
            var entity = await _exhibitionRepository.GetAll().FirstOrDefaultAsync();
            return entity.MapTo<ExhibitionListDto>();
        }
        /// <summary>
        /// 陈列活动Excel导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(isTransactional: false)]
        public async Task<APIResultDto> ExportExhibitionShopsExcel(GetExhibitionShopsInput input)
        {
            try
            {
                var exportData = await GetExhibitionShopsAsync(input);
                var result = new APIResultDto();
                result.Code = 0;
                result.Data = SaveExhibitionShopsAsyncsExcel("陈列店铺.xlsx", exportData);
                return result;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("ExportPostInfoExcel errormsg:{0} Exception:{1}", ex.Message, ex);
                return new APIResultDto() { Code = 901, Msg = "网络忙... 请待会重试！" };
            }
        }
        private async Task<List<ExhibitionShopListDto>> GetExhibitionShopsAsync(GetExhibitionShopsInput input)
        {
            var exhibitons = _exhibitionshopRepository.GetAll()
                 .WhereIf(!string.IsNullOrEmpty(input.ShopName), v => v.ShopName.Contains(input.ShopName)||v.NoId.ToString().Contains(input.ShopName));           
            var retailer = _retailerRepository.GetAll();
            var shop = _shopRepository.GetAll();
            var result = (from e in exhibitons
                          join r in retailer on e.RetailerId equals r.Id
                          join s in shop on e.ShopId equals s.Id
                          select new ExhibitionShopListDto()
                          {
                              Id = e.Id,
                              ShopName = e.ShopName,
                              CustCode = r.Code,
                              CustName = r.Name,
                              Area = r.Area,
                              ShopAddress = e.ShopAddress,
                              Phone = s.Tel,
                              Votes = e.Votes != null ? e.Votes : 0,
                              FansNum = s.FansNum,
                              NoId = e.NoId
                          }).WhereIf(!string.IsNullOrEmpty(input.Phone), v => v.Phone.Contains(input.Phone))
                 .WhereIf(!string.IsNullOrEmpty(input.CustCode), v => v.CustCode.Contains(input.CustCode))
                 .WhereIf(!string.IsNullOrEmpty(input.CustName), v => v.CustName.Contains(input.CustName));
            if (input.SortFansTotal != null && input.SortFansTotal == "ascend")
            {
                var exhibitionshops = await result
                    .OrderByDescending(v => v.FansNum).AsNoTracking()
                    .ToListAsync();
                var exhibitionshopListDtos = exhibitionshops.MapTo<List<ExhibitionShopListDto>>();
                return exhibitionshopListDtos;
            }
            else if (input.SortFansTotal != null && input.SortFansTotal == "descend")
            {
                var exhibitionshops = await result
                      .OrderBy(v => v.FansNum).AsNoTracking()
                      .ToListAsync();
                var exhibitionshopListDtos = exhibitionshops.MapTo<List<ExhibitionShopListDto>>();
                return exhibitionshopListDtos;
            }
            else if (input.SortVotesTotal != null && input.SortVotesTotal == "ascend")
            {
                var exhibitionshops = await result
                         .OrderByDescending(v => v.Votes).AsNoTracking()
                         .ToListAsync();
                var exhibitionshopListDtos = exhibitionshops.MapTo<List<ExhibitionShopListDto>>();
                return exhibitionshopListDtos;
            }
            else if (input.SortVotesTotal != null && input.SortVotesTotal == "descend")
            {
                var exhibitionshops = await result
                          .OrderBy(v => v.Votes).AsNoTracking()
                          .ToListAsync();
                var exhibitionshopListDtos = exhibitionshops.MapTo<List<ExhibitionShopListDto>>();
                return exhibitionshopListDtos;
            }
            else
            {
                var exhibitionshops = await result
                        .OrderBy(v => v.NoId).AsNoTracking()
                        .ToListAsync();
                var exhibitionshopListDtos = exhibitionshops.MapTo<List<ExhibitionShopListDto>>();
                return exhibitionshopListDtos;
            }
        }

        private string SaveExhibitionShopsAsyncsExcel(string fileName, List<ExhibitionShopListDto> data)
        {
            var fullPath = ExcelHelper.GetSavePath(_hostingEnvironment.WebRootPath) + fileName;
            using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("WeChatUser");
                var rowIndex = 0;
                IRow titleRow = sheet.CreateRow(rowIndex);
                string[] titles = { "参赛编号","零售客户编码", "客户名称", "店铺名称", "所属区县", "店铺地址", "店铺电话", "实时票数", "店铺会员数" };
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
                    ExcelHelper.SetCell(row.CreateCell(0), font, item.NoId);
                    ExcelHelper.SetCell(row.CreateCell(1), font, item.CustCode);
                    ExcelHelper.SetCell(row.CreateCell(2), font, item.CustName);
                    ExcelHelper.SetCell(row.CreateCell(3), font, item.ShopName);
                    ExcelHelper.SetCell(row.CreateCell(4), font, item.Area);
                    ExcelHelper.SetCell(row.CreateCell(5), font, item.ShopAddress);
                    ExcelHelper.SetCell(row.CreateCell(6), font, item.Phone);
                    ExcelHelper.SetCell(row.CreateCell(7), font, item.Votes.ToString());
                    ExcelHelper.SetCell(row.CreateCell(8), font, item.FansNum.ToString());
                }
                workbook.Write(fs);
            }
            return "/files/downloadtemp/" + fileName;
        }

        /// <summary>
        /// 获取陈列活动列表
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<List<ExhibitionViewDto>> GetWXPagedExhibitionShopsAsync(string type)
        {
            var config = await GetExhibitionByIdAsync();
            var exhibitons = _exhibitionshopRepository.GetAll().Where(v => v.Status == 1);
            var result = from e in exhibitons
                         select new ExhibitionWechatDto()
                         {
                             Id = e.Id,
                             ShopName = e.ShopName,
                             Votes = e.Votes ?? 0,
                             PicPath = e.PicPath,
                             ShopId = e.ShopId,
                             CreateTime = e.CreateTime,
                             NoId =e.NoId
                         };
            var resultList = new List<ExhibitionWechatDto>();
            if (type == "time")
            {
                resultList = await result.Take(config.TopTotal).OrderByDescending(v => v.CreateTime).ToListAsync();
            }
            else if (type == "vote")
            {
                resultList = await result.Take(config.TopTotal).OrderByDescending(v => v.Votes).ToListAsync();
            }
            else
            {
                resultList = await result.Take(config.TopTotal).OrderBy(v => v.NoId).ToListAsync();
            }

            var resultViewList = new List<ExhibitionViewDto>();
            ExhibitionViewDto view = new ExhibitionViewDto();
            int i = 0;
            foreach (var item in resultList)
            {
                item.ShopName = item.ShopName.Length > 10 ? item.ShopName.Substring(0, 10) + "..." : item.ShopName;
                view.Items.Add(item);
                if (i%2 == 1)
                {
                    resultViewList.Add(view);
                    view = new ExhibitionViewDto();
                }
                i++;
            }

            if (view.Items.Count() > 0)
            {
                resultViewList.Add(view);
            }

            return resultViewList;
        }

        /// <summary>
        /// 获取参加活动店铺数
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<int> GetWXExhibitionShopsCountAsync()
        {
            int total = await _exhibitionshopRepository.GetAll().CountAsync();
            return total;
        }

        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<List<ExhibitionShopListDto>> GetExhibitionShopByKeyAsync(string key)
        {
            var exhibitons = _exhibitionshopRepository.GetAll().Where(p => p.ShopName.Contains(key)||p.NoId.ToString().Equals(key));
            var result = from e in exhibitons
                         select new ExhibitionShopListDto()
                         {
                             Id = e.Id,
                             ShopName = e.ShopName,
                             Votes = e.Votes ?? 0,
                             PicPath = e.PicPath,
                             ShopId = e.ShopId,
                             NoId =e.NoId
                         };
            return await result.Take(5).ToListAsync();
        }

        /// <summary>
        /// 微信端根据id获取陈列店铺资料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<ExhibitionShopListDto> GetWXExhibitionShopsByIdAsync(Guid shopId)
        {
            var exhibitons = _exhibitionshopRepository.GetAll().Where(v => v.ShopId == shopId);
            var retailer = _retailerRepository.GetAll();
            var shop = _shopRepository.GetAll();
            var result = (from e in exhibitons
                          join r in retailer on e.RetailerId equals r.Id
                          join s in shop on e.ShopId equals s.Id
                          select new ExhibitionShopListDto()
                          {
                              Id = e.Id,
                              ShopName = e.ShopName,
                              CustCode = r.Code,
                              CustName = r.Name,
                              Area = r.Area,
                              ShopAddress = e.ShopAddress,
                              Phone = s.Tel,
                              Votes = e.Votes != null ? e.Votes : 0,
                              FansNum = s.FansNum.Value,
                              PicPath = e.PicPath,
                              ShopId = s.Id,
                              NoId =e.NoId
                          }).FirstOrDefaultAsync();
            return await result;
        }

        [AbpAllowAnonymous]
        [DisableAuditing]
        public Task<string> GetAuthorizationUrl(string shopId, string host)
        {
            string url = host + "/GAWX/ExhibitionDetailUrl";
            return Task.FromResult(_weChatOAuthAppService.GetAuthorizeUrl(url, shopId, Senparc.Weixin.MP.OAuthScope.snsapi_base));
        }
    }
}



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
using HC.WeChat.Retailers.Authorization;
using HC.WeChat.Retailers.Dtos;
using HC.WeChat.Retailers.DomainServices;
using HC.WeChat.Retailers;
using System;
using HC.WeChat.Authorization;
using Microsoft.AspNetCore.Identity;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Hosting;
using HC.WeChat.Dto;
using Abp.Domain.Uow;
using HC.WeChat.Helpers;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using HC.WeChat.LevelLogs;
using HC.WeChat.GACustPoints;
using HC.WeChat.GAGrades;
using HC.WeChat.Shops;
using HC.WeChat.IntegralDetails;
using HC.WeChat.Products;
using HC.WeChat.WeChatUsers;
using HC.WeChat.WechatEnums;

namespace HC.WeChat.Retailers
{
    /// <summary>
    /// Retailer应用层服务的接口实现方法
    /// </summary>
    //[AbpAuthorize(RetailerAppPermissions.Retailer)]
    [AbpAuthorize(AppPermissions.Pages)]
    public class RetailerAppService : WeChatAppServiceBase, IRetailerAppService
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        //private readonly IRepository<Retailer, Guid> _retailerRepository;
        private readonly IRetailerRepository _retailerRepository;
        private readonly IRetailerManager _retailerManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IRepository<Shop, Guid> _shopRepository;
        private readonly IPurchaserecordRepository _purchaserecordRepository;
        private readonly IRepository<IntegralDetail, Guid> _integraldetailRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<WeChatUser, Guid> _wechatuserRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        public RetailerAppService(IRetailerRepository retailerRepository
      , IRetailerManager retailerManager
            , IHostingEnvironment hostingEnvironment
            , IRepository<Shop, Guid> shopRepository
            , IPurchaserecordRepository purchaserecordRepository
            , IRepository<IntegralDetail, Guid> integraldetailRepository
            , IRepository<Product, Guid> productRepository
            , IRepository<WeChatUser, Guid> wechatuserRepository
        )
        {
            _hostingEnvironment = hostingEnvironment;
            _retailerRepository = retailerRepository;
            _retailerManager = retailerManager;
            _shopRepository = shopRepository;
            _purchaserecordRepository = purchaserecordRepository;
            _integraldetailRepository = integraldetailRepository;
            _productRepository = productRepository;
            _wechatuserRepository = wechatuserRepository;
        }

        /// <summary>
        /// 获取Retailer的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<RetailerListDto>> GetPagedRetailers(GetRetailersInput input)
        {
            var mid = UserManager.GetControlEmployeeId();
            var query = _retailerRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(input.Name), r => r.Name.Contains(input.Name) || r.Code.Contains(input.Name))
                .WhereIf(input.Scale.HasValue, r => r.Scale == input.Scale)
                .WhereIf(input.Markets.HasValue, r => r.MarketType == input.Markets)
                .WhereIf(input.Status.HasValue, r => r.IsAction == input.Status)
                .WhereIf(mid.HasValue, r => r.EmployeeId == mid);

            //TODO:根据传入的参数添加过滤条件
            var retailerCount = await query.CountAsync();

            var retailers = await query
                .OrderByDescending(r => r.CreationTime)
                .PageBy(input)
                .ToListAsync();

            //var retailerListDtos = ObjectMapper.Map<List <RetailerListDto>>(retailers);
            var retailerListDtos = retailers.MapTo<List<RetailerListDto>>();

            return new PagedResultDto<RetailerListDto>(
                retailerCount,
                retailerListDtos
                );

        }

        /// <summary>
        /// 通过指定id获取RetailerListDto信息
        /// </summary>
        [AbpAllowAnonymous]
        public async Task<RetailerListDto> GetRetailerByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _retailerRepository.GetAsync(input.Id);

            return entity.MapTo<RetailerListDto>();
        }

        /// <summary>
        /// 导出Retailer为excel表
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetRetailersToExcel(){
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
        public async Task<GetRetailerForEditOutput> GetRetailerForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetRetailerForEditOutput();
            RetailerEditDto retailerEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _retailerRepository.GetAsync(input.Id.Value);

                retailerEditDto = entity.MapTo<RetailerEditDto>();

                //retailerEditDto = ObjectMapper.Map<List <retailerEditDto>>(entity);
            }
            else
            {
                retailerEditDto = new RetailerEditDto();
            }

            output.Retailer = retailerEditDto;
            return output;

        }

        /// <summary>
        /// 添加或者修改Retailer的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateRetailer(CreateOrUpdateRetailerInput input)
        {

            if (input.Retailer.Id.HasValue)
            {
                await UpdateRetailerAsync(input.Retailer);
            }
            else
            {
                await CreateRetailerAsync(input.Retailer);
            }
        }

        /// <summary>
        /// 新增Retailer
        /// </summary>
        //[AbpAuthorize(RetailerAppPermissions.Retailer_CreateRetailer)]
        protected virtual async Task<RetailerEditDto> CreateRetailerAsync(RetailerEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var entity = ObjectMapper.Map<Retailer>(input);
            entity.TenantId = AbpSession.TenantId;
            entity = await _retailerRepository.InsertAsync(entity);
            return entity.MapTo<RetailerEditDto>();
        }

        /// <summary>
        /// 编辑Retailer
        /// </summary>
        //[AbpAuthorize(RetailerAppPermissions.Retailer_EditRetailer)]
        protected virtual async Task UpdateRetailerAsync(RetailerEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            var entity = await _retailerRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _retailerRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除Retailer信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[AbpAuthorize(RetailerAppPermissions.Retailer_DeleteRetailer)]
        public async Task DeleteRetailer(EntityDto<Guid> input)
        {

            //TODO:删除前的逻辑判断，是否允许删除
            await _retailerRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除Retailer的方法
        /// </summary>
        //[AbpAuthorize(RetailerAppPermissions.Retailer_BatchDeleteRetailers)]
        public async Task BatchDeleteRetailersAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _retailerRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 添加或者修改Retailer的方法
        /// </summary>
        /// <param name="input">零售客户实体</param>
        /// <returns></returns>
        public async Task CreateOrUpdateRetailerDto(RetailerEditDto input)
        {
            if (input.Id.HasValue)
            {
                await UpdateRetailerAsync(input);
            }
            else
            {
                await CreateRetailerAsync(input);
            }
        }

        /// <summary>
        /// 通过id查询单个零售户信息
        /// </summary>
        /// <param name="input">零售户id</param>
        /// <returns></returns>
        public async Task<RetailerListDto> GetRetailerByIdDtoAsync(EntityDto<Guid> input)
        {
            var entity = await _retailerRepository.GetAll().Where(r => r.Id == input.Id).FirstOrDefaultAsync();
            return entity.MapTo<RetailerListDto>();
        }

        /// <summary>
        /// 检查零售户编码是否可用
        /// </summary>
        /// <returns></returns>
        public bool CheckName(string code, Guid? id)
        {
            var count = _retailerRepository.GetAll().Where(r => r.Code == code).Count();
            var entity = _retailerRepository.GetAll().Where(e => e.Id == id).FirstOrDefault();
            if (entity != null)
            {
                if (entity.Code == code)
                {
                    return true;
                }
                else if (count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return count <= 0;
            }
        }
        #region 导出档级模板

        private async Task<List<RetailerListDto>> GetRetailerAllListAsync(GetRetailersInput input)
        {
            var mid = UserManager.GetControlEmployeeId();
            var query = _retailerRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(input.Name), r => r.Name.Contains(input.Name) || r.Code.Contains(input.Name))
                .WhereIf(input.Scale.HasValue, r => r.Scale == input.Scale)
                .WhereIf(input.Markets.HasValue, r => r.MarketType == input.Markets)
                .WhereIf(input.Status.HasValue, r => r.IsAction == input.Status)
                .WhereIf(mid.HasValue, r => r.EmployeeId == mid);
            var retailers = await query.OrderByDescending(r => r.CreationTime).ToListAsync();
            var retailerListDtos = retailers.MapTo<List<RetailerListDto>>();
            return retailerListDtos;
        }

        public async Task<List<RetailerListDto>> GetRetailerLevelListAsync(GetRetailersInput input)
        {
            var mid = UserManager.GetControlEmployeeId();
            var query = _retailerRepository.GetAll()
                  .WhereIf(!string.IsNullOrEmpty(input.Name), r => r.Name.Contains(input.Name) || r.Code.Contains(input.Name))
                  .WhereIf(input.Scale.HasValue, r => r.Scale == input.Scale)
                  .WhereIf(input.Markets.HasValue, r => r.MarketType == input.Markets)
                  .WhereIf(input.Status.HasValue, r => r.IsAction == input.Status)
                  .WhereIf(mid.HasValue, r => r.EmployeeId == mid);
            var retailers = await query.OrderByDescending(r => r.CreationTime).ToListAsync();
            var retailerListDtos = retailers.MapTo<List<RetailerListDto>>();
            return retailerListDtos;
        }

        /// <summary>
        /// 导出零售客户详单
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private string SaveRetailerAllExcel(string fileName, List<RetailerListDto> data)
        {
            var fullPath = ExcelHelper.GetSavePath(_hostingEnvironment.WebRootPath) + fileName;
            using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("RetailDetails");
                var rowIndex = 0;
                IRow titleRow = sheet.CreateRow(rowIndex);
                string[] titles = { "客户ID", "客户编码", "客户姓名", "客户分档", "经营地址", "订货周期", "订货电话", "订货方式", "业态", "分公司", "客户经理", "终端类型", "商圈类型", "经营规模", "市场类型", "市场部门ID", "市场部门名", "送货线路", "专卖证号" };
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
                    ExcelHelper.SetCell(row.CreateCell(0), font, item.CustId);
                    ExcelHelper.SetCell(row.CreateCell(1), font, item.Code);
                    ExcelHelper.SetCell(row.CreateCell(2), font, item.Name);
                    ExcelHelper.SetCell(row.CreateCell(3), font, item.ArchivalLevel);
                    ExcelHelper.SetCell(row.CreateCell(4), font, item.BusinessAddress);
                    ExcelHelper.SetCell(row.CreateCell(5), font, item.OrderCycle);
                    ExcelHelper.SetCell(row.CreateCell(6), font, item.Telephone);
                    ExcelHelper.SetCell(row.CreateCell(7), font, item.OrderMode.ToString());
                    ExcelHelper.SetCell(row.CreateCell(8), font, item.StoreType);
                    ExcelHelper.SetCell(row.CreateCell(9), font, item.BranchCompany);
                    ExcelHelper.SetCell(row.CreateCell(10), font, item.Manager);
                    ExcelHelper.SetCell(row.CreateCell(11), font, item.TerminalType.ToString());
                    ExcelHelper.SetCell(row.CreateCell(12), font, item.BusinessType);
                    ExcelHelper.SetCell(row.CreateCell(13), font, item.Scale.ToString());
                    ExcelHelper.SetCell(row.CreateCell(14), font, item.MarketType.ToString());
                    ExcelHelper.SetCell(row.CreateCell(15), font, item.DepartmentId);
                    ExcelHelper.SetCell(row.CreateCell(16), font, item.Department);
                    ExcelHelper.SetCell(row.CreateCell(17), font, item.DeliveryLine);
                    ExcelHelper.SetCell(row.CreateCell(18), font, item.LicenseKey);
                }
                workbook.Write(fs);
            }
            return "/files/downloadtemp/" + fileName;
        }

        private string SaveRetailerLevelExcel(string fileName, List<RetailerListDto> data)
        {
            var fullPath = ExcelHelper.GetSavePath(_hostingEnvironment.WebRootPath) + fileName;
            using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("RetailLevel");
                var rowIndex = 0;
                IRow titleRow = sheet.CreateRow(rowIndex);
                string[] titles = { "客户编码", "姓名", "客户分档", "订货电话", "市场部", "客户经理", "专卖证号" };
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
                    ExcelHelper.SetCell(row.CreateCell(0), font, item.Code);
                    ExcelHelper.SetCell(row.CreateCell(1), font, item.Name);
                    ExcelHelper.SetCell(row.CreateCell(2), font, item.ArchivalLevel);
                    ExcelHelper.SetCell(row.CreateCell(3), font, item.Telephone);
                    ExcelHelper.SetCell(row.CreateCell(4), font, item.Department);
                    ExcelHelper.SetCell(row.CreateCell(5), font, item.Manager);
                    ExcelHelper.SetCell(row.CreateCell(6), font, item.LicenseKey);
                }

                workbook.Write(fs);
            }
            return "/files/downloadtemp/" + fileName;
        }

        [UnitOfWork(isTransactional: false)]
        public async Task<APIResultDto> ExportRetailerLevelExcel(GetRetailersInput input)
        {
            try
            {
                var exportData = await GetRetailerLevelListAsync(input);
                var result = new APIResultDto();
                result.Code = 0;
                result.Data = SaveRetailerLevelExcel("零售客户.xlsx", exportData);
                return result;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("ExportPostInfoExcel errormsg:{0} Exception:{1}", ex.Message, ex);
                return new APIResultDto() { Code = 901, Msg = "网络忙... 请待会重试！" };
            }
        }

        [UnitOfWork(isTransactional: false)]
        public async Task<APIResultDto> ExportRetailerAllExcel(GetRetailersInput input)
        {
            try
            {
                var exportData = await GetRetailerAllListAsync(input);
                var result = new APIResultDto();
                result.Code = 0;
                result.Data = SaveRetailerAllExcel("零售客户详单.xlsx", exportData);
                return result;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("ExportPostInfoExcel errormsg:{0} Exception:{1}", ex.Message, ex);
                return new APIResultDto() { Code = 901, Msg = "网络忙... 请待会重试！" };
            }
        }

        #endregion

        #region 导入货源档级

        /// <summary>
        /// 更新到数据库
        /// </summary>
        private async Task UpdateRetailerLevelsAsync(List<RetailerListDto> retailerLevelList)
        {
            var rlcodes = retailerLevelList.Select(r => r.Code).ToArray();
            var retailerList = await _retailerRepository.GetAll().Where(r => rlcodes.Contains(r.Code)).ToListAsync();
            foreach (var item in retailerLevelList)
            {
                var retailer = retailerList.Where(r => r.Code == item.Code).FirstOrDefault();
                if (retailer != null)
                {
                    retailer.ArchivalLevel = item.ArchivalLevel;
                }
            }
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 从上传的Excel读出数据
        /// </summary>
        private async Task<List<RetailerListDto>> GetRetailerLevelAsync()
        {
            string fileName = _hostingEnvironment.WebRootPath + "/upload/files/CustomerUpload.xlsx";
            var resultList = new List<RetailerListDto>();
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);
                ISheet sheet = workbook.GetSheet("RetailLevel");
                if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                {
                    sheet = workbook.GetSheetAt(0);
                }

                if (sheet != null)
                {
                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = 1; i <= rowCount; ++i)//排除首行标题
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        var retailerLevel = new RetailerListDto();
                        if (row.GetCell(0) != null && row.GetCell(2) != null)
                        {
                            retailerLevel.Code = row.GetCell(0).ToString();
                            retailerLevel.ArchivalLevel = row.GetCell(2).ToString();
                            resultList.Add(retailerLevel);
                        }
                    }
                }

                return await Task.FromResult(resultList);
            }
        }

        /// <summary>
        /// 导入档级
        /// </summary>
        public async Task<APIResultDto> ImportRetailerLevelExcelAsync()
        {
            //获取Excel数据
            var excelList = await GetRetailerLevelAsync();

            //循环批量更新
            await UpdateRetailerLevelsAsync(excelList);

            return new APIResultDto() { Code = 0, Msg = "导入数据成功" };
        }

        #endregion

        #region 微信

        /// <summary>
        /// 分页获取零售户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<List<RetailerListDto>> GetAllRetailByPageAsync(GetRetailersWeChatInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(input.tenantId))
            {
                var retailList = new List<RetailerListDto>();
                if (input.IsMore)
                {
                    var retailListQ = await _retailerRepository.GetAll().Where(r => r.Telephone.Contains(input.Filter) || r.LicenseKey.Contains(input.Filter) || r.Name.Contains(input.Filter) && r.IsAction == true)
                   .OrderBy(r => r.Name).Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync();
                    retailList = retailListQ.MapTo<List<RetailerListDto>>();
                }
                else
                {
                    var retailListQ = await _retailerRepository.GetAll().Where(r => r.Telephone.Contains(input.Filter) || r.LicenseKey.Contains(input.Filter) || r.Name.Contains(input.Filter) && r.IsAction == true)
                    .OrderBy(r => r.Name).Skip(0).Take(20).ToListAsync();
                    retailList = retailListQ.MapTo<List<RetailerListDto>>();
                }
                return retailList;
            }
        }

        /// <summary>
        /// 通过指定id获取RetailerListDto信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<RetailerListDto> GetRetailerByIdDtoForWeChatAsync(Guid id)
        {
            var entity = await _retailerRepository.GetAll().Where(r => r.Id == id).FirstOrDefaultAsync();
            return entity.MapTo<RetailerListDto>();
        }
        /// <summary>
        /// 通过指定licenseKey获取RetailerListDto信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<RetailerListDto> GetRetailerByIdDtoByLKeyForWeChatAsync(Guid userId)
        {
            var entity = await _retailerRepository.GetAll().Where(r => r.Id == userId).FirstOrDefaultAsync();

            return entity.MapTo<RetailerListDto>();
        }
        #endregion

        #region 数据报表

        /// <summary>
        /// 查询店铺消费报表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ShopReportData>> GetDataStatisticsAsync(GetShopReportDataInput input)
        {
            List<ShopReportData> entity = null;
            if (input.TimeType == 2)
            {
                entity = await _retailerRepository.GetShopReportByRangeAsync(input.BeginTime, input.EndTime);
            }
            else
            {
                entity = await _retailerRepository.GetShopReportAsync();
            }
            var shopReportDataCount = entity.Count();

            var statisticsListDto = entity.MapTo<List<ShopReportData>>();
            return new PagedResultDto<ShopReportData>(
                shopReportDataCount,
                statisticsListDto
                );
        }

        /// <summary>
        /// 店铺报表详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ShopReportData>> GetShopReportDataDetailAsync(GetShopReportDataInput input)
        {
            var retailer = _retailerRepository.GetAll();
            var shop = _shopRepository.GetAll().Where(v => v.Status == ShopAuditStatus.已审核);
            var products = _productRepository.GetAll();
            var purchaserecord = _purchaserecordRepository.GetAll();
            List<ShopReportData> entity = null;
            int shopReportDataCount = 0;
            if (input.OrganizatType == 1)
            {
                entity = await (from pur in purchaserecord
                                join p in products on pur.ProductId equals p.Id
                                group new { pur.Specification, pur.Quantity, pur.Id, p.Price } by new { pur.Specification } into g
                                select new ShopReportData()
                                {
                                    Specification = g.Key.Specification,
                                    PriceTotal = g.Sum(v => v.Quantity * v.Price),
                                    ScanQuantity = g.Sum(v => v.Quantity),
                                    ScanFrequency = g.GroupBy(v => v.Id).Count()
                                }).WhereIf(!string.IsNullOrEmpty(input.Specification), r => r.Specification.Contains(input.Specification)).OrderByDescending(v => v.ScanQuantity).PageBy(input).ToListAsync();

                shopReportDataCount = await (from pur in purchaserecord
                                             join p in products on pur.ProductId equals p.Id
                                             group new { pur.Specification, pur.Quantity, pur.Id, p.Price } by new { pur.Specification } into g
                                             select new ShopReportData()
                                             {
                                                 Specification = g.Key.Specification,
                                                 PriceTotal = g.Sum(v => v.Quantity * v.Price),
                                                 ScanQuantity = g.Sum(v => v.Quantity),
                                                 ScanFrequency = g.GroupBy(v => v.Id).Count()
                                             }).WhereIf(!string.IsNullOrEmpty(input.Specification), r => r.Specification.Contains(input.Specification)).CountAsync();
            }
            else if (input.OrganizatType == 2)
            {
                entity = await (from pur in purchaserecord
                                join p in products on pur.ProductId equals p.Id
                                join s in shop on pur.ShopId equals s.Id
                                join r in retailer on s.RetailerId equals r.Id
                                where r.BranchCompany == input.Organization
                                group new { pur.Specification, pur.Quantity, pur.Id, p.Price } by new { pur.Specification } into g
                                select new ShopReportData()
                                {
                                    Specification = g.Key.Specification,
                                    PriceTotal = g.Sum(v => v.Quantity * v.Price),
                                    ScanQuantity = g.Sum(v => v.Quantity),
                                    ScanFrequency = g.GroupBy(v => v.Id).Count()
                                }).WhereIf(!string.IsNullOrEmpty(input.Specification), r => r.Specification.Contains(input.Specification)).OrderByDescending(v => v.ScanQuantity).PageBy(input).ToListAsync();

                shopReportDataCount = await (from pur in purchaserecord
                                             join p in products on pur.ProductId equals p.Id
                                             join s in shop on pur.ShopId equals s.Id
                                             join r in retailer on s.RetailerId equals r.Id
                                             where r.BranchCompany == input.Organization
                                             group new { pur.Specification, pur.Quantity, pur.Id, p.Price } by new { pur.Specification } into g
                                             select new ShopReportData()
                                             {
                                                 Specification = g.Key.Specification,
                                                 PriceTotal = g.Sum(v => v.Quantity * v.Price),
                                                 ScanQuantity = g.Sum(v => v.Quantity),
                                                 ScanFrequency = g.GroupBy(v => v.Id).Count()
                                             }).WhereIf(!string.IsNullOrEmpty(input.Specification), r => r.Specification.Contains(input.Specification)).CountAsync();
            }
            else if (input.OrganizatType == 3)
            {
                entity = await (from pur in purchaserecord
                                join p in products on pur.ProductId equals p.Id
                                join s in shop on pur.ShopId equals s.Id
                                join r in retailer on s.RetailerId equals r.Id
                                where r.Area == input.Organization
                                group new { pur.Specification, pur.Quantity, pur.Id, p.Price } by new { pur.Specification } into g
                                select new ShopReportData()
                                {
                                    Specification = g.Key.Specification,
                                    PriceTotal = g.Sum(v => v.Quantity * v.Price),
                                    ScanQuantity = g.Sum(v => v.Quantity),
                                    ScanFrequency = g.GroupBy(v => v.Id).Count()
                                }).WhereIf(!string.IsNullOrEmpty(input.Specification), r => r.Specification.Contains(input.Specification)).OrderByDescending(v => v.ScanQuantity).PageBy(input).ToListAsync();

                shopReportDataCount = await (from pur in purchaserecord
                                             join p in products on pur.ProductId equals p.Id
                                             join s in shop on pur.ShopId equals s.Id
                                             join r in retailer on s.RetailerId equals r.Id
                                             where r.Area == input.Organization
                                             group new { pur.Specification, pur.Quantity, pur.Id, p.Price } by new { pur.Specification } into g
                                             select new ShopReportData()
                                             {
                                                 Specification = g.Key.Specification,
                                                 PriceTotal = g.Sum(v => v.Quantity * v.Price),
                                                 ScanQuantity = g.Sum(v => v.Quantity),
                                                 ScanFrequency = g.GroupBy(v => v.Id).Count()
                                             }).WhereIf(!string.IsNullOrEmpty(input.Specification), r => r.Specification.Contains(input.Specification)).CountAsync();
            }
            else
            {
                entity = await (from pur in purchaserecord
                                join p in products on pur.ProductId equals p.Id
                                join s in shop on pur.ShopId equals s.Id
                                join r in retailer on s.RetailerId equals r.Id
                                where r.SlsmanName == input.Organization
                                group new { pur.Specification, pur.Quantity, pur.Id, p.Price } by new { pur.Specification } into g
                                select new ShopReportData()
                                {
                                    Specification = g.Key.Specification,
                                    PriceTotal = g.Sum(v => v.Quantity * v.Price),
                                    ScanQuantity = g.Sum(v => v.Quantity),
                                    ScanFrequency = g.GroupBy(v => v.Id).Count()
                                }).WhereIf(!string.IsNullOrEmpty(input.Specification), r => r.Specification.Contains(input.Specification)).OrderByDescending(v => v.ScanQuantity).PageBy(input).ToListAsync();

                shopReportDataCount = await (from pur in purchaserecord
                                             join p in products on pur.ProductId equals p.Id
                                             join s in shop on pur.ShopId equals s.Id
                                             join r in retailer on s.RetailerId equals r.Id
                                             where r.SlsmanName == input.Organization
                                             group new { pur.Specification, pur.Quantity, pur.Id, p.Price } by new { pur.Specification } into g
                                             select new ShopReportData()
                                             {
                                                 Specification = g.Key.Specification,
                                                 PriceTotal = g.Sum(v => v.Quantity * v.Price),
                                                 ScanQuantity = g.Sum(v => v.Quantity),
                                                 ScanFrequency = g.GroupBy(v => v.Id).Count()
                                             }).WhereIf(!string.IsNullOrEmpty(input.Specification), r => r.Specification.Contains(input.Specification)).CountAsync();
            }

            var statisticsListDto = entity.MapTo<List<ShopReportData>>();
            return new PagedResultDto<ShopReportData>(
                shopReportDataCount,
                statisticsListDto
                );
        }

        #endregion

        #region 机构报表Excel
        [UnitOfWork(isTransactional: false)]
        public async Task<APIResultDto> ExportShopReportDataExcel(GetShopReportDataInput input)
        {
            try
            {
                var exportData = await GetShopReportData(input);
                var result = new APIResultDto();
                result.Code = 0;
                result.Data = SaveShopReportDataExcel("机构报表统计.xlsx", exportData);
                return result;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("ExportShopExcel errormsg{0} Exception{1}", ex.Message, ex);
                return new APIResultDto() { Code = 901, Msg = "网络忙...请待会儿再试！" };
            }
        }

        private async Task<List<ShopReportData>> GetShopReportData(GetShopReportDataInput input)
        {
            List<ShopReportData> entity = null;
            if (input.TimeType == 2)
            {
                entity = await _retailerRepository.GetShopReportByRangeAsync(input.BeginTime, input.EndTime);
            }
            else
            {
                entity = await _retailerRepository.GetShopReportAsync();
            }
            var statisticsListDto = entity.MapTo<List<ShopReportData>>();
            return statisticsListDto;
        }

        private string SaveShopReportDataExcel(string fileName, List<ShopReportData> data)
        {
            var fullPath = ExcelHelper.GetSavePath(_hostingEnvironment.WebRootPath) + fileName;
            using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("机构报表");
                var rowIndex = 0;
                IRow titleRow = sheet.CreateRow(rowIndex);
                string[] titles = { "组织机构", "注册店铺数", "扫码量	", "扫码次数	", "扫码金额	", "消费积分	", "店铺积分	" };
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
                    ExcelHelper.SetCell(row.CreateCell(0), font, item.Organization);
                    ExcelHelper.SetCell(row.CreateCell(1), font, item.ShopTotal.ToString());
                    ExcelHelper.SetCell(row.CreateCell(2), font, item.ScanQuantity.ToString());
                    ExcelHelper.SetCell(row.CreateCell(3), font, item.ScanFrequency.ToString());
                    ExcelHelper.SetCell(row.CreateCell(4), font, item.PriceTotal.ToString());
                    ExcelHelper.SetCell(row.CreateCell(5), font, item.CustIntegral.ToString());
                    ExcelHelper.SetCell(row.CreateCell(6), font, item.RetailerIntegral.ToString());
                }
                workbook.Write(fs);
            }
            return "/files/downloadtemp/" + fileName;
        }
        #endregion 

        #region 单个机构报表Excel
        /// <summary>
        /// 单个机构报表统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(isTransactional: false)]
        public async Task<APIResultDto> ExportShopReportDataByOrganizationExcel(GetShopReportDataInput input)
        {
            try
            {
                var exportData = await GetShopReportDataByOrganizationExcel(input);
                var result = new APIResultDto();
                result.Code = 0;
                result.Data = SaveShopReportDataByOrganizationExcelExcel($"{input.Organization}报表统计.xlsx", exportData);
                return result;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("ExportShopExcel errormsg{0} Exception{1}", ex.Message, ex);
                return new APIResultDto() { Code = 901, Msg = "网络忙...请待会儿再试！" };
            }
        }
        private async Task<List<ShopReportData>> GetShopReportDataByOrganizationExcel(GetShopReportDataInput input)
        {
            var retailer = _retailerRepository.GetAll();
            var shop = _shopRepository.GetAll().Where(v => v.Status == ShopAuditStatus.已审核);
            var products = _productRepository.GetAll();
            var purchaserecord = _purchaserecordRepository.GetAll();
            List<ShopReportData> entity = null;
            if (input.OrganizatType == 1)
            {
                entity = await (from pur in purchaserecord
                                join p in products on pur.ProductId equals p.Id
                                group new { pur.Specification, pur.Quantity, pur.Id, p.Price } by new { pur.Specification } into g
                                select new ShopReportData()
                                {
                                    Specification = g.Key.Specification,
                                    PriceTotal = g.Sum(v => v.Quantity * v.Price),
                                    ScanQuantity = g.Sum(v => v.Quantity),
                                    ScanFrequency = g.GroupBy(v => v.Id).Count()
                                }).WhereIf(!string.IsNullOrEmpty(input.Specification), r => r.Specification.Contains(input.Specification)).OrderByDescending(v => v.ScanQuantity).ToListAsync();
            }
            else if (input.OrganizatType == 2)
            {
                entity = await (from pur in purchaserecord
                                join p in products on pur.ProductId equals p.Id
                                join s in shop on pur.ShopId equals s.Id
                                join r in retailer on s.RetailerId equals r.Id
                                where r.BranchCompany == input.Organization
                                group new { pur.Specification, pur.Quantity, pur.Id, p.Price } by new { pur.Specification } into g
                                select new ShopReportData()
                                {
                                    Specification = g.Key.Specification,
                                    PriceTotal = g.Sum(v => v.Quantity * v.Price),
                                    ScanQuantity = g.Sum(v => v.Quantity),
                                    ScanFrequency = g.GroupBy(v => v.Id).Count()
                                }).WhereIf(!string.IsNullOrEmpty(input.Specification), r => r.Specification.Contains(input.Specification)).OrderByDescending(v => v.ScanQuantity).ToListAsync();
            }
            else if (input.OrganizatType == 3)
            {
                entity = await (from pur in purchaserecord
                                join p in products on pur.ProductId equals p.Id
                                join s in shop on pur.ShopId equals s.Id
                                join r in retailer on s.RetailerId equals r.Id
                                where r.Area == input.Organization
                                group new { pur.Specification, pur.Quantity, pur.Id, p.Price } by new { pur.Specification } into g
                                select new ShopReportData()
                                {
                                    Specification = g.Key.Specification,
                                    PriceTotal = g.Sum(v => v.Quantity * v.Price),
                                    ScanQuantity = g.Sum(v => v.Quantity),
                                    ScanFrequency = g.GroupBy(v => v.Id).Count()
                                }).WhereIf(!string.IsNullOrEmpty(input.Specification), r => r.Specification.Contains(input.Specification)).OrderByDescending(v => v.ScanQuantity).ToListAsync();
            }
            else
            {
                entity = await (from pur in purchaserecord
                                join p in products on pur.ProductId equals p.Id
                                join s in shop on pur.ShopId equals s.Id
                                join r in retailer on s.RetailerId equals r.Id
                                where r.SlsmanName == input.Organization
                                group new { pur.Specification, pur.Quantity, pur.Id, p.Price } by new { pur.Specification } into g
                                select new ShopReportData()
                                {
                                    Specification = g.Key.Specification,
                                    PriceTotal = g.Sum(v => v.Quantity * v.Price),
                                    ScanQuantity = g.Sum(v => v.Quantity),
                                    ScanFrequency = g.GroupBy(v => v.Id).Count()
                                }).WhereIf(!string.IsNullOrEmpty(input.Specification), r => r.Specification.Contains(input.Specification)).OrderByDescending(v => v.ScanQuantity).ToListAsync();
            }
            int shopReportDataCount =
            await (from pur in purchaserecord
                   join p in products on pur.ProductId equals p.Id
                   group new { pur.Specification, pur.Quantity, pur.Id, p.Price } by new { pur.Specification } into g
                   select new ShopReportData()
                   {
                       Specification = g.Key.Specification,
                       PriceTotal = g.Sum(v => v.Quantity * v.Price),
                       ScanQuantity = g.Sum(v => v.Quantity),
                       ScanFrequency = g.GroupBy(v => v.Id).Count()
                   }).WhereIf(!string.IsNullOrEmpty(input.Specification), r => r.Specification.Contains(input.Specification)).CountAsync();
            var statisticsListDto = entity.MapTo<List<ShopReportData>>();
            return statisticsListDto;
        }
        private string SaveShopReportDataByOrganizationExcelExcel(string fileName, List<ShopReportData> data)
        {
            var fullPath = ExcelHelper.GetSavePath(_hostingEnvironment.WebRootPath) + fileName;
            using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("机构报表");
                var rowIndex = 0;
                IRow titleRow = sheet.CreateRow(rowIndex);
                string[] titles = { "商品名称", "扫码数量", "扫码次数	", "扫码金额	" };
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
                    ExcelHelper.SetCell(row.CreateCell(0), font, item.Specification);
                    ExcelHelper.SetCell(row.CreateCell(2), font, item.ScanQuantity.ToString());
                    ExcelHelper.SetCell(row.CreateCell(3), font, item.ScanFrequency.ToString());
                    ExcelHelper.SetCell(row.CreateCell(4), font, item.PriceTotal.ToString());
                }
                workbook.Write(fs);
            }
            return "/files/downloadtemp/" + fileName;
        }
        #endregion
    }

}


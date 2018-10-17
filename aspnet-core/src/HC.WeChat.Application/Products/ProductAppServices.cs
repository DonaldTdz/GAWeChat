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
using HC.WeChat.Products.Authorization;
using HC.WeChat.Products.Dtos;
using HC.WeChat.Products.DomainServices;
using HC.WeChat.Products;
using System;
using HC.WeChat.Authorization;
using Microsoft.AspNetCore.Hosting;
using HC.WeChat.Retailers;
using HC.WeChat.WeChatUsers;
using HC.WeChat.WechatEnums;
using HC.WeChat.EPCos;
using HC.WeChat.EPCoLines;
using HC.WeChat.GACustPoints;
using HC.WeChat.GAGrades;
using HC.WeChat.Retailers.Dtos;
using HC.WeChat.Helpers;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using HC.WeChat.Dto;
using Abp.Domain.Uow;
using System.Text.RegularExpressions;
using HC.WeChat.LevelLogs;
using Abp.Auditing;
//using System.Linq;

namespace HC.WeChat.Products
{
    /// <summary>
    /// Product应用层服务的接口实现方法
    /// </summary>
    //[AbpAuthorize(ProductAppPermissions.Product)]
    [AbpAuthorize(AppPermissions.Pages)]
    public class ProductAppService : WeChatAppServiceBase, IProductAppService
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IProductManager _productManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IRetailerAppService _retailerService;
        private readonly IRepository<WeChatUser, Guid> _wechatuserRepository;
        private readonly IRepository<EPCo, Guid> _epcoRepository;
        private readonly IRepository<EPCoLine, Guid> _epcolineRepository;
        private readonly IRepository<GACustPoint, Guid> _gacustpointRepository;
        private readonly IRepository<GAGrade, int> _gagradeRepository;
        private readonly ILevelLogAppService _levelLogAppService;
        private readonly IRepository<Retailer, Guid> _retailerRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductAppService(IRepository<Product, Guid> productRepository
      , IProductManager productManager, IHostingEnvironment hostingEnvironment,
            IRetailerAppService retailerService, IRepository<WeChatUser, Guid> wechatuserRepository,
            IRepository<EPCo, Guid> epcoRepository, IRepository<EPCoLine, Guid> epcolineRepository,
            IRepository<GACustPoint, Guid> gacustpointRepository, IRepository<GAGrade, int> gagradeRepository,
            ILevelLogAppService levelLogAppService, IRepository<Retailer, Guid> retailerRepository
        )
        {
            _productRepository = productRepository;
            _productManager = productManager;
            _hostingEnvironment = hostingEnvironment;
            _retailerService = retailerService;
            _wechatuserRepository = wechatuserRepository;
            _epcoRepository = epcoRepository;
            _epcolineRepository = epcolineRepository;
            _gacustpointRepository = gacustpointRepository;
            _gagradeRepository = gagradeRepository;
            _levelLogAppService = levelLogAppService;
            _retailerRepository = retailerRepository;
        }

        /// <summary>
        /// 获取Product的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ProductListDto>> GetPagedProducts(GetProductsInput input)
        {
            // if (input.SortValue == "ascend")
            // {
            //     var query = _productRepository.GetAll()
            //    .WhereIf(!string.IsNullOrEmpty(input.Name), p => p.Specification.Contains(input.Name))
            //    .WhereIf(input.Type.HasValue, p => p.Type == input.Type)
            //    .WhereIf(input.IsRare.HasValue, p => p.IsRare == input.IsRare);
            //     //TODO:根据传入的参数添加过滤条件
            //     var productCount = await query.CountAsync();

            //     var products = await query
            //         .OrderByDescending(p => p.SearchCount)
            //         .ThenBy(input.Sorting)
            //         .PageBy(input)
            //         .ToListAsync();

            //     //var productListDtos = ObjectMapper.Map<List <ProductListDto>>(products);
            //     var productListDtos = products.MapTo<List<ProductListDto>>();

            //     return new PagedResultDto<ProductListDto>(
            //         productCount,
            //         productListDtos
            //         );
            // }
            // else if (input.SortValue == "descend")
            // {
            //     var query = _productRepository.GetAll()
            //             .WhereIf(!string.IsNullOrEmpty(input.Name), p => p.Specification.Contains(input.Name))
            //             .WhereIf(input.Type.HasValue, p => p.Type == input.Type)
            //             .WhereIf(input.IsRare.HasValue, p => p.IsRare == input.IsRare);
            //     //TODO:根据传入的参数添加过滤条件
            //     var productCount = await query.CountAsync();

            //     var products = await query
            //         .OrderBy(p => p.SearchCount)
            //         .ThenBy(input.Sorting)
            //         .PageBy(input)
            //         .ToListAsync();

            //     //var productListDtos = ObjectMapper.Map<List <ProductListDto>>(products);
            //     var productListDtos = products.MapTo<List<ProductListDto>>();

            //     return new PagedResultDto<ProductListDto>(
            //         productCount,
            //         productListDtos
            //         );
            // }
            // else
            // {
            //     var query = _productRepository.GetAll()
            //.WhereIf(!string.IsNullOrEmpty(input.Name), p => p.Specification.Contains(input.Name))
            //.WhereIf(input.Type.HasValue, p => p.Type == input.Type)
            //.WhereIf(input.IsRare.HasValue, p => p.IsRare == input.IsRare);
            //     //TODO:根据传入的参数添加过滤条件
            //     var productCount = await query.CountAsync();
            //     var products = await query
            //         .OrderBy(p => p.Specification)
            //         .ThenBy(input.Sorting)
            //         .PageBy(input)
            //         .ToListAsync();

            //     //var productListDtos = ObjectMapper.Map<List <ProductListDto>>(products);
            //     var productListDtos = products.MapTo<List<ProductListDto>>();

            //     return new PagedResultDto<ProductListDto>(
            //         productCount,
            //         productListDtos
            //         );
            // }
            var query = _productRepository.GetAll()
           .WhereIf(!string.IsNullOrEmpty(input.Name), p => p.Specification.Contains(input.Name))
           .WhereIf(input.Type.HasValue, p => p.Type == input.Type)
           .WhereIf(input.IsRare.HasValue, p => p.IsRare == input.IsRare);
            //TODO:根据传入的参数添加过滤条件
            var productCount = await query.CountAsync();
            if (input.SortValue == "ascend")
            {
                var products = await query
                     .OrderByDescending(p => p.SearchCount)
                     .ThenBy(input.Sorting)
                     .PageBy(input)
                     .ToListAsync();
                var productListDtos = products.MapTo<List<ProductListDto>>();

                return new PagedResultDto<ProductListDto>(
                    productCount,
                    productListDtos
                    );
            }
            else if (input.SortValue == "descend")
            {
                var products = await query
                    .OrderBy(p => p.SearchCount)
                    .ThenBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                //var productListDtos = ObjectMapper.Map<List <ProductListDto>>(products);
                var productListDtos = products.MapTo<List<ProductListDto>>();

                return new PagedResultDto<ProductListDto>(
                    productCount,
                    productListDtos
                    );
            }
            else
            {
                var products = await query
                    .OrderBy(p => p.Specification)
                    .ThenBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                //var productListDtos = ObjectMapper.Map<List <ProductListDto>>(products);
                var productListDtos = products.MapTo<List<ProductListDto>>();

                return new PagedResultDto<ProductListDto>(
                    productCount,
                    productListDtos
                    );
            }
        }

        /// <summary>
        /// 通过指定id获取ProductListDto信息
        /// </summary>
        public async Task<ProductListDto> GetProductByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _productRepository.GetAsync(input.Id);

            return entity.MapTo<ProductListDto>();
        }

        /// <summary>
        /// 导出Product为excel表
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetProductsToExcel(){
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
        public async Task<GetProductForEditOutput> GetProductForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetProductForEditOutput();
            ProductEditDto productEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _productRepository.GetAsync(input.Id.Value);

                productEditDto = entity.MapTo<ProductEditDto>();

                //productEditDto = ObjectMapper.Map<List <productEditDto>>(entity);
            }
            else
            {
                productEditDto = new ProductEditDto();
            }

            output.Product = productEditDto;
            return output;

        }

        /// <summary>
        /// 添加或者修改Product的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateProduct(CreateOrUpdateProductInput input)
        {

            if (input.Product.Id.HasValue)
            {
                await UpdateProductAsync(input.Product);
            }
            else
            {
                await CreateProductAsync(input.Product);
            }
        }

        /// <summary>
        /// 新增Product
        /// </summary>
        //[AbpAuthorize(ProductAppPermissions.Product_CreateProduct)]
        protected virtual async Task<ProductListDto> CreateProductAsync(ProductEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var entity = ObjectMapper.Map<Product>(input);
            entity.SearchCount = 0;
            entity = await _productRepository.InsertAsync(entity);
            return entity.MapTo<ProductListDto>();
        }

        /// <summary>
        /// 编辑Product
        /// </summary>
        //[AbpAuthorize(ProductAppPermissions.Product_EditProduct)]
        protected virtual async Task<ProductListDto> UpdateProductAsync(ProductEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            var entity = await _productRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            var result = await _productRepository.UpdateAsync(entity);
            return result.MapTo<ProductListDto>();
        }

        /// <summary>
        /// 删除Product信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[AbpAuthorize(ProductAppPermissions.Product_DeleteProduct)]
        public async Task DeleteProduct(EntityDto<Guid> input)
        {

            //TODO:删除前的逻辑判断，是否允许删除
            await _productRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除Product的方法
        /// </summary>
        //[AbpAuthorize(ProductAppPermissions.Product_BatchDeleteProducts)]
        public async Task BatchDeleteProductsAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _productRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 通过指定id获取ProductListDto信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProductListDto> GetProductByIdDtoAsync(EntityDto<Guid> input)
        {
            var entity = await _productRepository.GetAll().Where(p => p.Id == input.Id).FirstOrDefaultAsync();
            return entity.MapTo<ProductListDto>();
        }


        /// <summary>
        /// 添加或者修改Product的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProductListDto> CreateOrUpdateProductDto(ProductEditDto input)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            if (!string.IsNullOrEmpty(input.Img64) && !string.IsNullOrEmpty(input.FileName))
            {
                var base64 = new WechatImgBase64() { fileName = input.FileName, imageBase64 = input.Img64 };
                var photoUrl = await FilesPostsBase64(base64, "product");
                input.PhotoUrl = !string.IsNullOrEmpty(photoUrl) ? photoUrl : input.PhotoUrl;
            }
            if (input.Id.HasValue)
            {
                var entity = _productRepository.GetAsync(input.Id.MapTo<Guid>()).Result;
                var url = entity.PhotoUrl;
                var result = await UpdateProductAsync(input);
                //删除原来的单个图片
                if (url != result.PhotoUrl && url != "/assets/img/default.png")
                {
                    if (File.Exists(webRootPath + url))
                    {
                        File.Delete(webRootPath + url);
                    }
                }
                return result;

            }
            else
            {
                return await CreateProductAsync(input);

            }
        }
        /// <summary>
        /// base64转换
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> FilesPostsBase64(WechatImgBase64 input, string fileName)
        {
            var saveUrl = "";
            if (!string.IsNullOrWhiteSpace(input.imageBase64))
            {
                var reg = new Regex("data:image/(.*);base64,");
                input.imageBase64 = reg.Replace(input.imageBase64, "");
                byte[] imageByte = Convert.FromBase64String(input.imageBase64);
                var memorystream = new MemoryStream(imageByte);

                string webRootPath = _hostingEnvironment.WebRootPath;
                string contentRootPath = _hostingEnvironment.ContentRootPath;
                string fileExt = Path.GetExtension(input.fileName); //文件扩展名，不含“.”
                string newFileName = Guid.NewGuid().ToString() + fileExt; //随机生成新的文件名
                var fileDire = webRootPath + string.Format("/upload/{0}/", fileName);
                if (!Directory.Exists(fileDire))
                {
                    Directory.CreateDirectory(fileDire);
                }

                var filePath = fileDire + newFileName;

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await memorystream.CopyToAsync(stream);
                }
                saveUrl = filePath.Substring(webRootPath.Length);
                return saveUrl;
            }
            return saveUrl;
        }

        /// <summary>
        /// 获取特色商品
        /// </summary>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<RareProductDto> GetRareProduct(int? tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var query = _productRepository.GetAll().Where(p => p.IsRare == true && p.IsAction == true);

                RareProductDto data = new RareProductDto();
                data.CigaretteProducts = (await query.Where(q => q.Type == WechatEnums.ProductTypeEnum.卷烟类).OrderBy(q => q.Specification).ToListAsync()).MapTo<List<ProductListDto>>();
                data.SpecialProducts = (await query.Where(q => q.Type == WechatEnums.ProductTypeEnum.特产类).OrderBy(q => q.Specification).ToListAsync()).MapTo<List<ProductListDto>>();
                return data;
            }
        }

        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<ShopProductDto> GetShopProductByCode(string code, int? tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var query = await _productRepository.GetAll()
                    .Where(p => p.IsAction == true && p.Type == WechatEnums.ProductTypeEnum.卷烟类)
                    .Where(p => p.PackageCode == code || p.BarCode == code)
                    .FirstOrDefaultAsync();
                var shopProduct = query.MapTo<ShopProductDto>();
                if (shopProduct != null)
                {
                    if (shopProduct.PackageCode == code)
                    {
                        shopProduct.Num = 1;
                    }
                    else
                    {
                        shopProduct.Num = 10;
                    }
                    return shopProduct;
                }

                return null;
            }
        }

        /// <summary>
        /// 检查包码条码是否重复（0：不重复，1：包码重复，2：条码重复，3：包码、条码重复）
        /// </summary>
        /// <param name="code"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public async Task<int> GetCheckCode(CheckInput input)
        {
            var result = 0;
            var countP = 0;
            var countB = 0;
            if (!string.IsNullOrEmpty(input.PCode))
            {
                countP = await _productRepository.GetAll().Where(p => p.PackageCode == input.PCode || p.BarCode == input.PCode).WhereIf(input.ProductId != Guid.Empty, p => p.Id != input.ProductId).CountAsync();
            }
            if (!string.IsNullOrEmpty(input.BCode))
            {
                countB = await _productRepository.GetAll().Where(p => p.BarCode == input.BCode || p.PackageCode == input.BCode).WhereIf(input.ProductId != Guid.Empty, p => p.Id != input.ProductId).CountAsync();
            }
            //var entity = _productRepository.GetAll().Where(e => e.Id == input.ProductId).FirstOrDefault();

            if (countB > 0 && countP > 0)
            {
                result = 3;
            }
            else if (countB > 0 && countP == 0)
            {
                result = 2;
            }
            else if (countB == 0 && countP > 0)
            {
                result = 1;
            }
            return result;
        }

        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<List<RareProductSearchDto>> GetRareProductByKeyAsync(int? tenantId, string key)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var query = await _productRepository.GetAll().Where(p => p.IsRare == true && p.IsAction == true && p.Specification.Contains(key)||p.Tags.Contains(key)).ToListAsync();
                return query.MapTo<List<RareProductSearchDto>>();
            }
        }

        /// <summary>
        /// 获取台账信息
        /// </summary>
        /// <param name="userId">专卖证号</param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<RetailAllInfoDe> GetCustAndAccountInfoAsync(int? tenantId, Guid userId, int span)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {

                //RetailAllInfo result = new RetailAllInfo() { BasicInfo = new RetailInfoDto(), AccountBooks = new List<RetailAccount>() };
                RetailAllInfoDe result = new RetailAllInfoDe()
                {
                    //BasicInfo = new RetailInfoDto(),
                    MonthAccountBooks = new List<RetailAccountDe>(),
                    PreMonthAccountBooks = new List<RetailAccountDe>(),
                    AccountBooks = new List<RetailAccountDe>()
                };

                #region 原台账

                //var totalOrders = _epcoRepository.GetAll().Where(f => f.CUST_ID == retail.CustId);
                //var orderCodes = await totalOrders.Select(t => t.CO_NUM).ToListAsync();

                //var orderDetails = _epcolineRepository.GetAll().Where(f => orderCodes.Contains(f.CO_NUM));

                //var groupItems = await orderDetails.GroupBy(f => f.ITEM_ID).ToListAsync();
                //IList<string> orderItemIds = groupItems.Select(f => f.Key).ToList();
                //var goods = await _productRepository.GetAll().Where(f => orderItemIds.Contains(f.ItemId)).ToListAsync();
                //string sumMonth = DateTime.Now.Year.ToString();
                ////月度
                ////当月
                //if (DateTime.Now.Month < 10)
                //{
                //    sumMonth = sumMonth + "0" + DateTime.Now.Month.ToString();
                //}
                //else
                //{
                //    sumMonth = sumMonth + DateTime.Now.Month.ToString();
                //}
                ////上月
                //string preMonth = DateTime.Now.AddMonths(-1).Year.ToString();
                //if (DateTime.Now.AddMonths(-1).Month < 10)
                //{
                //    preMonth = preMonth + "0" + DateTime.Now.AddMonths(-1).Month.ToString();
                //}
                //else
                //{
                //    preMonth = preMonth + DateTime.Now.AddMonths(-1).Month.ToString();
                //}
                ////季度
                //var nowDate = sumMonth + (DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString());
                //var quarterlySpan = GetDate(3);
                ////年度
                //var yearSpan = GetDate(6);
                //foreach (var item in orderItemIds)
                //{
                //    var gd = goods.FirstOrDefault(f => f.ItemId == item);

                //    //上月
                //    var preMonthOrderCoNums = await totalOrders.Where(f => f.POSE_DATE.Contains(preMonth)).Select(f => f.CO_NUM).ToListAsync();
                //    var preMonthOd = await orderDetails.Where(f => f.ITEM_ID == item && preMonthOrderCoNums.Contains(f.CO_NUM)).ToListAsync();
                //    var preMothQty = preMonthOd.Count == 0 ? 0 : preMonthOd.Sum(t => t.QTY_ORD);

                //    //本月
                //    var thisMonthOrderCoNums = await totalOrders.Where(f => f.POSE_DATE.Contains(sumMonth)).Select(f => f.CO_NUM).ToListAsync();
                //    var thisMonthOd = await orderDetails.Where(f => f.ITEM_ID == item && thisMonthOrderCoNums.Contains(f.CO_NUM)).ToListAsync();
                //    var thisMonthQty = thisMonthOd.Count == 0 ? 0 : thisMonthOd.Sum(t => t.QTY_ORD);

                //    //季度  where Convert.ToDateTime(t.Date.Trim()).CompareTo(Convert.ToDateTime("2009/9/9")) >= 0 && Convert.ToDateTime(t.Date.Trim()).CompareTo(Convert.ToDateTime("2009/10/9")) <= 0) 

                //    var quarterlySpanOrderCoNums = await totalOrders.Where(f => f.POSE_DATE.CompareTo(quarterlySpan) >= 0 && f.POSE_DATE.CompareTo(nowDate) <= 0).Select(f => f.CO_NUM).ToListAsync();
                //    var quarterlySpanOd = await orderDetails.Where(f => f.ITEM_ID == item && quarterlySpanOrderCoNums.Contains(f.CO_NUM)).ToListAsync();
                //    var quarterlySpanQty = quarterlySpanOd.Count == 0 ? 0 : quarterlySpanOd.Sum(t => t.QTY_ORD);

                //    //年度
                //    var yearSpanOrderCoNums = await totalOrders.Where(f => f.POSE_DATE.CompareTo(yearSpan) >= 0 && f.POSE_DATE.CompareTo(nowDate) <= 0).Select(f => f.CO_NUM).ToListAsync();
                //    var yearSpanOd = await orderDetails.Where(f => f.ITEM_ID == item && yearSpanOrderCoNums.Contains(f.CO_NUM)).ToListAsync();
                //    var yearSpanQty = yearSpanOd.Count == 0 ? 0 : yearSpanOd.Sum(t => t.QTY_ORD);

                //    RetailAccount book = new RetailAccount
                //    {
                //        BookDate = sumMonth,
                //        PreDate = preMonth,
                //        ItemCode = gd == null ? string.Empty : gd.ItemCode,
                //        ItemName = gd == null ? string.Empty : gd.Specification,
                //        LicenseCode = retail.LicenseKey,
                //        PreMonthQty = preMothQty.Value,
                //        ThsMonthQty = thisMonthQty.Value,
                //        QuarterlyDate = quarterlySpan,
                //        QuarterlyQty = quarterlySpanQty.Value,
                //        YearDate = yearSpan,
                //        YearQty = yearSpanQty.Value
                //    };
                //    if (book.PreMonthQty != 0 || book.ThsMonthQty != 0 || book.QuarterlyQty != 0 || book.YearQty != 0)
                //    {
                //        result.AccountBooks.Add(book);
                //    }
                //}

                #endregion

                #region 新台账
                if (span == 1)
                {
                    result.MonthAccountBooks = await GetRetailAccountDeAsync(tenantId, userId, 2);
                    result.PreMonthAccountBooks = await GetRetailAccountDeAsync(tenantId, userId, 1);
                }
                else if (span == 3)
                {
                    result.AccountBooks = await GetRetailAccountDeAsync(tenantId, userId, 3);
                }
                else if (span == 6)
                {
                    result.AccountBooks = await GetRetailAccountDeAsync(tenantId, userId, 6);
                }
                #endregion

                return result;
            }
        }

        public string GetDate(int span, bool isDay, string sep = "")
        {
            var year = DateTime.Now.AddMonths(-span).Year.ToString();
            if (DateTime.Now.AddMonths(-span).Month < 10)
            {
                if (isDay)
                {
                    return year + sep + "0" + DateTime.Now.AddMonths(-span).Month.ToString() + sep + "01";
                }
                else
                {
                    return year + sep + "0" + DateTime.Now.AddMonths(-span).Month.ToString();
                }
            }
            else
            {
                if (isDay)
                {
                    return year + sep + DateTime.Now.AddMonths(-span).Month.ToString() + sep + "01";
                }
                else
                {
                    return year + sep + DateTime.Now.AddMonths(-span).Month.ToString();
                }
            }
        }

        /// <summary>
        /// 获取台账
        /// </summary>
        /// <param name="retail"></param>
        /// <param name="span">1：上月2：本月，3：季度，6：半年</param>
        /// <returns></returns>
        public async Task<List<RetailAccountDe>> GetRetailAccountDeAsync(int? tenantId, Guid userId, int span)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var retail = await _retailerService.GetRetailerByIdDtoByLKeyForWeChatAsync(userId);
                var result = new List<RetailAccountDe>();
                if (retail != null)
                {
                    //当月
                    string sumMonth = GetDate(0, false);
                    //上月
                    string preMonth = GetDate(1, false);
                    //季度
                    var nowDate = sumMonth + (DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString());
                    var quarterlySpan = GetDate(2, true);
                    //半年度
                    var yearSpan = GetDate(5, true);
                    var totalOrders = _epcoRepository.GetAll().Where(f => f.CUST_ID == retail.CustId)
                        .WhereIf(span == 1, f => f.POSE_DATE.Contains(preMonth))//上月
                        .WhereIf(span == 2, f => f.POSE_DATE.Contains(sumMonth))//本月
                        .WhereIf(span == 3, f => f.POSE_DATE.CompareTo(quarterlySpan) >= 0 && f.POSE_DATE.CompareTo(nowDate) <= 0)//季度
                        .WhereIf(span == 6, f => f.POSE_DATE.CompareTo(yearSpan) >= 0 && f.POSE_DATE.CompareTo(nowDate) <= 0);//半年

                    //订单号
                    var orderCodes = await totalOrders.Select(t => t.CO_NUM).ToListAsync();

                    //订单下的商品id
                    var orderDetails = _epcolineRepository.GetAll().Where(f => orderCodes.Contains(f.CO_NUM));
                    var groupItems = await orderDetails.GroupBy(f => f.ITEM_ID).ToListAsync();
                    IList<string> orderItemIds = groupItems.Select(f => f.Key).ToList();

                    //根据商品id获取商品信息
                    var goods = await _productRepository.GetAll().Where(f => orderItemIds.Contains(f.ItemId)).ToListAsync();
                    foreach (var item in orderItemIds)
                    {
                        //商品信息
                        var gd = goods.FirstOrDefault(f => f.ItemId == item);

                        //var MonthOrderCoNums = await totalOrders.Select(f => f.CO_NUM).ToListAsync();
                        var MonthOd = await orderDetails.Where(f => f.ITEM_ID == item).ToListAsync();
                        var MonthQty = MonthOd.Count == 0 ? 0 : MonthOd.Sum(t => t.QTY_ORD);
                        RetailAccountDe book = new RetailAccountDe
                        {
                            BookDate = span == 1 ? preMonth : (span == 2 ? sumMonth : (span == 3 ? GetDate(2, false, ".") : (span == 6 ? GetDate(5, false, ".") : ""))),
                            ItemCode = gd == null ? string.Empty : gd.ItemCode,
                            ItemName = gd == null ? string.Empty : gd.Specification,
                            LicenseCode = retail.LicenseKey,
                            MonthQty = MonthQty.Value,
                        };
                        if (book.MonthQty != 0)
                        {
                            result.Add(book);
                        }
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 获取档级信息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<RetailInfoDto> GetRetailBasicInfoAsync(int? tenantId, Guid userId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                #region  基础信息
                RetailInfoDto retailInfo = new RetailInfoDto();
                var retail = await _retailerService.GetRetailerByIdDtoByLKeyForWeChatAsync(userId);
                if (retail != null)
                {
                    var weChatUser = _wechatuserRepository.GetAll().Where(w => w.BindStatus == BindStatusEnum.已绑定 && w.UserId == retail.Id).FirstOrDefaultAsync().Result;
                    retailInfo.Name = retail.Name;
                    retailInfo.CustId = retail.CustId;
                    retailInfo.Code = retail.Code;
                    retailInfo.BusinessAddress = retail.BusinessAddress;
                    retailInfo.HeadImgUrl = weChatUser != null ? weChatUser.HeadImgUrl : string.Empty;
                    retailInfo.LicenseKey = retail.LicenseKey;
                    retailInfo.OpenId = weChatUser != null ? weChatUser.OpenId : string.Empty;
                    retailInfo.VerificationCode = retail.RetailerVerificationCode;
                    retailInfo.IsShopkeeper = weChatUser != null ? weChatUser.IsShopkeeper : null;
                    //计算月订单金额
                    //当月
                    int currentYear = DateTime.Now.Year;
                    int currentMonth = DateTime.Now.Month;
                    string queryMonth = currentYear.ToString();
                    if (currentMonth < 10)
                    {
                        queryMonth = queryMonth + "0" + currentMonth.ToString();
                    }
                    else
                    {
                        queryMonth = queryMonth + currentMonth.ToString();
                    }
                    //上月
                    int preYear = DateTime.Now.AddMonths(-1).Year;
                    int preOdMonth = DateTime.Now.AddMonths(-1).Month;
                    string queryPreMonth = preYear.ToString();
                    if (preOdMonth < 10)
                    {
                        queryPreMonth = queryPreMonth + "0" + preOdMonth.ToString();
                    }
                    else
                    {
                        queryPreMonth = queryPreMonth + preOdMonth.ToString();
                    }
                    //本月订单统计
                    var mothOrders = _epcoRepository.GetAll().Where(f => f.CUST_ID == retail.CustId && f.POSE_DATE.Contains(queryMonth));
                    var mothcount = await mothOrders.CountAsync();
                    retailInfo.MonthOrderMoney = mothcount == 0 ? 0 : await mothOrders.SumAsync(f => f.AMT_SUM.Value);
                    retailInfo.MonthOrderQty = mothcount == 0 ? 0 : (int)await mothOrders.SumAsync(f => f.QTY_SUM);

                    //上月订单统计
                    var premonthOrders = _epcoRepository.GetAll().Where(f => f.CUST_ID == retail.CustId && f.POSE_DATE.Contains(queryPreMonth));
                    var premothcount = await premonthOrders.CountAsync();
                    retailInfo.PreMonthOrderMoney = premothcount == 0 ? 0 : await premonthOrders.SumAsync(f => f.AMT_SUM.Value);
                    retailInfo.PreMonthOrderQty = premothcount == 0 ? 0 : (int)await premonthOrders.SumAsync(f => f.QTY_SUM.Value);

                    //计算川烟量
                    IList<string> siChuanIds = await _productRepository.GetAll().Where(f => f.MfrId == "20510002").Select(t => t.ItemId).ToListAsync();
                    IList<string> coNums = await mothOrders.Select(f => f.CO_NUM).ToListAsync();
                    var siChuanOrders = _epcolineRepository.GetAll().Where(f => coNums.Contains(f.CO_NUM) && siChuanIds.Contains(f.ITEM_ID));
                    retailInfo.SiChuanQty = await siChuanOrders.CountAsync() == 0 ? 0 : (int)await siChuanOrders.SumAsync(f => f.QTY_ORD.Value);

                    //积分
                    var allPoints = _gacustpointRepository.GetAll().Where(f => f.CustId == retail.CustId);//.ToListAsync();//LicenseCode实际指的是CustId（名字取得有误）
                    var count = await allPoints.CountAsync();
                    var totalPoints = count == 0 ? 0 : await allPoints.SumAsync(t => t.Point);
                    int monthPoints = 0;
                    if (count != 0)
                    {
                        var mothPointData = await allPoints.SingleOrDefaultAsync(t => t.Pmonth == queryMonth);
                        monthPoints = mothPointData == null ? 0 : mothPointData.Point;
                    }
                    //积分暂未获得？
                    retailInfo.TotalPoint = totalPoints;
                    retailInfo.MonthPoint = monthPoints;
                    //档级
                    retailInfo.Level = retail.ArchivalLevel;
                    #region 已达档级
                    var gradLevel = await _gagradeRepository.GetAll().Where(f => f.StartPoint <= monthPoints).OrderByDescending(f => f.StartPoint).FirstOrDefaultAsync();

                    retailInfo.CurrentLevel = gradLevel == null ? "1档" : gradLevel.GradeLevel.ToString() + "档";
                    #endregion

                    //result.BasicInfo = retailInfo;
                    #endregion

                }
                return retailInfo;
            }
        }

        #region 导出商品信息
        /// <summary>
        /// 获取Excel数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<List<ProductListDto>> GeProductsNoPage(GetProductsInput input)
        {
            //var query = _productRepository.GetAll()
            //   .WhereIf(!string.IsNullOrEmpty(input.Name), p => p.Specification.Contains(input.Name))
            //   .WhereIf(input.Type.HasValue, p => p.Type == input.Type)
            //   .WhereIf(input.IsRare.HasValue, p => p.IsRare == input.IsRare);
            ////TODO:根据传入的参数添加过滤条件
            //var productCount = await query.CountAsync();

            //var products = await query.ToListAsync();

            ////var productListDtos = ObjectMapper.Map<List <ProductListDto>>(products);
            //var productListDtos = products.MapTo<List<ProductListDto>>();

            //return productListDtos;
            var query = _productRepository.GetAll()
                 .WhereIf(!string.IsNullOrEmpty(input.Name), p => p.Specification.Contains(input.Name))
                .WhereIf(input.Type.HasValue, p => p.Type == input.Type)
                .WhereIf(input.IsRare.HasValue, p => p.IsRare == input.IsRare);
            //TODO:根据传入的参数添加过滤条件
            //var productCount = await query.CountAsync();
            List<Product> products;
            List<ProductListDto> productListDtos;
            if (input.SortValue == "ascend")
            {
                products = await query.OrderByDescending(p => p.SearchCount).ThenBy(input.Sorting).ToListAsync();
                productListDtos = products.MapTo<List<ProductListDto>>();
                return productListDtos;
            }
            else if (input.SortValue == "descend")
            {
                products = await query.OrderBy(p => p.SearchCount).ThenBy(input.Sorting).ToListAsync();
                productListDtos = products.MapTo<List<ProductListDto>>();
                return productListDtos;
            }
            else
            {
                products = await query.OrderBy(p => p.Specification).ThenBy(input.Sorting).ToListAsync();
                productListDtos = products.MapTo<List<ProductListDto>>();
                return productListDtos;
            }
        }

        /// <summary>
        /// 创建Excel
        /// </summary>
        /// <param name="fileName">表名</param>
        /// <param name="data">表数据</param>
        /// <returns></returns>
        private string SaveProductsExcel(string fileName, List<ProductListDto> data)
        {
            var fullPath = ExcelHelper.GetSavePath(_hostingEnvironment.WebRootPath) + fileName;
            using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Employees");
                var rowIndex = 0;
                IRow titleRow = sheet.CreateRow(rowIndex);
                string[] titles = { "商品规格", "商品类型", "指导零售价", "是否是特色商品", "包码", "条码", "搜索次数", "商品id", "商品code", "所属公司", "是否启用" };
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
                    IRow row = sheet.CreateRow(rowIndex);
                    ExcelHelper.SetCell(row.CreateCell(0), font, item.Specification);
                    ExcelHelper.SetCell(row.CreateCell(1), font, item.TypeName);
                    ExcelHelper.SetCell(row.CreateCell(2), font, item.Price.ToString());
                    var isRareStr = item.IsRare.ToString();
                    ExcelHelper.SetCell(row.CreateCell(3), font, (isRareStr == "true" || isRareStr == "True") ? "是" : "否");
                    ExcelHelper.SetCell(row.CreateCell(4), font, item.PackageCode);
                    ExcelHelper.SetCell(row.CreateCell(5), font, item.BarCode);
                    ExcelHelper.SetCell(row.CreateCell(6), font, item.SearchCount.ToString());
                    ExcelHelper.SetCell(row.CreateCell(7), font, item.ItemId);
                    ExcelHelper.SetCell(row.CreateCell(8), font, item.ItemCode);
                    ExcelHelper.SetCell(row.CreateCell(9), font, item.Company);
                    var isActionStr = item.IsAction.ToString();
                    ExcelHelper.SetCell(row.CreateCell(10), font, (isActionStr == "true" || isActionStr == "True") ? "是" : "否");
                }
                workbook.Write(fs);
            }
            return "/files/downloadtemp/" + fileName;
        }

        /// <summary>
        /// 导出商品Excel
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns></returns>
        [UnitOfWork(isTransactional: false)]
        public async Task<APIResultDto> ExportProductsExcel(GetProductsInput input)
        {
            try
            {
                var exportData = await GeProductsNoPage(input);
                var result = new APIResultDto();
                result.Code = 0;
                result.Data = SaveProductsExcel("商品信息.xlsx", exportData);
                return result;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("ExportProductsExcel errormsg{0} Exception{1}", ex.Message, ex);
                return new APIResultDto() { Code = 901, Msg = "网络忙...请待会儿再试！" };
            }
        }
        #endregion

        #region  手动更新档级
        [AbpAllowAnonymous]
        public void UpdateLevel()
        {
            var isUpdate = _levelLogAppService.IsUpdateLevel().Result;
            if (!isUpdate)
            {
                var result = UpdateRetail().Result;
                if (result)
                {
                   _levelLogAppService.CreateSingleLevelLog();
                }
                Logger.InfoFormat("当前更新档级时间：{0}", DateTime.Now);
            }
        }
        [AbpAllowAnonymous]
        public Task<bool> UpdateRetail()
        {
            var pmonth = GetDate(1, false, "");
            var retails =  _retailerRepository.GetAll().Where(r => r.IsAction).ToList();
            var gradeList = _gagradeRepository.GetAll().ToList();
            var gacustpoint = _gacustpointRepository.GetAll().Where(c => c.Pmonth == pmonth).ToList();
           
            foreach (var item in retails)
            {
                var mothPointdates = gacustpoint.SingleOrDefault(c => c.CustId == item.CustId);
                var mothPoint = mothPointdates == null ? 0 : mothPointdates.Point;
                var gradLevel = gradeList.Where(g => g.StartPoint <= mothPoint).OrderByDescending(g => g.StartPoint).FirstOrDefault();
                item.ArchivalLevel = gradLevel == null ? "1" : gradLevel.GradeLevel.ToString();
            }
            return Task.FromResult(true);
        }

        /// <summary>
        /// 微信获取商品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<ProductListDto> GetWXProductByIdAsync(Guid id)
        {
            var entity =await _productRepository.GetAsync(id);
            return entity.MapTo<ProductListDto>();
        }
        #endregion


    }
}


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
using HC.WeChat.ShopProducts.Authorization;
using HC.WeChat.ShopProducts.Dtos;
using HC.WeChat.ShopProducts.DomainServices;
using HC.WeChat.ShopProducts;
using System;
using HC.WeChat.Products;
using HC.WeChat.Authorization;
using HC.WeChat.Dto;

namespace HC.WeChat.ShopProducts
{
    /// <summary>
    /// ShopProduct应用层服务的接口实现方法
    /// </summary>
    //[AbpAuthorize(ShopProductAppPermissions.ShopProduct)]
    [AbpAuthorize(AppPermissions.Pages)]
    public class ShopProductAppService : WeChatAppServiceBase, IShopProductAppService
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        private readonly IRepository<ShopProduct, Guid> _shopproductRepository;
        private readonly IShopProductManager _shopproductManager;
        private readonly IRepository<Product, Guid> _productRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ShopProductAppService(IRepository<ShopProduct, Guid> shopproductRepository
      , IShopProductManager shopproductManager, IRepository<Product, Guid> productRepository

        )
        {
            _shopproductRepository = shopproductRepository;
            _shopproductManager = shopproductManager;
            _productRepository = productRepository;
        }

        /// <summary>
        /// 获取ShopProduct的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ShopProductListDto>> GetPagedShopProducts(GetShopProductsInput input)
        {

            var query = _shopproductRepository.GetAll();
            //TODO:根据传入的参数添加过滤条件
            var shopproductCount = await query.CountAsync();

            var shopproducts = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            //var shopproductListDtos = ObjectMapper.Map<List <ShopProductListDto>>(shopproducts);
            var shopproductListDtos = shopproducts.MapTo<List<ShopProductListDto>>();

            return new PagedResultDto<ShopProductListDto>(
                shopproductCount,
                shopproductListDtos
                );

        }

        /// <summary>
        /// 通过指定id获取ShopProductListDto信息
        /// </summary>
        public async Task<ShopProductListDto> GetShopProductByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _shopproductRepository.GetAsync(input.Id);

            return entity.MapTo<ShopProductListDto>();
        }

        /// <summary>
        /// 导出ShopProduct为excel表
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetShopProductsToExcel(){
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
        public async Task<GetShopProductForEditOutput> GetShopProductForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetShopProductForEditOutput();
            ShopProductEditDto shopproductEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _shopproductRepository.GetAsync(input.Id.Value);

                shopproductEditDto = entity.MapTo<ShopProductEditDto>();

                //shopproductEditDto = ObjectMapper.Map<List <shopproductEditDto>>(entity);
            }
            else
            {
                shopproductEditDto = new ShopProductEditDto();
            }

            output.ShopProduct = shopproductEditDto;
            return output;

        }

        /// <summary>
        /// 添加或者修改ShopProduct的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateShopProduct(CreateOrUpdateShopProductInput input)
        {

            if (input.ShopProduct.Id.HasValue)
            {
                await UpdateShopProductAsync(input.ShopProduct);
            }
            else
            {
                await CreateShopProductAsync(input.ShopProduct);
            }
        }

        /// <summary>
        /// 新增ShopProduct
        /// </summary>
        //[AbpAuthorize(ShopProductAppPermissions.ShopProduct_CreateShopProduct)]
        protected virtual async Task<ShopProductEditDto> CreateShopProductAsync(ShopProductEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var entity = ObjectMapper.Map<ShopProduct>(input);

            entity = await _shopproductRepository.InsertAsync(entity);
            return entity.MapTo<ShopProductEditDto>();
        }

        /// <summary>
        /// 编辑ShopProduct
        /// </summary>
        //[AbpAuthorize(ShopProductAppPermissions.ShopProduct_EditShopProduct)]
        protected virtual async Task UpdateShopProductAsync(ShopProductEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            var entity = await _shopproductRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _shopproductRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除ShopProduct信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[AbpAuthorize(ShopProductAppPermissions.ShopProduct_DeleteShopProduct)]
        public async Task DeleteShopProduct(EntityDto<Guid> input)
        {

            //TODO:删除前的逻辑判断，是否允许删除
            await _shopproductRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除ShopProduct的方法
        /// </summary>
        //[AbpAuthorize(ShopProductAppPermissions.ShopProduct_BatchDeleteShopProducts)]
        public async Task BatchDeleteShopProductsAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _shopproductRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        /// <summary>
        /// 添加或者修改ShopProduct的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateShopProductDto(ShopProductEditDto input)
        {

            if (input.Id.HasValue)
            {
                await UpdateShopProductAsync(input);
            }
            else
            {
                await CreateShopProductAsync(input);
            }
        }

        /// <summary>
        /// 获取ShopProduct的分页列表信息连接Product表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ShopProductListDto>> GetPagedShopProductsReferProducts(GetShopProductsInput input)
        {

            var querySP = _shopproductRepository.GetAll()
                .Where(sp=>sp.ShopId==input.ShopId);
            var queryP = _productRepository.GetAll()
                .Where(p => p.IsRare == true && p.IsAction == true);
            var query = from sp in querySP
                        join p in queryP on sp.ProductId equals p.Id 
                        select new ShopProductListDto
                        {
                            Id = sp.Id,
                            ProductId = sp.ProductId,
                            ShopId = sp.ShopId,
                            Specification = p.Specification,
                            Type = p.Type,
                            Price = p.Price,
                            PackageCode = p.PackageCode,
                            BarCode = p.BarCode,
                            PhotoUrl = p.PhotoUrl
                        };
            //TODO:根据传入的参数添加过滤条件
            var shopproductCount = await query.CountAsync();

            var shopproducts = await query
                .OrderBy(sp=>sp.Specification)
                .ThenBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            //var shopproductListDtos = ObjectMapper.Map<List <ShopProductListDto>>(shopproducts);
            var shopproductListDtos = shopproducts.MapTo<List<ShopProductListDto>>();

            return new PagedResultDto<ShopProductListDto>(
                shopproductCount,
                shopproductListDtos
                );

        }

        /// <summary>
        /// 根据店铺Id 获取特色商品
        /// </summary>
        [AbpAllowAnonymous]
        public async Task<List<ShopProductListDto>> GetShopProductsByShopId(Guid shopId, int? tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var query = from s in _shopproductRepository.GetAll().Where(s => s.ShopId == shopId)
                            join p in _productRepository.GetAll().Where(p => p.IsRare == true && p.IsAction == true) on s.ProductId equals p.Id
                            select new ShopProductListDto
                            {
                                Id = p.Id,
                                ProductId = s.ProductId,
                                ShopId = s.ShopId,
                                Specification = p.Specification,
                                Type = p.Type,
                                Price = p.Price,
                                PackageCode = p.PackageCode,
                                BarCode = p.BarCode,
                                PhotoUrl = p.PhotoUrl,
                                Desc = p.Desc
                            };
                return await query.OrderBy(q => q.Type).ThenBy(q => q.Specification).ToListAsync();
            }
        }

        /// <summary>
        /// 保存店铺产品
        /// </summary>
        [AbpAllowAnonymous]
        public async Task<APIResultDto> SaveShopProducts(BatchSaveShopProductDto input)
        {
            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                var currentProducts = await _shopproductRepository.GetAll().Where(s => s.ShopId == input.ShopId).ToListAsync();
                //删除
                var deleteIds = currentProducts.Where(c => !input.ProductIds.Contains(c.ProductId)).Select(c => c.Id).ToArray();
                await _shopproductRepository.DeleteAsync(s => deleteIds.Contains(s.Id));
                //新增
                var cpIds = currentProducts.Select(c => c.ProductId).ToArray();
                var addIds = input.ProductIds.Where(p => !cpIds.Contains(p)).ToArray();
                foreach (var pid in addIds)
                {
                    await _shopproductRepository.InsertAsync(new ShopProduct() { ProductId = pid, ShopId = input.ShopId });
                }

                await CurrentUnitOfWork.SaveChangesAsync();

                APIResultDto result = new APIResultDto();
                result.Code = 0;
                result.Msg = "提交数据成功";
                result.Data = await GetShopProductsByShopId(input.ShopId, input.TenantId);
                return result;
            }
        }
    }
}


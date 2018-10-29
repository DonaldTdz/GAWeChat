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
using HC.WeChat.ShopEvaluations.Authorization;
using HC.WeChat.ShopEvaluations.Dtos;
using HC.WeChat.ShopEvaluations.DomainServices;
using HC.WeChat.ShopEvaluations;
using System;
using HC.WeChat.PurchaseRecords;
using HC.WeChat.WeChatUsers;
using HC.WeChat.Authorization;
using HC.WeChat.PurchaseRecords.Dtos;
using HC.WeChat.Products;
using HC.WeChat.Products.Dtos;
using HC.WeChat.Dto;
using HC.WeChat.Shops;
using HC.WeChat.Shops.Dtos;
using HC.WeChat.WechatEnums;
using HC.WeChat.IntegralDetails;
using HC.WeChat.MemberConfigs;
using Abp.Auditing;

namespace HC.WeChat.ShopEvaluations
{
    /// <summary>
    /// ShopEvaluation应用层服务的接口实现方法
    /// </summary>
    //[AbpAuthorize(ShopEvaluationAppPermissions.ShopEvaluation)]
    [AbpAuthorize(AppPermissions.Pages)]
    public class ShopEvaluationAppService : WeChatAppServiceBase, IShopEvaluationAppService
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        private readonly IRepository<ShopEvaluation, Guid> _shopevaluationRepository;
        private readonly IShopEvaluationManager _shopevaluationManager;
        private readonly IRepository<PurchaseRecord, Guid> _purchaserecordRepository;
        private readonly IRepository<WeChatUser, Guid> _wechatuserRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<Shop, Guid> _shopRepository;
        private readonly IRepository<IntegralDetail, Guid> _integraldetailRepository;
        private readonly IRepository<MemberConfig, Guid> _memberConfigRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ShopEvaluationAppService(IRepository<ShopEvaluation, Guid> shopevaluationRepository
            , IShopEvaluationManager shopevaluationManager
            , IRepository<PurchaseRecord, Guid> purchaserecordRepository
            , IRepository<WeChatUser, Guid> wechatuserRepository
            , IRepository<Product, Guid> productRepository
            , IRepository<Shop, Guid> shopRepository
            , IRepository<IntegralDetail, Guid> integraldetailRepository
            , IRepository<MemberConfig, Guid> memberConfigRepository

        )
        {
            _integraldetailRepository = integraldetailRepository;
            _shopevaluationRepository = shopevaluationRepository;
            _shopevaluationManager = shopevaluationManager;
            _purchaserecordRepository = purchaserecordRepository;
            _wechatuserRepository = wechatuserRepository;
            _productRepository = productRepository;
            _shopRepository = shopRepository;
            _memberConfigRepository = memberConfigRepository;
        }

        /// <summary>
        /// 获取ShopEvaluation的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ShopEvaluationListDto>> GetPagedShopEvaluations(GetShopEvaluationsInput input)
        {

            var query = _shopevaluationRepository.GetAll();
            //TODO:根据传入的参数添加过滤条件
            var shopevaluationCount = await query.CountAsync();

            var shopevaluations = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            //var shopevaluationListDtos = ObjectMapper.Map<List <ShopEvaluationListDto>>(shopevaluations);
            var shopevaluationListDtos = shopevaluations.MapTo<List<ShopEvaluationListDto>>();

            return new PagedResultDto<ShopEvaluationListDto>(
                shopevaluationCount,
                shopevaluationListDtos
                );

        }

        /// <summary>
        /// 通过指定id获取ShopEvaluationListDto信息
        /// </summary>
        public async Task<ShopEvaluationListDto> GetShopEvaluationByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _shopevaluationRepository.GetAsync(input.Id);

            return entity.MapTo<ShopEvaluationListDto>();
        }

        /// <summary>
        /// 导出ShopEvaluation为excel表
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetShopEvaluationsToExcel(){
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
        public async Task<GetShopEvaluationForEditOutput> GetShopEvaluationForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetShopEvaluationForEditOutput();
            ShopEvaluationEditDto shopevaluationEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _shopevaluationRepository.GetAsync(input.Id.Value);

                shopevaluationEditDto = entity.MapTo<ShopEvaluationEditDto>();

                //shopevaluationEditDto = ObjectMapper.Map<List <shopevaluationEditDto>>(entity);
            }
            else
            {
                shopevaluationEditDto = new ShopEvaluationEditDto();
            }

            output.ShopEvaluation = shopevaluationEditDto;
            return output;

        }

        /// <summary>
        /// 添加或者修改ShopEvaluation的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateShopEvaluation(CreateOrUpdateShopEvaluationInput input)
        {

            if (input.ShopEvaluation.Id.HasValue)
            {
                await UpdateShopEvaluationAsync(input.ShopEvaluation);
            }
            else
            {
                await CreateShopEvaluationAsync(input.ShopEvaluation);
            }
        }

        /// <summary>
        /// 新增ShopEvaluation
        /// </summary>
        //[AbpAuthorize(ShopEvaluationAppPermissions.ShopEvaluation_CreateShopEvaluation)]
        protected virtual async Task<ShopEvaluationEditDto> CreateShopEvaluationAsync(ShopEvaluationEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var entity = ObjectMapper.Map<ShopEvaluation>(input);

            entity = await _shopevaluationRepository.InsertAsync(entity);
            return entity.MapTo<ShopEvaluationEditDto>();
        }

        /// <summary>
        /// 编辑ShopEvaluation
        /// </summary>
        //[AbpAuthorize(ShopEvaluationAppPermissions.ShopEvaluation_EditShopEvaluation)]
        protected virtual async Task UpdateShopEvaluationAsync(ShopEvaluationEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            var entity = await _shopevaluationRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _shopevaluationRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除ShopEvaluation信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[AbpAuthorize(ShopEvaluationAppPermissions.ShopEvaluation_DeleteShopEvaluation)]
        public async Task DeleteShopEvaluation(EntityDto<Guid> input)
        {

            //TODO:删除前的逻辑判断，是否允许删除
            await _shopevaluationRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除ShopEvaluation的方法
        /// </summary>
        //[AbpAuthorize(ShopEvaluationAppPermissions.ShopEvaluation_BatchDeleteShopEvaluations)]
        public async Task BatchDeleteShopEvaluationsAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _shopevaluationRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 获取ShopEvaluation的分页列表信息联合购买记录表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ShopEvaluationListDto>> GetPagedShopEvaluationsByPurchaseRecord(GetShopEvaluationsInput input)
        {
            //if (input.ShopId == Guid.Empty)
            //{
            //    return new PagedResultDto<ShopEvaluationListDto>(0, new List<ShopEvaluationListDto>());
            //}
            var queryEvaluation = _shopevaluationRepository.GetAll()
                .Where(e => e.ShopId == input.ShopId)
                .WhereIf(input.Evaluation.HasValue, e => e.Evaluation == input.Evaluation);
            var queryPurchase = _purchaserecordRepository.GetAll();
            var queryWeChat = _wechatuserRepository.GetAll();
            var query = from e in queryEvaluation
                        join p in queryPurchase on e.PurchaseRecordId equals p.Id into queryE
                        from ep in queryE.DefaultIfEmpty()
                        join w in queryWeChat on ep.OpenId equals w.OpenId into queryP
                        from epw in queryP.DefaultIfEmpty()
                        select new ShopEvaluationListDto
                        {
                            Id = e.Id,
                            PurchaseRecordId = e.PurchaseRecordId,
                            Specification = ep != null ? ep.Specification : "",
                            Quantity = ep != null ? ep.Quantity : null,
                            ShopId = e.ShopId,
                            OpenId = e.OpenId,
                            Evaluation = e.Evaluation,
                            IsCorrectQuantity = e.IsCorrectQuantity,
                            Content = e.Content,
                            CreationTime = e.CreationTime,
                            TenantId = e.TenantId,
                            NickName = epw != null ? epw.NickName : ""
                        };

            //TODO:根据传入的参数添加过滤条件
            var shopevaluationCount = await query.CountAsync();

            var shopevaluations = await query
                .OrderByDescending(e => e.CreationTime)
                .ThenBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            //var shopevaluationListDtos = ObjectMapper.Map<List <ShopEvaluationListDto>>(shopevaluations);
            var shopevaluationListDtos = shopevaluations.MapTo<List<ShopEvaluationListDto>>();

            return new PagedResultDto<ShopEvaluationListDto>(
                shopevaluationCount,
                shopevaluationListDtos
                );

        }

        /// <summary>
        /// 微信根据openId获取当前用户未评价过的商品记录（不分页）
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        //[AbpAllowAnonymous]
        //[DisableAuditing]
        //public async Task<List<PurchaseRecordListDto>> GetWXNotEvaluationByIdAsync(int? tenantId, string openId)
        //{
        //    using (CurrentUnitOfWork.SetTenantId(tenantId))
        //    {
        //        var records = from pr in _purchaserecordRepository.GetAll().Where(p => p.OpenId == openId)
        //                      select new PurchaseRecordListDto()
        //                      {
        //                          Id = pr.Id,
        //                          CreationTime = pr.CreationTime,
        //                          OpenId = pr.OpenId,
        //                          ShopName = pr.ShopName,
        //                          Specification = pr.Specification,
        //                          Quantity = pr.Quantity,
        //                          ProductId = pr.ProductId,
        //                      };
        //        var products = from p in _productRepository.GetAll()
        //                       select new ProductListDto()
        //                       {
        //                           Id = p.Id,
        //                           PhotoUrl = p.PhotoUrl
        //                       };
        //        var entity = from pr in records
        //                     join p in products on pr.ProductId equals p.Id
        //                     select new PurchaseRecordListDto()
        //                     {
        //                         Id = pr.Id,
        //                         CreationTime = pr.CreationTime,
        //                         OpenId = pr.OpenId,
        //                         ShopName = pr.ShopName,
        //                         Specification = pr.Specification,
        //                         Quantity = pr.Quantity,
        //                         ProductId = pr.ProductId,
        //                         PhotoUrl = p.PhotoUrl
        //                     };
        //        //评价表找出PRid
        //        var ePRidList = _shopevaluationRepository.GetAll().Where(e=>e.OpenId==openId).Select(e => e.PurchaseRecordId);
        //        //记录表找出PRid 
        //        var PRidList = entity.Select(e => e.Id);
        //        //找出没评价的实体列表
        //        var finallyEntity = from p in PRidList
        //                      join pr in entity on p equals pr.Id
        //                      where !(ePRidList).Contains(pr.Id)
        //                      select new PurchaseRecordListDto()
        //                      {
        //                          Id = pr.Id,
        //                          CreationTime = pr.CreationTime,
        //                          Integral = pr.Integral,
        //                          OpenId = pr.OpenId,
        //                          ShopName = pr.ShopName,
        //                          Specification = pr.Specification,
        //                          Quantity = pr.Quantity,
        //                          ProductId = pr.ProductId,
        //                          PhotoUrl = pr.PhotoUrl
        //                      };
        //        return await finallyEntity.OrderByDescending(v => v.CreationTime).ToListAsync();
        //    }
        //}

        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<List<PurchaseRecordListDto>> GetWXNotEvaluationByIdAsync(int? tenantId, string openId, int pageIndex, int pageSize)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var records = from pr in _purchaserecordRepository.GetAll().Where(p => p.OpenId == openId)
                              select new PurchaseRecordListDto()
                              {
                                  Id = pr.Id,
                                  CreationTime = pr.CreationTime,
                                  OpenId = pr.OpenId,
                                  ShopName = pr.ShopName,
                                  Specification = pr.Specification,
                                  Quantity = pr.Quantity,
                                  ProductId = pr.ProductId,
                              };
                var products = from p in _productRepository.GetAll()
                               select new ProductListDto()
                               {
                                   Id = p.Id,
                                   PhotoUrl = p.PhotoUrl
                               };
                var entity = from pr in records
                             join p in products on pr.ProductId equals p.Id
                             select new PurchaseRecordListDto()
                             {
                                 Id = pr.Id,
                                 CreationTime = pr.CreationTime,
                                 OpenId = pr.OpenId,
                                 ShopName = pr.ShopName,
                                 Specification = pr.Specification,
                                 Quantity = pr.Quantity,
                                 ProductId = pr.ProductId,
                                 PhotoUrl = p.PhotoUrl
                             };
                //评价表找出PRid
                var ePRidList = _shopevaluationRepository.GetAll().Where(e => e.OpenId == openId).Select(e => e.PurchaseRecordId);
                //记录表找出PRid 
                var PRidList = entity.Select(e => e.Id);
                //找出没评价的实体列表
                var finallyEntity = from p in PRidList
                                    join pr in entity on p equals pr.Id
                                    where !(ePRidList).Contains(pr.Id)
                                    select new PurchaseRecordListDto()
                                    {
                                        Id = pr.Id,
                                        CreationTime = pr.CreationTime,
                                        Integral = pr.Integral,
                                        OpenId = pr.OpenId,
                                        ShopName = pr.ShopName,
                                        Specification = pr.Specification,
                                        Quantity = pr.Quantity,
                                        ProductId = pr.ProductId,
                                        PhotoUrl = pr.PhotoUrl
                                    };
                return await finallyEntity.OrderByDescending(v => v.CreationTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            }
        }

        /// <summary>
        /// 查询未评价的个数
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>

        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<int> GetWXCountNotEvaluationByIdAsync(int? tenantId, string openId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var records = _purchaserecordRepository.GetAll().Where(r => r.OpenId == openId)
                              .Select(r => r.Id);
                var ePRidList = _shopevaluationRepository.GetAll().Where(e => e.OpenId == openId).Select(e => e.PurchaseRecordId);
                var finallyEntity = from p in records
                                    where !(ePRidList).Contains(p)
                                    select new PurchaseRecordListDto()
                                    {
                                        Id = p,
                                    };
                return await finallyEntity.CountAsync();
            }
        }


        /// <summary>
        /// 查看评价
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="shopEvaluationId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<ShopEvaluationListDto> GetWXEvaluationByIdAsync(int? tenantId, Guid? Id)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var query = _shopevaluationRepository.GetAll().Where(e => e.PurchaseRecordId== Id);
                var finallyEntity = from se in query                              
                                    select new ShopEvaluationListDto()
                                    {
                                        Id=se.Id,
                                        IsCorrectQuantity =se.IsCorrectQuantity,
                                        Evaluation = se.Evaluation,
                                        CreationTime =se.CreationTime,
                                        PurchaseRecordId=se.PurchaseRecordId
                                    };
                return await finallyEntity.FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// 商品详情-评价
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="openId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<PurchaseRecordListDto> GetWXProductsDetailsByIdAsync(int? tenantId, string openId, Guid? productId,Guid?id)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var query = _purchaserecordRepository.GetAll().Where(p => p.OpenId == openId && p.ProductId==productId&&p.Id==id);
                var records = from pr in query
                              select new PurchaseRecordListDto()
                              {
                                  Id = pr.Id,
                                  CreationTime = pr.CreationTime,
                                  OpenId = pr.OpenId,
                                  ShopName = pr.ShopName,
                                  Specification = pr.Specification,
                                  Quantity = pr.Quantity,
                                  ProductId = pr.ProductId,
                                  Integral =pr.Integral,
                                  ShopId =pr.ShopId,
                                  IsEvaluation =pr.IsEvaluation
                              };
                var products = from p in _productRepository.GetAll()
                               select new ProductListDto()
                               {
                                   Id = p.Id,
                                   PhotoUrl = p.PhotoUrl
                               };
                var entity = from pr in records
                             join p in products on pr.ProductId equals p.Id
                             select new PurchaseRecordListDto()
                             {
                                 Id = pr.Id,
                                 CreationTime = pr.CreationTime,
                                 Integral = pr.Integral,
                                 OpenId = pr.OpenId,
                                 ShopName = pr.ShopName,
                                 Specification = pr.Specification,
                                 Quantity = pr.Quantity,
                                 ProductId = pr.ProductId,
                                 PhotoUrl = p.PhotoUrl,
                                 ShopId =pr.ShopId,
                                 IsEvaluation =pr.IsEvaluation
                             };
                return await entity.FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// 提交评价信息并更新店铺评价&购买记录&积分详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<APIResultDto> SubmitShopEvaluationAsync(ShopEvaluation input)
        {
            //新增评价
            var result = input.MapTo<ShopEvaluation>();
            result.Id = Guid.NewGuid();
            await _shopevaluationRepository.InsertAsync(result);
            //修改店铺评价
            var shopEntity = _shopRepository.GetAll().Where(s => s.Id == input.ShopId).FirstOrDefault();     
            var evaluationIds = shopEntity.Evaluation.Split(',');
            int[] intEvaluationIds = Array.ConvertAll<string, int>(evaluationIds, s => int.Parse(s));
            if (input.Evaluation == ScoreLevelEmun.好)
            {
                intEvaluationIds[0]++;
            }
            else if (input.Evaluation == ScoreLevelEmun.中)
            {
                intEvaluationIds[1]++;
            }
            else
            {
                intEvaluationIds[2]++;
            }
            string evaluation = intEvaluationIds[0].ToString() + ',' + intEvaluationIds[1].ToString() + ',' + intEvaluationIds[2].ToString();
            shopEntity.Evaluation = evaluation;
            await _shopRepository.UpdateAsync(shopEntity);
            //更新购买记录
            var record = _purchaserecordRepository.GetAll().Where(pr => pr.Id == input.PurchaseRecordId).FirstOrDefault();
            record.IsEvaluation = true;
            await _purchaserecordRepository.UpdateAsync(record);
            //新增积分详情
            var config = await GetIntegralConfig(input.TenantId);
            var user = await _wechatuserRepository.GetAll().Where(u => u.OpenId == input.OpenId).FirstOrDefaultAsync();
            var intDetail = new IntegralDetail();
            intDetail.InitialIntegral = user.IntegralTotal;
            intDetail.Integral = int.Parse(config);
            intDetail.FinalIntegral = user.IntegralTotal + intDetail.Integral;
            intDetail.OpenId = user.OpenId;
            intDetail.RefId = result.Id.ToString();
            intDetail.TenantId = input.TenantId;
            intDetail.Type = IntegralTypeEnum.评价店铺赠送;
            intDetail.Desc = "评价店铺赠送";
            await _integraldetailRepository.InsertAsync(intDetail);
            //更新用户总积分
            user.IntegralTotal = intDetail.FinalIntegral.Value;
            await _wechatuserRepository.UpdateAsync(user);
            return new APIResultDto() { Code = 0, Msg = "提交成功，您的评价已生效" };
        }

        private async Task<string> GetIntegralConfig(int? tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                //获取积分配置
                var configValue = await _memberConfigRepository.GetAll().Where(c => c.Type == DeployTypeEnum.积分配置&&c.Code==DeployCodeEnum.商品评价).Select(c=>c.Value).FirstOrDefaultAsync();
                if (configValue == null)
                {
                    configValue = "5";  
                }
                return configValue;
            }
        }
    }
}


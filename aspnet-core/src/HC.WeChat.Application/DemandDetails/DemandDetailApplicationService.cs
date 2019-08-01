
using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Abp.UI;
using Abp.AutoMapper;
using Abp.Extensions;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using HC.WeChat.DemandDetails.Dtos;
using HC.WeChat.DemandDetails.DomainService;
using HC.WeChat.Authorization;
using HC.WeChat.Dto;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using HC.WeChat.WeChatUsers;
using HC.WeChat.Retailers;
using HC.WeChat.ForecastRecords;

namespace HC.WeChat.DemandDetails
{
    /// <summary>
    /// DemandDetail应用层服务的接口实现方法  
    ///</summary>
    //[AbpAuthorize]
    [AbpAuthorize(AppPermissions.Pages)]
    public class DemandDetailAppService : WeChatAppServiceBase, IDemandDetailAppService
    {
        private readonly IRepository<DemandDetail, Guid> _entityRepository;
        private readonly IRepository<WeChatUser, Guid> _wechatuserRepository;
        private readonly IRepository<ForecastRecord, Guid> _forecastRecordRepository;
        private readonly IRetailerRepository _retailerRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IDemandDetailManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public DemandDetailAppService(
        IRepository<DemandDetail, Guid> entityRepository
        , IRepository<WeChatUser, Guid> wechatuserRepository
        , IRepository<ForecastRecord, Guid> forecastRecordRepository
        , IRetailerRepository retailerRepository
        , IDemandDetailManager entityManager
        , IHostingEnvironment hostingEnvironment
        )
        {
            _entityRepository = entityRepository;
            _wechatuserRepository = wechatuserRepository;
            _retailerRepository = retailerRepository;
            _forecastRecordRepository = forecastRecordRepository;
            _entityManager = entityManager;
            _hostingEnvironment = hostingEnvironment;
        }


        /// <summary>
        /// 获取DemandDetail的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<DemandDetailListDto>> GetPaged(GetDemandDetailsInput input)
        {
            var query = _entityRepository.GetAll().Where(v => v.DemandForecastId == input.DemandForecastId);
            var count = await query.CountAsync();
            var entityList = await query
                    .OrderBy(v => v.RetailerName).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();
            var entityListDtos = entityList.MapTo<List<DemandDetailListDto>>();
            return new PagedResultDto<DemandDetailListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取DemandDetailListDto信息
        /// </summary>

        public async Task<DemandDetailListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<DemandDetailListDto>();
        }

        /// <summary>
        /// 获取编辑 DemandDetail
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetDemandDetailForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetDemandDetailForEditOutput();
            DemandDetailEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<DemandDetailEditDto>();

                //demandDetailEditDto = ObjectMapper.Map<List<demandDetailEditDto>>(entity);
            }
            else
            {
                editDto = new DemandDetailEditDto();
            }

            output.DemandDetail = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改DemandDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task CreateOrUpdate(CreateOrUpdateDemandDetailInput input)
        {

            if (input.DemandDetail.Id.HasValue)
            {
                await Update(input.DemandDetail);
            }
            else
            {
                await Create(input.DemandDetail);
            }
        }


        /// <summary>
        /// 新增DemandDetail
        /// </summary>

        protected virtual async Task<DemandDetailEditDto> Create(DemandDetailEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <DemandDetail>(input);
            var entity = input.MapTo<DemandDetail>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<DemandDetailEditDto>();
        }

        /// <summary>
        /// 编辑DemandDetail
        /// </summary>

        protected virtual async Task Update(DemandDetailEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除DemandDetail信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task Delete(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除DemandDetail的方法
        /// </summary>

        public async Task BatchDelete(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 导入需求预测数据
        /// </summary>
        /// <returns></returns>
        public async Task<APIResultDto> ImportDemandDetailExcelAsync(ImportDto input)
        {
            //获取Excel数据
            var excelList = await GetDemandDetailDataAsync();
            //循环批量更新
            await UpdateDemandDetailDataAsync(excelList, input.DemandForecastId);
            return new APIResultDto() { Code = 0, Msg = "导入数据成功" };
        }

        /// <summary>
        /// 从上传的Excel读出数据
        /// </summary>
        private async Task<List<DemandDetailListDto>> GetDemandDetailDataAsync()
        {
            string fileName = _hostingEnvironment.WebRootPath + "/upload/files/DemandDetailUpload.xlsx";
            var demandList = new List<DemandDetailListDto>();
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);
                ISheet sheet = workbook.GetSheet("DemandDetailData");
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

                        var demandData = new DemandDetailListDto();
                        if (row.GetCell(0) != null)
                        {
                            demandData.RetailerCode = row.GetCell(0).ToString();
                            demandData.RetailerName = row.GetCell(1).ToString();
                            demandData.Name = row.GetCell(2).ToString();
                            demandData.SuggestPrice = decimal.Parse(row.GetCell(3).ToString());
                            demandData.WholesalePrice = decimal.Parse(row.GetCell(4).ToString());
                            demandData.YearOnYear = decimal.Parse(row.GetCell(5).ToString());
                            demandData.LastMonthNum = int.Parse(row.GetCell(6).ToString());
                            //demandData.IsAlien =  Boolean.Parse(row.GetCell(7).ToString());
                            //demandData.Type = int.Parse(row.GetCell(8).ToString());
                            demandList.Add(demandData);
                        }
                    }
                }
                return await Task.FromResult(demandList);
            }
        }

        /// <summary>
        /// 更新到数据库
        /// </summary>
        private async Task UpdateDemandDetailDataAsync(List<DemandDetailListDto> excelList, Guid id)
        {
            //var rlcodes = excelList.Select(r => r.RetailerCode).ToArray();
            //var retailerList = await _entityRepository.GetAll().Where(r => rlcodes.Contains(r.RetailerCode)).ToListAsync();
            foreach (var item in excelList)
            {
                var entity = new DemandDetail();
                //var retailer = retailerList.Where(r => r.RetailerCode == item.RetailerCode).FirstOrDefault();
                //if (retailer != null)
                //{
                entity.DemandForecastId = id;
                entity.RetailerCode = item.RetailerCode;
                entity.RetailerName = item.RetailerName;
                entity.Name = item.Name;
                entity.SuggestPrice = item.SuggestPrice;
                entity.WholesalePrice = item.WholesalePrice;
                entity.YearOnYear = item.YearOnYear;
                entity.LastMonthNum = item.LastMonthNum;
                entity.IsAlien = item.IsAlien;
                entity.Type = item.Type;
                await _entityRepository.InsertAsync(entity);
                //}
            }
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 微信获取当前用户需求列表
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<List<DetailWXListDto>> GetWXDetailListByIdAsync(GetWXDetailListDto input)
        {
            Guid? userId = await _wechatuserRepository.GetAll().Where(v => v.OpenId == input.OpenId).Select(v => v.UserId).FirstOrDefaultAsync();
            string retailCode = await _retailerRepository.GetAll().Where(v => v.Id == userId).Select(v => v.Code).FirstOrDefaultAsync();
            var query = _entityRepository.GetAll().Where(v => v.DemandForecastId == input.DemandForecastId && v.RetailerCode == retailCode);
            var list = await (from q in query
                              select new DetailWXListDto()
                              {
                                  Id = q.Id,
                                  LastMonthNum = q.LastMonthNum,
                                  Name = q.Name,
                                  PredictiveValue = 0
                              }).ToListAsync();
            return list;
        }

        /// <summary>
        /// 微信查看当前用户填写记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<List<DetailWXListDto>> GetWXDetailRecordByIdAsync(GetWXDetailListDto input)
        {
            Guid? userId = await _wechatuserRepository.GetAll().Where(v => v.OpenId == input.OpenId).Select(v => v.UserId).FirstOrDefaultAsync();
            string retailCode = await _retailerRepository.GetAll().Where(v => v.Id == userId).Select(v => v.Code).FirstOrDefaultAsync();
            var query = _entityRepository.GetAll().Where(v => v.DemandForecastId == input.DemandForecastId && v.RetailerCode == retailCode);
            var record = _forecastRecordRepository.GetAll().Where(v => v.OpenId == input.OpenId && v.DemandForecastId == input.DemandForecastId);
            var list = await (from q in query
                              join r in record on q.Id equals r.DemandDetailId
                              select new DetailWXListDto()
                              {
                                  Id = q.Id,
                                  LastMonthNum = q.LastMonthNum,
                                  Name = q.Name,
                                  PredictiveValue = r.PredictiveValue
                              }).ToListAsync();
            return list;
        }

        /// <summary>
        /// 查询当前零售户预测需求记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<RetailDemandDetailListDto>> GetDetailRecordByIdAsync(GetDemandDetailsInput input)
        {
            string openId = await _wechatuserRepository.GetAll().Where(v => v.UserId == input.UserId).Select(v => v.OpenId).FirstOrDefaultAsync();
            var query = _entityRepository.GetAll().Where(v => v.DemandForecastId == input.DemandForecastId);
            var record = _forecastRecordRepository.GetAll().Where(v => v.DemandForecastId == input.DemandForecastId && v.OpenId == openId);
            var list = await (from q in query
                              join r in record on q.Id equals r.DemandDetailId
                              select new RetailDemandDetailListDto()
                              {
                                  Id = r.Id,
                                  LastMonthNum = q.LastMonthNum,
                                  Name = q.Name,
                                  SuggestPrice =q.SuggestPrice,
                                  IsAlien = q.IsAlien,
                                  WholesalePrice = q.WholesalePrice,
                                  YearOnYear = q.YearOnYear,
                                  PredictiveValue = r.PredictiveValue
                              }).OrderByDescending(v=>v.PredictiveValue).PageBy(input).ToListAsync();
            int count = record.Count();
            return new PagedResultDto<RetailDemandDetailListDto>(count, list);
        }
    }
}
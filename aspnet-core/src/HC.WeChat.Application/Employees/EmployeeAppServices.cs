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
using HC.WeChat.Employees.Authorization;
using HC.WeChat.Employees.Dtos;
using HC.WeChat.Employees.DomainServices;
using HC.WeChat.Employees;
using System;
using HC.WeChat.Authorization;
using HC.WeChat.WechatEnums;
using HC.WeChat.Authorization.Roles;
using HC.WeChat.Authorization.Users;
using HC.WeChat.Helpers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using HC.WeChat.Dto;
using Abp.Domain.Uow;
using Abp.Auditing;
using HC.WeChat.LuckySigns;

namespace HC.WeChat.Employees
{
    /// <summary>
    /// Employee应用层服务的接口实现方法
    /// </summary>
    //[AbpAuthorize(EmployeeAppPermissions.Employee)]
    [AbpAuthorize(AppPermissions.Pages)]
    public class EmployeeAppService : WeChatAppServiceBase, IEmployeeAppService
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        private readonly IRepository<Employee, Guid> _employeeRepository;
        private readonly IEmployeeManager _employeeManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IRepository<LuckySign, Guid> _LuckySignRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EmployeeAppService(IRepository<Employee, Guid> employeeRepository
            , IEmployeeManager employeeManager, IHostingEnvironment hostingEnvironment
            , IRepository<LuckySign, Guid> LuckySignRepository
        )
        {
            _employeeRepository = employeeRepository;
            _employeeManager = employeeManager;
            _hostingEnvironment = hostingEnvironment;
            _LuckySignRepository = LuckySignRepository;

        }

        /// <summary>
        /// 获取Employee的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<EmployeeListDto>> GetPagedEmployees(GetEmployeesInput input)
        {
            var mid = UserManager.GetControlEmployeeId();
            var query = _employeeRepository.GetAll()
                  .WhereIf(!string.IsNullOrEmpty(input.Filter) && input.Filter != "null", e => e.Name.Contains(input.Filter) || e.Code.Contains(input.Filter))
                  .WhereIf(input.Position.HasValue, e => e.Position == input.Position)
                  .WhereIf(mid.HasValue, e => e.Id == mid);
            //TODO:根据传入的参数添加过滤条件
            var employeeCount = await query.CountAsync();

            var employees = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            //var employeeListDtos = ObjectMapper.Map<List <EmployeeListDto>>(employees);
            var employeeListDtos = employees.MapTo<List<EmployeeListDto>>();

            return new PagedResultDto<EmployeeListDto>(
                employeeCount,
                employeeListDtos
                );

        }

        /// <summary>
        /// 通过指定id获取EmployeeListDto信息
        /// </summary>
        public async Task<EmployeeListDto> GetEmployeeByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _employeeRepository.GetAsync(input.Id);

            return entity.MapTo<EmployeeListDto>();
        }

        /// <summary>
        /// 导出Employee为excel表
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetEmployeesToExcel(){
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
        public async Task<GetEmployeeForEditOutput> GetEmployeeForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetEmployeeForEditOutput();
            EmployeeEditDto employeeEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _employeeRepository.GetAsync(input.Id.Value);

                employeeEditDto = entity.MapTo<EmployeeEditDto>();

                //employeeEditDto = ObjectMapper.Map<List <employeeEditDto>>(entity);
            }
            else
            {
                employeeEditDto = new EmployeeEditDto();
            }

            output.Employee = employeeEditDto;
            return output;

        }

        /// <summary>
        /// 添加或者修改Employee的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateEmployee(CreateOrUpdateEmployeeInput input)
        {

            if (input.Employee.Id.HasValue)
            {
                await UpdateEmployeeAsync(input.Employee);
            }
            else
            {
                await CreateEmployeeAsync(input.Employee);
            }
        }

        /// <summary>
        /// 新增Employee
        /// </summary>
        //[AbpAuthorize(EmployeeAppPermissions.Employee_CreateEmployee)]
        protected virtual async Task<EmployeeEditDto> CreateEmployeeAsync(EmployeeEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var entity = ObjectMapper.Map<Employee>(input);
            entity.TenantId = AbpSession.TenantId;
            entity = await _employeeRepository.InsertAsync(entity);
            return entity.MapTo<EmployeeEditDto>();
        }

        /// <summary>
        /// 编辑Employee
        /// </summary>
        //[AbpAuthorize(EmployeeAppPermissions.Employee_EditEmployee)]
        protected virtual async Task UpdateEmployeeAsync(EmployeeEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            var entity = await _employeeRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _employeeRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除Employee信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[AbpAuthorize(EmployeeAppPermissions.Employee_DeleteEmployee)]
        public async Task DeleteEmployee(EntityDto<Guid> input)
        {

            //TODO:删除前的逻辑判断，是否允许删除
            await _employeeRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除Employee的方法
        /// </summary>
        //[AbpAuthorize(EmployeeAppPermissions.Employee_BatchDeleteEmployees)]
        public async Task BatchDeleteEmployeesAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _employeeRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 用于员工模态框的数据获取
        /// </summary>
        /// <param name="input">员工姓名或编号</param>
        /// <returns></returns>
        public async Task<PagedResultDto<EmployeeListDto>> GetPagedEmployeesModal(GetEmployeesInput input)
        {
            var query = _employeeRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(input.Filter), e => e.Name.Contains(input.Filter) || e.Code.Contains(input.Filter))
                .WhereIf(input.IsManger, e => e.Position == UserPositionEnum.客户经理);

            //TODO:根据传入的参数添加过滤条件
            var employeeCount = await query.CountAsync();
            input.MaxResultCount = 10;
            input.SkipCount = 0;
            var employees = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            //var employeeListDtos = ObjectMapper.Map<List <EmployeeListDto>>(employees);
            var employeeListDtos = employees.MapTo<List<EmployeeListDto>>();

            return new PagedResultDto<EmployeeListDto>(
                employeeCount,
                employeeListDtos
                );
        }
        /// <summary>
        /// 添加或者修改Employee的方法
        /// </summary>
        /// <param name="input">员工信息实体</param>
        /// <returns></returns>
        public async Task CreateOrUpdateEmployeeDto(EmployeeEditDto input)
        {

            if (input.Id.HasValue)
            {
                await UpdateEmployeeAsync(input);
            }
            else
            {
                await CreateEmployeeAsync(input);
            }
        }
        /// <summary>
        /// 检查零售户编码是否可用
        /// </summary>
        /// <returns></returns>
        public bool CheckCode(string code, Guid? id)
        {
            var count = _employeeRepository.GetAll().Where(e => e.Code == code).Count();
            var entity = _employeeRepository.GetAll().Where(e => e.Id == id).FirstOrDefault();
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


        #region 导出员工Excel

        /// <summary>
        /// 获取Excel数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<List<EmployeeListDto>> GeEmployeesNoPage(GetEmployeesInput input)
        {
            var mid = UserManager.GetControlEmployeeId();
            var query = _employeeRepository.GetAll()
                  .WhereIf(!string.IsNullOrEmpty(input.Filter) && input.Filter != "null", e => e.Name.Contains(input.Filter) || e.Code.Contains(input.Filter))
                  .WhereIf(input.Position.HasValue, e => e.Position == input.Position)
                  .WhereIf(mid.HasValue, e => e.Id == mid);
            //TODO:根据传入的参数添加过滤条件

            var employees = await query
                .OrderBy(input.Sorting)
                .ToListAsync();

            var employeeListDtos = employees.MapTo<List<EmployeeListDto>>();

            return employeeListDtos;
        }

        /// <summary>
        /// 创建Excel
        /// </summary>
        /// <param name="fileName">表名</param>
        /// <param name="data">表数据</param>
        /// <returns></returns>
        private string SaveEmployeeExcel(string fileName, List<EmployeeListDto> data)
        {
            var fullPath = ExcelHelper.GetSavePath(_hostingEnvironment.WebRootPath) + fileName;
            using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Employees");
                var rowIndex = 0;
                IRow titleRow = sheet.CreateRow(rowIndex);
                string[] titles = { "员工编码", "姓名", "职位", "电话", "所属公司", "所属市场部门", "是否启用" };
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
                    ExcelHelper.SetCell(row.CreateCell(0), font, item.Code);
                    ExcelHelper.SetCell(row.CreateCell(1), font, item.Name);
                    ExcelHelper.SetCell(row.CreateCell(2), font, item.PositionName);
                    ExcelHelper.SetCell(row.CreateCell(3), font, item.Phone);
                    ExcelHelper.SetCell(row.CreateCell(4), font, item.Company);
                    ExcelHelper.SetCell(row.CreateCell(5), font, item.Department);
                    ExcelHelper.SetCell(row.CreateCell(6), font, item.IsAction ? "启用" : "禁用");

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
        public async Task<APIResultDto> ExportEmployeesExcel(GetEmployeesInput input)
        {
            try
            {
                var exportData = await GeEmployeesNoPage(input);
                var result = new APIResultDto();
                result.Code = 0;
                result.Data = SaveEmployeeExcel("公司员工.xlsx", exportData);
                return result;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("ExportEmployeesExcel errormsg{0} Exception{1}", ex.Message, ex);
                return new APIResultDto() { Code = 901, Msg = "网络忙...请待会儿再试！" };
            }
        }
        /// <summary>
        /// 分组部门名字
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<List<string>> GetEmployeeNameListAsyn() 
        {
            var listStr =  _employeeRepository.GetAll().GroupBy(v => new { v.DeptName })
                            .Select(m =>
                                m.Key.DeptName
                            );
            return await listStr.ToListAsync();        
        }

        /// <summary>
        /// 获取签到情况
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<List<GetEmployeeDetailByDeptOutput>> GetSignListByDeptNameAsync(string deptName) 
        {
            var query = from e in _employeeRepository.GetAll().Where(v => v.DeptName == deptName)
                        join l in _LuckySignRepository.GetAll()
                        on e.Id equals l.UserId into table
                        from el in table.DefaultIfEmpty()
                        select new GetEmployeeDetailByDeptOutput()
                        {
                            Name = e.Name,
                            Code = e.Code,
                            IsSign=el.Id==null?false:true
                        };

            return await query.ToListAsync();

        }


        #endregion
    }
}


using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HC.WeChat.Employees.Dtos;
using HC.WeChat.Employees;
using System;

namespace HC.WeChat.Employees
{
    /// <summary>
    /// Employee应用层服务的接口方法
    /// </summary>
    public interface IEmployeeAppService : IApplicationService
    {
        /// <summary>
        /// 获取Employee的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<EmployeeListDto>> GetPagedEmployees(GetEmployeesInput input);

        /// <summary>
        /// 通过指定id获取EmployeeListDto信息
        /// </summary>
        Task<EmployeeListDto> GetEmployeeByIdAsync(EntityDto<Guid> input);

        /// <summary>
        /// 导出Employee为excel表
        /// </summary>
        /// <returns></returns>
        //  Task<FileDto> GetEmployeesToExcel();
        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetEmployeeForEditOutput> GetEmployeeForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetEmployeeForEditOutput
        /// <summary>
        /// 添加或者修改Employee的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateEmployee(CreateOrUpdateEmployeeInput input);

        /// <summary>
        /// 删除Employee信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteEmployee(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除Employee
        /// </summary>
        Task BatchDeleteEmployeesAsync(List<Guid> input);

        /// <summary>
        /// 获取员工信息用于模态框
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<EmployeeListDto>> GetPagedEmployeesModal(GetEmployeesInput input);

        /// <summary>
        /// 添加或者修改Employee的方法
        /// </summary>
        /// <param name="input">员工信息实体</param>
        /// <returns></returns>
        Task CreateOrUpdateEmployeeDto(EmployeeEditDto input);

        /// <summary>
        /// 检查零售户编码是否可用
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        bool CheckCode(string code,Guid? id);

        /// <summary>
        /// 获取部门表
        /// </summary>
        /// <returns></returns>
        Task<List<EmployeeTotalDto>> GetEmployeeNameListAsyn();

        /// <summary>
        /// 获取部门下成员表
        /// </summary>
        /// <param name="deptName"></param>
        /// <returns></returns>
        Task<List<GetEmployeeDetailByDeptOutput>> GetSignListByDeptNameAsync(string deptName);
    }
}

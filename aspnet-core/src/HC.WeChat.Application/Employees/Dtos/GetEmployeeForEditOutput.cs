using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HC.WeChat.Employees;

namespace HC.WeChat.Employees.Dtos
{
    public class GetEmployeeForEditOutput
{
////BCC/ BEGIN CUSTOM CODE SECTION
////ECC/ END CUSTOM CODE SECTION
        public EmployeeEditDto Employee { get; set; }

}
    /// <summary>
    /// 部门下人员详细输出表
    /// </summary>
    public class GetEmployeeDetailByDeptOutput { 
    
        public string Name { get; set; }

        public string Code { get; set; }

        public bool IsSign { get; set; }

    }

    /// <summary>
    /// 返回部门列表信息菜单
    /// </summary>
    public class EmployeeTotalDto
    {
        public int Total { get; set; }

        public int Signed { get; set; }

        public string Name { get; set; }

    }
}
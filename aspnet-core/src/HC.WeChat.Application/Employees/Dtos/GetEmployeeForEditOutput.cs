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
    public class GetEmployeeDetailByDeptOutput { 
    
        public string Name { get; set; }

        public string Code { get; set; }

        public bool IsSign { get; set; }

    }
}
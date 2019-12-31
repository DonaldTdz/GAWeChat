using System;
using Abp.Application.Services.Dto;
using HC.WeChat.Employees.Dtos.LTMAutoMapper;
using HC.WeChat.Employees;
using HC.WeChat.WechatEnums;
using Abp.AutoMapper;

namespace HC.WeChat.Employees.Dtos
{
    [AutoMapFrom(typeof(Employee))]
    public class EmployeeListDto : FullAuditedEntityDto<Guid>
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        public string Code { get; set; }
        public string Name { get; set; }
        public UserPositionEnum Position { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public bool IsAction { get; set; }
        public int? TenantId { get; set; }
        public string PositionName
        {
            get
            {
                return this.Position.ToString();
            }
        }
        public string VerificationCode { get; set; }

        /// <summary>
        /// 验证码显示
        /// </summary>
        public string EmployeeVerificationCode
        {
            get
            {
                var emCode = (1 + Code.Substring(Code.Length - 6)).ToString();
                var verCode = (int.Parse(emCode) * 15 + 15).ToString();
                return verCode.Substring(verCode.Length - 6);
            }
        }
       

    }
}
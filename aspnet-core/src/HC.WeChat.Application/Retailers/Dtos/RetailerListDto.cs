using System;
using Abp.Application.Services.Dto;
using HC.WeChat.Retailers.Dtos.LTMAutoMapper;
using HC.WeChat.Retailers;
using HC.WeChat.WechatEnums;
using Abp.AutoMapper;

namespace HC.WeChat.Retailers.Dtos
{
    [AutoMapFrom(typeof(Retailer))]
    public class RetailerListDto : FullAuditedEntityDto<Guid>
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        public string Code { get; set; }
        public string Name { get; set; }
        public string BusinessAddress { get; set; }
        public string ArchivalLevel { get; set; }
        public string OrderCycle { get; set; }
        public string StoreType { get; set; }
        public string Telephone { get; set; }
        public bool IsAction { get; set; }
        public string BranchCompany { get; set; }
        public string Department { get; set; }
        public Guid? EmployeeId { get; set; }
        public string Manager { get; set; }
        public OrderModeEnum? OrderMode { get; set; }
        public TerminalTypeEnum? TerminalType { get; set; }
        public string BusinessType { get; set; }
        public ScaleEnum? Scale { get; set; }
        public MarketTypeEnum? MarketType { get; set; }
        public string DeliveryLine { get; set; }
        public int? TenantId { get; set; }

        public string LicenseKey { get; set; }
        public string VerificationCode { get; set; }
        public string OrderModeName
        {
            get
            {
                return OrderMode.ToString();
            }
        }
        public string TerminalTypeName
        {
            get
            {
                return TerminalType.ToString();
            }
        }
        public string ScaleName
        {
            get
            {
                return Scale.ToString();
            }
        }
        public string MarketTypeName
        {
            get
            {
                return MarketType.ToString();
            }
        }
        /// <summary>
        /// 验证码
        /// </summary>
        public string RetailerVerificationCode
        {
            get
            {
                var emCode = (1 + Code.Substring(Code.Length - 6)).ToString();
                var verCode = (int.Parse(emCode) * 15 + 15).ToString();
                return verCode.Substring(verCode.Length - 6);
            }
        }

        /// <summary>
        /// 客户id
        /// </summary>
        public string CustId { get; set; }
        /// <summary>
        /// 线路编号
        /// </summary>
        public string DisLineCode { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 市场部id
        /// </summary>
        public string DepartmentId { get; set; }

        public string SlsmanId { get; set; }

        public string SlsmanName { get; set; }

        /// <summary>
        /// 片区
        /// </summary>
        public string Area { get; set; }

    }
}
using System.ComponentModel.DataAnnotations;
using HC.WeChat.Retailers.Dtos.LTMAutoMapper;
using HC.WeChat.Retailers;
using System;
using HC.WeChat.WechatEnums;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace HC.WeChat.Retailers.Dtos
{
    [AutoMapTo(typeof(Retailer))]
    public class RetailerEditDto: AuditedEntityDto<Guid?>
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        //public Guid? Id { get; set; }

        /// <summary>
        /// 客户编码
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Code { get; set; }


        /// <summary>
        /// 客户姓名
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }


        /// <summary>
        /// 经营地址
        /// </summary>
        [StringLength(500)]
        public string BusinessAddress { get; set; }


        /// <summary>
        /// 客户分档
        /// </summary>
        [StringLength(100)]
        public string ArchivalLevel { get; set; }


        /// <summary>
        /// 订货周期
        /// </summary>
        [StringLength(100)]
        public string OrderCycle { get; set; }


        /// <summary>
        /// 业态
        /// </summary>
        [StringLength(100)]
        public string StoreType { get; set; }


        /// <summary>
        /// 订货电话
        /// </summary>
        [StringLength(100)]
        public string Telephone { get; set; }


        /// <summary>
        /// 客户状态
        /// </summary>
        [Required]
        public bool IsAction { get; set; }


        /// <summary>
        /// 分公司 快照
        /// </summary>
        [StringLength(200)]
        public string BranchCompany { get; set; }


        /// <summary>
        /// 市场部﻿ 快照
        /// </summary>
        [StringLength(100)]
        public string Department { get; set; }
        public Guid? EmployeeId { get; set; }


        /// <summary>
        /// 客户经理 快照
        /// </summary>
        [StringLength(50)]
        public string Manager { get; set; }
        public OrderModeEnum? OrderMode { get; set; }
        public TerminalTypeEnum? TerminalType { get; set; }


        /// <summary>
        /// 商圈类型
        /// </summary>
        [StringLength(100)]
        public string BusinessType { get; set; }
        public ScaleEnum? Scale { get; set; }
        public MarketTypeEnum? MarketType { get; set; }


        /// <summary>
        /// 送货线路
        /// </summary>
        [StringLength(500)]
        public string DeliveryLine { get; set; }
        public int? TenantId { get; set; }

        [StringLength(50)]
        public string LicenseKey { get; set; }

        public string VerificationCode { get; set; }


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
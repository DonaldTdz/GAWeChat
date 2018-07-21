using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using HC.WeChat.WechatEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.WeChat.Retailers
{
    /// <summary>
    /// 零售客户
    /// </summary>
    [Table("Retailers")]
    public class Retailer : FullAuditedEntity<Guid>, IMayHaveTenant
    {

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

        /// <summary>
        /// 客户经理Id 外键
        /// </summary>
        public Guid? EmployeeId { get; set; }

        /// <summary>
        /// 客户经理 快照
        /// </summary>
        [StringLength(50)]
        public string Manager { get; set; }

        /// <summary>
        /// 订货方式(枚举 无、网上订货、电话订货、手机)
        /// </summary>
        public OrderModeEnum? OrderMode { get; set; }

        /// <summary>
        /// 终端类型(枚举 无、建议终端、普通终端、现代终端)
        /// </summary>
        public TerminalTypeEnum? TerminalType { get; set; }

        /// <summary>
        /// 商圈类型
        /// </summary>
        [StringLength(100)]
        public string BusinessType { get; set; }

        /// <summary>
        /// 经营规模(枚举 大、小、中)
        /// </summary>
        public ScaleEnum? Scale { get; set; }

        /// <summary>
        /// 市场类型(枚举 城镇、乡村)
        /// </summary>
        public MarketTypeEnum? MarketType { get; set; }

        /// <summary>
        /// 送货线路
        /// </summary>
        [StringLength(500)]
        public string DeliveryLine { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// 专卖证号
        /// </summary>
        [StringLength(50)]
        public string LicenseKey { get; set; }

        /// <summary>
        /// 绑定验证码
        /// </summary>
        [StringLength(50)]
        public string VerificationCode { get; set; }

        /// <summary>
        /// 客户id
        /// </summary>
        [StringLength(30)]
        [Required]
        public string CustId { get; set; }
        /// <summary>
        /// 线路编号
        /// </summary>
        [StringLength(50)]
        public string DisLineCode { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        [StringLength(6)]
        public string Category { get; set; }
        /// <summary>
        /// 市场部id
        /// </summary>
        [StringLength(50)]
        [Required]
        public string DepartmentId { get; set; }

        [StringLength(50)]
        public string SlsmanId { get; set; }

        [StringLength(100)]
        public string SlsmanName { get; set; }

        /// <summary>
        /// 片区
        /// </summary>
        public string Area { get; set; }
    }
}

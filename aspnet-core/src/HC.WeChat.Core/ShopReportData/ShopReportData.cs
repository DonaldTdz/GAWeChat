using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.WeChat.ShopReportDatas
{
    /// <summary>
    /// 店铺数据报表
    /// </summary>
    [Table("ShopReportDatas")]
    public class ShopReportData
    {
        /// <summary>
        /// RootId
        /// </summary>
        [Required]
        public virtual int RootId { get; set; }

        /// <summary>
        /// CompanyId
        /// </summary>
        [Required]
        public virtual int CompanyId { get; set; }

        /// <summary>
        /// AreaId
        /// </summary>
        [Required]
        public virtual int AreaId { get; set; }

        /// <summary>
        /// SlsmanNameId
        /// </summary>
        [Required]
        public virtual int SlsmanNameId { get; set; }

        /// <summary>
        /// GroupNum
        /// </summary>
        [Required]
        public virtual int GroupNum { get; set; }

        /// <summary>
        /// Organization
        /// </summary>
        [StringLength(50)]
        public virtual string Organization { get; set; }

        /// <summary>
        /// ShopTotal
        /// </summary>
        public virtual int? ShopTotal { get; set; }

        /// <summary>
        /// ScanQuantity
        /// </summary>
        public virtual int? ScanQuantity { get; set; }

        /// <summary>
        /// ScanFrequency
        /// </summary>
        public virtual int? ScanFrequency { get; set; }

        /// <summary>
        /// PriceTotal
        /// </summary>
        public virtual string PriceTotal { get; set; }

        /// <summary>
        /// CustIntegral
        /// </summary>
        public virtual int? CustIntegral { get; set; }

        /// <summary>
        /// RetailerIntegral
        /// </summary>
        public virtual int? RetailerIntegral { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public virtual DateTime? CreationTime { get; set; }
    }
}

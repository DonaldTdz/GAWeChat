using System;
using System.Collections.Generic;
using System.Text;

namespace HC.WeChat.Retailers
{
    public class ShopReportData
    {
        /// <summary>
        /// RootId
        /// </summary>
        public int RootId { get; set; }

        /// <summary>
        /// CompanyId
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// AreaId
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// SlsmanNameId
        /// </summary>
        public int SlsmanNameId { get; set; }

        /// <summary>
        /// GroupNum
        /// </summary>
        public int GroupNum { get; set; }

        /// <summary>
        /// Organization
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// ShopTotal
        /// </summary>
        public int? ShopTotal { get; set; }

        /// <summary>
        /// ScanQuantity
        /// </summary>
        public int? ScanQuantity { get; set; }

        /// <summary>
        /// ScanFrequency
        /// </summary>
        public int? ScanFrequency { get; set; }

        /// <summary>
        /// PriceTotal
        /// </summary>
        public decimal? PriceTotal { get; set; }

        /// <summary>
        /// CustIntegral
        /// </summary>
        public int? CustIntegral { get; set; }

        /// <summary>
        /// RetailerIntegral
        /// </summary>
        public int? RetailerIntegral { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime? CreationTime { get; set; }

        public string Specification { get; set; }
    }
}

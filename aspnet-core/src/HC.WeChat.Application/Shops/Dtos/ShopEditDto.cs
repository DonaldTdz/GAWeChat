﻿using System.ComponentModel.DataAnnotations;
using HC.WeChat.Shops.Dtos.LTMAutoMapper;
using HC.WeChat.Shops;
using System;
using HC.WeChat.WechatEnums;
using Abp.AutoMapper;

namespace HC.WeChat.Shops.Dtos
{
    //[AutoMapFrom(typeof(Shop))]
    public class ShopEditDto
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        public Guid? Id { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Name { get; set; }


        /// <summary>
        /// 店铺地址
        /// </summary>
        [StringLength(200)]
        public string Address { get; set; }


        /// <summary>
        /// 店铺描述
        /// </summary>
        [StringLength(500)]
        public string Desc { get; set; }
        public Guid? RetailerId { get; set; }
        //public string RetailerName { get; set; }


        /// <summary>
        /// 店铺形象图片
        /// </summary>
        [StringLength(500)]
        public string CoverPhoto { get; set; }
        public int? SaleTotal { get; set; }
        public int? ReadTotal { get; set; }

        /// <summary>
        /// 排重浏览量
        /// </summary>
        public int? SingleTotal { get; set; }
        /// <summary>
        /// 评价描述（好（0），中（10），差（0））
        /// </summary>
        [StringLength(100)]
        public string Evaluation { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public double? QqLongitude { get; set; }
        public double? QqLatitude { get; set; }
        public ShopAuditStatus? Status { get; set; }
        public DateTime? AuditTime { get; set; }


        /// <summary>
        /// 申请创建时间
        /// </summary>
        [Required]
        public DateTime CreationTime { get; set; }
        public int? TenantId { get; set; }

        /// <summary>
        /// 审核状态名字
        /// </summary>
        //public string StatusName
        //{
        //    get
        //    {
        //        return Status.ToString();
        //    }
        //}
        public string Tel { get; set; }

        /// <summary>
        /// 拒绝理由
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 微信票据（二维码）
        /// </summary>
        [StringLength(200)]
        public string Ticket { get; set; }

        /// <summary>
        /// 二维码url
        /// </summary>
        [StringLength(200)]
        public string WechatUrl { get; set; }


        /// <summary>
        /// 二维码图片url
        /// </summary>
        [StringLength(500)]
        public string QRUrl { get; set; }

        /// <summary>
        /// 粉丝数
        /// </summary>
        public  int? FansNum { get; set; }
    }
}
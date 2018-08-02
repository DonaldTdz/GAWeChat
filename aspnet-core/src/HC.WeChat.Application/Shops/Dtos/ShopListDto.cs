using System;
using Abp.Application.Services.Dto;
using HC.WeChat.Shops.Dtos.LTMAutoMapper;
using HC.WeChat.Shops;
using HC.WeChat.WechatEnums;
using Abp.AutoMapper;
using System.Collections.Generic;

namespace HC.WeChat.Shops.Dtos
{
    [AutoMapFrom(typeof(Shop))]
    public class ShopListDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Desc { get; set; }
        public Guid? RetailerId { get; set; }
        public string RetailerName { get; set; }
        public string CoverPhoto { get; set; }
        public int? SaleTotal { get; set; }
        public int? ReadTotal { get; set; }
        public string RetailerCode { get; set; }


        /// <summary>
        /// 排重浏览量
        /// </summary>
        public int? SingleTotal { get; set; }
        public string Evaluation { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public double? QqLongitude { get; set; }
        public double? QqLatitude { get; set; }
        public ShopAuditStatus? Status { get; set; }
        public DateTime? AuditTime { get; set; }
        public DateTime CreationTime { get; set; }
        public int? TenantId { get; set; }
        public string StatusName
        {
            get
            {
                return Status.ToString();
            }
        }

        public string Tel { get; set; }

        /// <summary>
        /// 拒绝理由
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 微信票据（二维码）
        /// </summary>
        public string Ticket { get; set; }

        /// <summary>
        /// 二维码url
        /// </summary>
        public string WechatUrl { get; set; }

        /// <summary>
        /// 二维码图片url
        /// </summary>
        public string QRUrl { get; set; }

        /// <summary>
        /// 粉丝数
        /// </summary>
        public int? FansNum { get; set; }

        /// <summary>
        /// 分公司 快照
        /// </summary>
        public string BranchCompany { get; set; }

        /// <summary>
        /// 客户经理 快照
        /// </summary>
        public string Manager { get; set; }

        /// <summary>
        /// 零售户片区
        /// </summary>
        public string Area { get; set; }
    }
    [AutoMapFrom(typeof(Shop))]
    public class CheckShopDto
    {
        public Guid Id { get; set; }
        public ShopAuditStatus? Status { get; set; }
        public string Reason { get; set; }
        //public DateTime? AuditTime { get; set; }
    }

    [AutoMapFrom(typeof(Shop))]
    public class NearbyShopDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string CoverPhoto { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public double? QqLongitude { get; set; }
        public double? QqLatitude { get; set; }
        public string Tel { get; set; }
        public double Distance { get; set; }
    }
    /// <summary>
    /// 首页信息
    /// </summary>
    public class HomeInfo
    {
        public int ShopCount { get; set; }

        public int PendingShopCount { get; set; }

        public int GoodsSearchCount { get; set; }

        public int IntegralTotal { get; set; }

        public int WechatUserCount { get; set; }

    }

    /// <summary>
    /// 生成店铺二维码返回结果
    /// </summary>
    public class CreateQRResult
    {
        /// <summary>
        /// 票据
        /// </summary>
        public string Ticket { get; set; }
        /// <summary>
        /// 二维码图片解析后的地址（根据它可自行生成二维码）
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 本地二维码图片Url
        /// </summary>
        public string QRUrl { get; set; }

    }

    /// <summary>
    /// 店铺入驻数统计
    /// </summary>
    public class ShopStatisticDto
    {
        /// <summary>
        /// 分公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 入驻店铺数
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// 排序id
        /// </summary>
        public int GroupId { get; set; }

    }

    public class ShopStatisticLiDto
    {
        /// <summary>
        /// 店铺入驻数统计
        /// </summary>
        public List<ShopStatisticDto> ShopStaDto { get; set; }

        /// <summary>
        /// 店铺入驻总数
        /// </summary>
        public int? Total { get; set; }

    }

    public class ShopUrlQrCodeDto
    {
        public Guid Id { get; set; }

        public string Company { get; set; }

        public string UserName { get; set; }

        public string UserCode { get; set; }
    }
}
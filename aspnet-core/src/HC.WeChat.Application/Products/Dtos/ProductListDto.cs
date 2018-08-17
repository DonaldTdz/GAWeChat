using System;
using Abp.Application.Services.Dto;
using HC.WeChat.Products.Dtos.LTMAutoMapper;
using HC.WeChat.Products;
using HC.WeChat.WechatEnums;
using Abp.AutoMapper;
using System.Collections.Generic;

namespace HC.WeChat.Products.Dtos
{
    [AutoMapFrom(typeof(Product))]
    public class ProductListDto : EntityDto<Guid>
    {
        public string Specification { get; set; }
        public ProductTypeEnum? Type { get; set; }
        public decimal? Price { get; set; }
        public bool? IsRare { get; set; }
        public string PackageCode { get; set; }
        public string BarCode { get; set; }
        public int? SearchCount { get; set; }
        public bool? IsAction { get; set; }
        public DateTime CreationTime { get; set; }
        public int? TenantId { get; set; }
        public long? CreatorUserId { get; set; }

        public string PhotoUrl { get; set; }
        public string TypeName
        {
            get
            {
                return Type.ToString();
            }
        }

        /// <summary>
        /// 产品营销系统Id
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 营销系统code
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 地区编码Id
        /// </summary>
        public string MfrId { get; set; }

        /// <summary>
        /// 商标所属公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 产品介绍
        /// </summary>
        public string Desc { get; set; }
    }

    public class RareProductDto
    {
        /// <summary>
        /// 卷烟类
        /// </summary>
        public List<ProductListDto> CigaretteProducts { get; set; }

        /// <summary>
        /// 特产类
        /// </summary>
        public List<ProductListDto> SpecialProducts { get; set; }
    }

    /// <summary>
    /// 购买商品Dto
    /// </summary>
    [AutoMapFrom(typeof(Product))]
    public class ShopProductDto : EntityDto<Guid>
    {
        public string Specification { get; set; }

        public decimal? Price { get; set; }

        public string PackageCode { get; set; }

        public string BarCode { get; set; }

        public int Num { get; set; }
    }

    [AutoMapFrom(typeof(Product))]
    public class RareProductSearchDto : EntityDto<Guid>
    {
        public string Specification { get; set; }

        public ProductTypeEnum? Type { get; set; }

        public string TypeName
        {
            get
            {
                return Type.ToString();
            }
        }
    }

    /// <summary>
    /// 零售户
    /// </summary>
    public class RetailInfoDto
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public string CustId { get; set; }

        /// <summary>
        /// 客户编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 零售户姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否是店主 针对零售客户
        /// </summary>
        public virtual bool? IsShopkeeper { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImgUrl { get; set; }

        /// <summary>
        /// 经营地址
        /// </summary>
        public string BusinessAddress { get; set; }

        /// <summary>
        /// 微信OPENID
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 专卖证号
        /// </summary>
        public string LicenseKey { get; set; }

        /// <summary>
        /// 本月订货金额
        /// </summary>
        public decimal MonthOrderMoney { get; set; }

        /// <summary>
        /// 本月订货总量
        /// </summary>
        public int MonthOrderQty { get; set; }

        /// <summary>
        /// 上月订货金额
        /// </summary>
        public decimal PreMonthOrderMoney { get; set; }

        /// <summary>
        /// 上月订货总量
        /// </summary>
        public int PreMonthOrderQty { get; set; }

        /// <summary>
        /// 川烟数量
        /// </summary>
        public int SiChuanQty { get; set; }

        /// <summary>
        /// 档级
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 已达档级
        /// </summary>
        public string CurrentLevel { get; set; }

        /// <summary>
        /// 上月档级
        /// </summary>
        public string PreLevel { get; set; }

        /// <summary>
        /// 累计积分
        /// </summary>
        public decimal TotalPoint { get; set; }

        /// <summary>
        /// 当月积分
        /// </summary>
        public decimal MonthPoint { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerificationCode { get; set; }
    }

    /// <summary>
    /// (暂时未用)
    /// </summary>
    public class RetailAccount
    {
        /// <summary>
        /// 专卖证号
        /// </summary>
        public string LicenseCode { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string BookDate { get; set; }

        /// <summary>
        /// 上月日期
        /// </summary>
        public string PreDate { get; set; }

        /// <summary>
        /// 卷烟编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 卷烟名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 上月订购数
        /// </summary>
        public decimal PreMonthQty { get; set; }

        /// <summary>
        ///  本月订购数
        /// </summary>
        public decimal ThsMonthQty { get; set; }

        /// <summary>
        /// 季度起始时间
        /// </summary>
        public string QuarterlyDate { get; set; }

        /// <summary>
        /// 季度订购数
        /// </summary>
        public decimal QuarterlyQty { get; set; }

        /// <summary>
        /// 年度起始时间
        /// </summary>
        public string YearDate { get; set; }

        /// <summary>
        /// 年度订购数
        /// </summary>
        public decimal YearQty { get; set; }
    }


    /// <summary>
    /// 零售户所有信息（暂时未用）
    /// </summary>
    public class RetailAllInfo
    {
        /// <summary>
        /// 基础信息
        /// </summary>
        public RetailInfoDto BasicInfo { get; set; }

        /// <summary>
        /// 台账信息
        /// </summary>
        public IList<RetailAccount> AccountBooks { get; set; }
    }

    public class RetailAccountDe
    {
        /// <summary>
        /// 专卖证号
        /// </summary>
        public string LicenseCode { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string BookDate { get; set; }

        /// <summary>
        /// 上月日期
        /// </summary>
        public string PreDate { get; set; }

        /// <summary>
        /// 卷烟编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 卷烟名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 订购数
        /// </summary>
        public decimal MonthQty { get; set; }

        /// <summary>
        /// 季度起始时间
        /// </summary>
        public string QuarterlyDate { get; set; }

        /// <summary>
        /// 年度起始时间
        /// </summary>
        public string YearDate { get; set; }
    }

    public class RetailAllInfoDe
    {
        /// <summary>
        /// 本月台账信息
        /// </summary>
        public IList<RetailAccountDe> MonthAccountBooks { get; set; }

        /// <summary>
        /// 上月台账信息
        /// </summary>
        public IList<RetailAccountDe> PreMonthAccountBooks { get; set; }

        /// <summary>
        /// 台账信息(季度、年度)
        /// </summary>
        public IList<RetailAccountDe> AccountBooks { get; set; }
    }


}
using System;
using Abp.Application.Services.Dto;
using HC.WeChat.ShopProducts.Dtos.LTMAutoMapper;
using HC.WeChat.ShopProducts;
using HC.WeChat.WechatEnums;
using Abp.AutoMapper;

namespace HC.WeChat.ShopProducts.Dtos
{
    [AutoMapFrom(typeof(ShopProduct))]
    public class ShopProductListDto : EntityDto<Guid>
    {
        public Guid ProductId { get; set; }
        public Guid ShopId { get; set; }
        public string Specification { get; set; }
        /// <summary>
        /// 产品类型（枚举：卷烟类、特产类）
        /// </summary>
        public ProductTypeEnum? Type { get; set; }
        /// <summary>
        /// 指导零售价
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// 包码
        /// </summary>
        public string PackageCode { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }
        /// <summary>
        /// 产品图片
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// 类型名
        /// </summary>
        public string TypeName
        {
            get
            {
                return Type.ToString();
            }
        }

        /// <summary>
        /// 产品描述
        /// </summary>
        public string Desc { get; set; }
    }
}
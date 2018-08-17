using System;
using Abp.Application.Services.Dto;
using HC.WeChat.PurchaseRecords.Dtos.LTMAutoMapper;
using HC.WeChat.PurchaseRecords;
using HC.WeChat.Products.Dtos;
using System.Collections.Generic;
using Abp.AutoMapper;

namespace HC.WeChat.PurchaseRecords.Dtos
{
    public class PurchaseRecordListDto : EntityDto<Guid>
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        public Guid? ProductId { get; set; }
        public string Specification { get; set; }
        public int? Quantity { get; set; }
        public Guid? ShopId { get; set; }
        public string ShopName { get; set; }
        public string OpenId { get; set; }
        public int? TenantId { get; set; }
        public int? Integral { get; set; }
        public string Remark { get; set; }
        public DateTime CreationTime { get; set; }
        public string OperatorOpenId { get; set; }
        public string OperatorName { get; set; }
        public string PhotoUrl { get; set; }
        public bool? IsEvaluation { get; set; }
        public string WeChatName { get; set; }
        public string Phone { get; set; }
        public string FavouriteSpecification { get; set; }
        public decimal? Price { get; set; }

    }

    [AutoMapTo(typeof(PurchaseRecord))]
    public class ExchangeIntegralDto
    {
        public List<ShopProductDto> ShopProductList { get; set; }
        public Guid? ShopId { get; set; }
        public string ShopName { get; set; }
        public string OpenId { get; set; }
        public int? TenantId { get; set; }
        public string OperatorOpenId { get; set; }
        public string OperatorName { get; set; }
        public Guid? RetailerId { get; set; }
        public string host { get; set; }
    }
}
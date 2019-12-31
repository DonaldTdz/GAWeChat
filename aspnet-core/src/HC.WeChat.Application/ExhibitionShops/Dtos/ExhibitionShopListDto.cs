using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.ExhibitionShops;
using System.Collections.Generic;

namespace HC.WeChat.ExhibitionShops.Dtos
{
    public class ExhibitionShopListDto : EntityDto<Guid>
    {

        /// <summary>
        /// CreateTime
        /// </summary>
        [Required(ErrorMessage = "CreateTime不能为空")]
        public DateTime CreateTime { get; set; }
        public int NoId { get; set; }

        /// <summary>
        /// RetailerId
        /// </summary>
        public Guid? RetailerId { get; set; }

        /// <summary>
        /// ShopName
        /// </summary>
        [Required(ErrorMessage = "ShopName不能为空")]
        public string ShopName { get; set; }


        /// <summary>
        /// ShopAddress
        /// </summary>
        public string ShopAddress { get; set; }


        /// <summary>
        /// PicPath
        /// </summary>
        [Required(ErrorMessage = "PicPath不能为空")]
        public string PicPath { get; set; }


        /// <summary>
        /// Votes
        /// </summary>
        public int? Votes { get; set; }


        /// <summary>
        /// Status
        /// </summary>
        [Required(ErrorMessage = "Status不能为空")]
        public int Status { get; set; }


        /// <summary>
        /// ShopId
        /// </summary>
        [Required(ErrorMessage = "ShopId不能为空")]
        public Guid ShopId { get; set; }


        /// <summary>
        /// AuditTime
        /// </summary>
        public DateTime? AuditTime { get; set; }

        public string Phone { get; set; }
        public int? FansNum { get; set; }
        public string Area { get; set; }
        public string CustCode { get; set; }
        public string CustName { get; set; }

        public int? Total { get; set; }

        ////BCC/ BEGIN CUSTOM CODE SECTION

        ////ECC/ END CUSTOM CODE SECTION
    }

    public class ExhibitionWechatDto : EntityDto<Guid?>
    {
        public DateTime CreateTime { get; set; }

        //public Guid? Id { get; set; }

        public string ShopName { get; set; }

        public int Votes { get; set; }

        public string PicPath { get; set; }

        public Guid? ShopId { get; set; }

        public int NoId { get; set; }
    }

    public class ExhibitionViewDto
    {
        public ExhibitionViewDto()
        {
            Items = new List<ExhibitionWechatDto>();
        }
        public List<ExhibitionWechatDto> Items { get; set; }
    }
}
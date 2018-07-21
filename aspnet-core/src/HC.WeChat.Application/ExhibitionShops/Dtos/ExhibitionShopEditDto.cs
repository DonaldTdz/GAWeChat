using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace HC.WeChat.ExhibitionShops.Dtos
{
    public class ExhibitionShopEditDto : EntityDto<Guid?>
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





        ////BCC/ BEGIN CUSTOM CODE SECTION

        ////ECC/ END CUSTOM CODE SECTION
    }
}
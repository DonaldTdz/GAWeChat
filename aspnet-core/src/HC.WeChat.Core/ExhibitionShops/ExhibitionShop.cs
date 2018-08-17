using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using HC.WeChat.WechatEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.WeChat.ExhibitionShops
{
    /// <summary>
    /// 陈列店铺
    /// </summary>
    [Table("ExhibitionShops")]
    public class ExhibitionShop : Entity<Guid>
    {

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 零售客户Id
        /// </summary>
        public virtual Guid? RetailerId { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        [Required]
        [StringLength(200)]
        public virtual string ShopName { get; set; }

        /// <summary>
        /// 店铺地址
        /// </summary>
        [StringLength(500)]
        public virtual string ShopAddress { get; set; }

        /// <summary>
        /// 陈列图片
        /// </summary>
        [Required]
        public virtual string PicPath { get; set; }

        /// <summary>
        /// 投票数
        /// </summary>
        public virtual int? Votes { get; set; }

        /// <summary>
        /// 是否通过审核
        /// </summary>
        [Required]
        public virtual int Status { get; set; }

        /// <summary>
        /// 店铺Id
        /// </summary>
        [Required]
        public virtual Guid ShopId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime? AuditTime { get; set; }

        /// <summary>
        /// 参赛编号
        /// </summary>
        public virtual int NoId { get; set; }
    }
}

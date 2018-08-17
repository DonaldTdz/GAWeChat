using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using HC.WeChat.WechatEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.WeChat.Favorites
{
    /// <summary>
    /// 店铺收藏
    /// </summary>
    [Table("Favorites")]
    public class Favorite : Entity<Guid>, IHasCreationTime
    {

        /// <summary>
        /// 店铺Id
        /// </summary>
        [Required]
        public virtual Guid ShopId { get; set; }

        /// <summary>
        /// 店铺快照
        /// </summary>
        [StringLength(200)]
        public virtual string ShopName { get; set; }

        /// <summary>
        /// OpenId
        /// </summary>
        [Required]
        public virtual string OpenId { get; set; }

        /// <summary>
        /// 微信快照
        /// </summary>
        [StringLength(200)]
        public virtual string NickName { get; set; }

        /// <summary>
        /// 收藏时间
        /// </summary>
        [Required]
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// 取消时间
        /// </summary>
        public virtual DateTime? CancelTime { get; set; }

        /// <summary>
        /// 是否取消收藏
        /// </summary>
        [Required]
        public virtual bool IsCancel { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual Guid? ProductId { get; set; }


        /// <summary>
        /// 店铺图片
        /// </summary>
        public virtual string CoverPhoto { get; set; }
    }
}

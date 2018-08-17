using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using HC.WeChat.Favorites;

namespace HC.WeChat.Favorites.Dtos
{
    public class FavoriteEditDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }


        /// <summary>
        /// ShopId
        /// </summary>
        [Required(ErrorMessage = "ShopId不能为空")]
        public Guid ShopId { get; set; }


        /// <summary>
        /// ShopName
        /// </summary>
        public string ShopName { get; set; }


        /// <summary>
        /// OpenId
        /// </summary>
        [Required(ErrorMessage = "OpenId不能为空")]
        public string OpenId { get; set; }


        /// <summary>
        /// NickName
        /// </summary>
        public string NickName { get; set; }


        /// <summary>
        /// CreationTime
        /// </summary>
        [Required(ErrorMessage = "CreationTime不能为空")]
        public DateTime CreationTime { get; set; }


        /// <summary>
        /// CancelTime
        /// </summary>
        public DateTime? CancelTime { get; set; }


        /// <summary>
        /// IsCancel
        /// </summary>
        [Required(ErrorMessage = "IsCancel不能为空")]
        public bool IsCancel { get; set; }


        /// <summary>
        /// ProductId
        /// </summary>
        public Guid? ProductId { get; set; }

        public string CoverPhoto { get; set; }




        //// custom codes

        //// custom codes end
    }
}
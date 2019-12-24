

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.LotteryDetails;

namespace HC.WeChat.LotteryDetails.Dtos
{
    public class LotteryDetailListDto : EntityDto<Guid>,IHasCreationTime 
    {

        
		/// <summary>
		/// LuckyDrawId
		/// </summary>
		[Required(ErrorMessage="LuckyDrawId不能为空")]
		public Guid LuckyDrawId { get; set; }



        /// <summary>
        /// UserId
        /// </summary>
        [Required(ErrorMessage="OpenId不能为空")]
		public Guid UserId { get; set; }



		/// <summary>
		/// IsWin
		/// </summary>
		[Required(ErrorMessage="IsWin不能为空")]
		public bool IsWin { get; set; }


        /// <summary>
        /// 是否允许中奖
        /// </summary>
        [Required]
        public bool IsCanWin { get; set; }

        /// <summary>
        /// PrizeId
        /// </summary>
        public Guid? PrizeId { get; set; }



		/// <summary>
		/// PrizeName
		/// </summary>
		public string PrizeName { get; set; }



		/// <summary>
		/// IsLottery
		/// </summary>
		public bool IsLottery { get; set; }



		/// <summary>
		/// LotteryTime
		/// </summary>
		public DateTime? LotteryTime { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
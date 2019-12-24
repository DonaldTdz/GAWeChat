
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using HC.WeChat.LotteryDetails;

namespace  HC.WeChat.LotteryDetails.Dtos
{
    public class LotteryDetailEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }         


        
		/// <summary>
		/// LuckyDrawId
		/// </summary>
		[Required(ErrorMessage="LuckyDrawId不能为空")]
		public Guid LuckyDrawId { get; set; }



		/// <summary>
		/// OpenId
		/// </summary>
		[Required(ErrorMessage="OpenId不能为空")]
		public string OpenId { get; set; }



		/// <summary>
		/// IsWin
		/// </summary>
		[Required(ErrorMessage="IsWin不能为空")]
		public bool IsWin { get; set; }



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




    }
}
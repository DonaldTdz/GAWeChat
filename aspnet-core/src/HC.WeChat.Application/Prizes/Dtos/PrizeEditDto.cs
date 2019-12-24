
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using HC.WeChat.Prizes;
using HC.WeChat.WechatEnums;

namespace  HC.WeChat.Prizes.Dtos
{
    public class PrizeEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }         


        
		/// <summary>
		/// Name
		/// </summary>
		[Required(ErrorMessage="Name不能为空")]
		public string Name { get; set; }



		/// <summary>
		/// LuckyDrawId
		/// </summary>
		[Required(ErrorMessage="LuckyDrawId不能为空")]
		public Guid LuckyDrawId { get; set; }



		/// <summary>
		/// Type
		/// </summary>
		[Required(ErrorMessage="Type不能为空")]
		public PrizeType Type { get; set; }



		/// <summary>
		/// Num
		/// </summary>
		[Required(ErrorMessage="Num不能为空")]
		public int Num { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using HC.WeChat.LuckySigns;

namespace  HC.WeChat.LuckySigns.Dtos
{
    public class LuckySignEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }         


        
		/// <summary>
		/// OpenId
		/// </summary>
		[Required(ErrorMessage= "UserId不能为空")]
		public Guid UserId { get; set; }


        public DateTime CreationTime { get; set; }
    }
}
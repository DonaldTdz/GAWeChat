

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.LuckySigns;

namespace HC.WeChat.LuckySigns.Dtos
{
    public class LuckySignListDto : EntityDto<Guid>,IHasCreationTime 
    {

        
		/// <summary>
		/// OpenId
		/// </summary>
		[Required(ErrorMessage= "UserId不能为空")]
		public Guid UserId { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
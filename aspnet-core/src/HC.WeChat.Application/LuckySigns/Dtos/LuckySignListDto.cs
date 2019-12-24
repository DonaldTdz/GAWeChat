

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.LuckySigns;

namespace HC.WeChat.LuckySigns.Dtos
{
    public class LuckySignListDto : AuditedEntityDto<Guid>,IHasCreationTime 
    {

        
		/// <summary>
		/// OpenId
		/// </summary>
		[Required(ErrorMessage="OpenId不能为空")]
		public string OpenId { get; set; }




    }
}
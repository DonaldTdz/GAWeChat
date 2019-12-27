

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

    /// <summary>
    /// 抽奖时查询签到状态的DTo
    /// </summary>

    public class GetLuckySignInfoDto
    {

        public string Name { get; set; }

        public string Code { get; set; }

        public bool LotteryState { get; set; }

        public string DeptName { get; set; }

    }
}
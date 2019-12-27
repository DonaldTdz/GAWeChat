

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.LuckyDraws;
using System.Collections.Generic;

namespace HC.WeChat.LuckyDraws.Dtos
{
    public class LuckyDrawListDto : EntityDto<Guid>,IHasCreationTime 
    {

        
		/// <summary>
		/// Name
		/// </summary>
		[Required(ErrorMessage="Name不能为空")]
		public string Name { get; set; }



		/// <summary>
		/// BeginTime
		/// </summary>
		[Required(ErrorMessage="BeginTime不能为空")]
		public DateTime BeginTime { get; set; }



		/// <summary>
		/// EndTime
		/// </summary>
		[Required(ErrorMessage="EndTime不能为空")]
		public DateTime EndTime { get; set; }



		/// <summary>
		/// IsPubish
		/// </summary>
		[Required(ErrorMessage="IsPubish不能为空")]
		public bool IsPublish { get; set; }



		/// <summary>
		/// PubishTime
		/// </summary>
		public DateTime? PublishTime { get; set; }

        public DateTime CreationTime { get; set; }
    }


	public class WXLuckyDrawOutput
	{

		public DateTime CreationTime { get; set; }

		public string Name { get; set; }

		public Guid? Id { get; set; }

		public Boolean IsPublish { get; set; }

		public DateTime BeginTime { get; set; }

		public DateTime EndTime { get; set; }

		public bool LotteryState { get; set; }
	}

	/// <summary>
	/// 单个抽奖活动详情返回
	/// </summary>
	public class WXLuckyDrawDetailIDOutput
	{
		public WXLuckyDrawDetailIDOutput() 
		{
			List = new List<WeiXinPriceInput>();
			LotteryDetails = new List<LotteryDetailDto>();
		}

		public string Name { get; set; }

		public DateTime? BeginTime { get; set; }

		public DateTime? EndTime { get; set; }

		public bool IsPublish { get; set; }

		public List<WeiXinPriceInput> List { get; set; }

		/// <summary>
		/// 开奖状态
		/// </summary>
		public bool LotteryState { get; set; }


		public List<LotteryDetailDto> LotteryDetails { get; set; }

	}

	public class LotteryDetailDto
	{ 
	
		public string Name { get; set; }

		public string PrizeName { get; set; }

		public int Num { get; set; }
	}
}
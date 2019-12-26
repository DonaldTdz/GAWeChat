
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using HC.WeChat.LuckyDraws;
using HC.WeChat.WechatEnums;

namespace  HC.WeChat.LuckyDraws.Dtos
{
    public class LuckyDrawEditDto : EntityDto<Guid?>, IHasCreationTime
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

	public class WeiXinCreateInput
	{

		public string Name { get; set; }

		public DateTime? BeginTime { get; set; }

		public DateTime? EndTime { get; set; }

		public Boolean IsPublish { get; set; }

		public List<WeiXinPriceInput> List { get; set; }

	}
	public class WeiXinPriceInput
	{

		public string Name { get; set; }

		public PrizeType Type { get; set; }

		public int Num { get; set; }

	}
}
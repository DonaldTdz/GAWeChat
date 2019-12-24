

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.Extensions;
using Abp.UI;
using Abp.Domain.Repositories;
using Abp.Domain.Services;

using HC.WeChat;
using HC.WeChat.LotteryDetails;


namespace HC.WeChat.LotteryDetails.DomainService
{
    /// <summary>
    /// LotteryDetail领域层的业务管理
    ///</summary>
    public class LotteryDetailManager :WeChatDomainServiceBase, ILotteryDetailManager
    {
		
		private readonly IRepository<LotteryDetail,Guid> _repository;

		/// <summary>
		/// LotteryDetail的构造方法
		///</summary>
		public LotteryDetailManager(
			IRepository<LotteryDetail, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitLotteryDetail()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}



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
using HC.WeChat.Prizes;


namespace HC.WeChat.Prizes.DomainService
{
    /// <summary>
    /// Prize领域层的业务管理
    ///</summary>
    public class PrizeManager :WeChatDomainServiceBase, IPrizeManager
    {
		
		private readonly IRepository<Prize,Guid> _repository;

		/// <summary>
		/// Prize的构造方法
		///</summary>
		public PrizeManager(
			IRepository<Prize, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitPrize()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}

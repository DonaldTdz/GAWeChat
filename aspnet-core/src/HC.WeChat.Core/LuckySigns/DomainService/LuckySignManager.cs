

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
using HC.WeChat.LuckySigns;


namespace HC.WeChat.LuckySigns.DomainService
{
    /// <summary>
    /// LuckySign领域层的业务管理
    ///</summary>
    public class LuckySignManager :WeChatDomainServiceBase, ILuckySignManager
    {
		
		private readonly IRepository<LuckySign,Guid> _repository;

		/// <summary>
		/// LuckySign的构造方法
		///</summary>
		public LuckySignManager(
			IRepository<LuckySign, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitLuckySign()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}

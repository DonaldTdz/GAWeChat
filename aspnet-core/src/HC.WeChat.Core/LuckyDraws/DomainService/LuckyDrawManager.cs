

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
using HC.WeChat.LuckyDraws;


namespace HC.WeChat.LuckyDraws.DomainService
{
    /// <summary>
    /// LuckyDraw领域层的业务管理
    ///</summary>
    public class LuckyDrawManager :WeChatDomainServiceBase, ILuckyDrawManager
    {
		
		private readonly IRepository<LuckyDraw,Guid> _repository;

		/// <summary>
		/// LuckyDraw的构造方法
		///</summary>
		public LuckyDrawManager(
			IRepository<LuckyDraw, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitLuckyDraw()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}

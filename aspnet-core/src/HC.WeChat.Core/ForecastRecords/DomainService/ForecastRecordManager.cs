

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
using HC.WeChat.ForecastRecords;


namespace HC.WeChat.ForecastRecords.DomainService
{
    /// <summary>
    /// ForecastRecord领域层的业务管理
    ///</summary>
    public class ForecastRecordManager :WeChatDomainServiceBase, IForecastRecordManager
    {
		
		private readonly IRepository<ForecastRecord,Guid> _repository;

		/// <summary>
		/// ForecastRecord的构造方法
		///</summary>
		public ForecastRecordManager(
			IRepository<ForecastRecord, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitForecastRecord()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}

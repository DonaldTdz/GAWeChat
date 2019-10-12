

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
using HC.WeChat.AnswerRecords;


namespace HC.WeChat.AnswerRecords.DomainService
{
    /// <summary>
    /// AnswerRecord领域层的业务管理
    ///</summary>
    public class AnswerRecordManager :WeChatDomainServiceBase, IAnswerRecordManager
    {
		
		private readonly IRepository<AnswerRecord,Guid> _repository;

		/// <summary>
		/// AnswerRecord的构造方法
		///</summary>
		public AnswerRecordManager(
			IRepository<AnswerRecord, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitAnswerRecord()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}

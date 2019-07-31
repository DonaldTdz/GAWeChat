

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
using HC.WeChat.QuestionOptions;


namespace HC.WeChat.QuestionOptions.DomainService
{
    /// <summary>
    /// QuestionOption领域层的业务管理
    ///</summary>
    public class QuestionOptionManager :WeChatDomainServiceBase, IQuestionOptionManager
    {
		
		private readonly IRepository<QuestionOption,Guid> _repository;

		/// <summary>
		/// QuestionOption的构造方法
		///</summary>
		public QuestionOptionManager(
			IRepository<QuestionOption, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitQuestionOption()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}

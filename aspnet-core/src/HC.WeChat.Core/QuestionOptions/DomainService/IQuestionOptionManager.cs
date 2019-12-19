

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.WeChat.QuestionOptions;


namespace HC.WeChat.QuestionOptions.DomainService
{
    public interface IQuestionOptionManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitQuestionOption();



		 
      
         

    }
}

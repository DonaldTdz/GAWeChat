

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.WeChat.Questionnaires;


namespace HC.WeChat.Questionnaires.DomainService
{
    public interface IQuestionnaireManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitQuestionnaire();



		 
      
         

    }
}

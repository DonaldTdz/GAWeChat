

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.WeChat.QuestionRecords;


namespace HC.WeChat.QuestionRecords.DomainService
{
    public interface IQuestionRecordManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitQuestionRecord();



		 
      
         

    }
}

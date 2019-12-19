

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.WeChat.AnswerRecords;


namespace HC.WeChat.AnswerRecords.DomainService
{
    public interface IAnswerRecordManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitAnswerRecord();



		 
      
         

    }
}

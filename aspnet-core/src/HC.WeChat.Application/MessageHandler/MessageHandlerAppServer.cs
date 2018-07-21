using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.WeChat.Senparc.MessageHandlers;
using HC.WeChat.WechatMessages;
using HC.WeChat.WechatSubscribes;
using HC.WeChat.WeChatUsers;
using HC.WeChat.WeChatUsers.DomainServices;
using Senparc.Weixin.MP.Entities.Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HC.WeChat.MessageHandler
{
    public class MessageHandlerAppServer : WeChatAppServiceBase, IMessageHandlerAppServer
    {
        private readonly IRepository<WechatMessage, Guid> _wechatmessageRepository;
        private readonly IRepository<WechatSubscribe, Guid> _wechatsubscribeRepository;
        //private readonly IWeChatUserManager _wechatUserManager;
        private readonly IWeChatUserAppService _wChatUserAppService;
        public MessageHandlerAppServer(IRepository<WechatMessage, Guid> wechatmessageRepository, 
            IRepository<WechatSubscribe, Guid> wechatsubscribeRepository,
             IWeChatUserAppService wChatUserAppService)
            //IWeChatUserManager wechatUserManager)
        {
            _wechatmessageRepository = wechatmessageRepository;
            _wechatsubscribeRepository = wechatsubscribeRepository;
            //_wechatUserManager = wechatUserManager;
            _wChatUserAppService = wChatUserAppService;
        }

        public async Task<string> MessageHandler(PostModel postModel, Stream msgStream, int? tenantId)
        {
            //设置每个人上下文消息储存的最大数量
            var maxRecordCount = 10;
            string body = new StreamReader(msgStream).ReadToEnd();
            byte[] requestData = Encoding.UTF8.GetBytes(body);
            Stream inputStream = new MemoryStream(requestData);

            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var messageHandler = new HCMessageHandler(_wechatmessageRepository, _wechatsubscribeRepository, _wChatUserAppService, tenantId, inputStream, postModel, maxRecordCount);
                messageHandler.Logger = Logger;
                /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
                 * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
                messageHandler.OmitRepeatedMessage = true;

                //执行微信处理过程
                messageHandler.Execute();

                await MessageHandlerLogAsync(messageHandler);

                if (messageHandler.ResponseDocument == null)
                {
                    return string.Empty;
                }
                return messageHandler.ResponseDocument.ToString();
            }
        }

        private async Task MessageHandlerLogAsync(AbpMessageHandler messageHandler)
        {
             await Task.Run(() => {
                 //记录 Request 日志
                 Logger.InfoFormat("RequestDocument:{0}", messageHandler.RequestDocument);

                 if (messageHandler.UsingEcryptMessage)
                 {
                     Logger.InfoFormat("EcryptRequestDocument:{0}", messageHandler.EcryptRequestDocument);
                 }
                 Logger.Info("Request 日志 记录完成");

                 //记录 Response 日志
                 Logger.InfoFormat("ResponseDocument:{0}", messageHandler.ResponseDocument);

                 if (messageHandler.UsingEcryptMessage && messageHandler.FinalResponseDocument != null)
                 {
                     Logger.InfoFormat("FinalResponseDocument:{0}", messageHandler.FinalResponseDocument);
                 }
             });
        }
    }
}

using Abp.Domain.Repositories;
using Abp.WeChat.Senparc.MessageHandlers;
using Castle.Core.Logging;
using HC.WeChat.WechatAppConfigs;
using HC.WeChat.WechatMessages;
using HC.WeChat.WechatSubscribes;
using HC.WeChat.WeChatUsers;
using HC.WeChat.WeChatUsers.DomainServices;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Senparc.Weixin.Entities.Request;
using Abp.Threading;
using Senparc.Weixin.MP;

namespace HC.WeChat.MessageHandler
{
    public class HCMessageHandler : AbpMessageHandler
    {
        private readonly IRepository<WechatMessage, Guid> _wechatmessageRepository;
        private readonly IRepository<WechatSubscribe, Guid> _wechatsubscribeRepository;
        //private readonly IWeChatUserManager _wechatUserManager;
        private readonly IWeChatUserAppService _wChatUserAppService;
        private readonly IRepository<WechatAppConfig, int> _wechatappconfigRepository;
        public ILogger Logger { protected get; set; }

        //private int? _tenantId = 0;
        private int? _tenantId;


        public HCMessageHandler(IRepository<WechatMessage, Guid> wechatmessageRepository, 
            IRepository<WechatSubscribe, Guid> wechatsubscribeRepository,
            //IWeChatUserManager wechatUserManager,
            IRepository<WechatAppConfig, int> wechatappconfigRepository,
            IWeChatUserAppService wChatUserAppService,
            int? tenantId, Stream inputStream, 
            PostModel postModel, 
            int maxRecordCount = 0) : base(inputStream, postModel, maxRecordCount)
        {
            _wechatmessageRepository = wechatmessageRepository;
            _wechatsubscribeRepository = wechatsubscribeRepository;
            //_wechatUserManager = wechatUserManager;
            _wChatUserAppService = wChatUserAppService;
            Logger = NullLogger.Instance;
            _tenantId = tenantId;
            _wechatappconfigRepository = wechatappconfigRepository;
        }

        private WechatSubscribe GetWechatSubscribe()
        {
            return _wechatsubscribeRepository.GetAll().Where(w => w.TenantId == _tenantId).FirstOrDefault();
        }

        private List<WechatMessage> GetWechatMessageList()
        {
            //return _wechatmessageRepository.GetAll().Where(w => w.TenantId == _tenantId).ToList();
            //先处理文字消息
            return _wechatmessageRepository.GetAll().Where(w => w.TenantId == _tenantId && w.MsgType == WechatEnums.MsgTypeEnum.文字消息).ToList();
        }
        private List<WechatMessage> GetWechatMessagePicList()
        {
            //处理图文消息
            return _wechatmessageRepository.GetAll().Where(w => w.TenantId == _tenantId && w.MsgType == WechatEnums.MsgTypeEnum.图文消息).ToList();
        }

        /// <summary>
        /// 获取MediaId
        /// </summary>
        /// <returns></returns>
        //private string GetWechatSubscribeMediaId()
        //{
        //    return _wechatsubscribeRepository.GetAll().Where(w => w.TenantId == _tenantId && w.MsgType==WechatEnums.MsgTypeEnum.纯图片).Select(w=>w.MediaId).FirstOrDefault();
        //}

        public override void ConfigurationMessageInfo(RequestMessageText requestMessage)
        {
            MessageInfo = new AbpMessageInfo();
            MessageInfo.KeyWords = new Dictionary<string, string>(); // 文本
            MessageInfo.KeyWordsPic = new Dictionary<string, Article>(); // 图文
            var keyWordList = GetWechatMessageList();
            var keyPicList = GetWechatMessagePicList();
            if (keyWordList.Count > 0)
            {
                MessageInfo.KeyWords = keyWordList.ToDictionary(key => key.KeyWord, value => value.Content);
                MessageInfo.KeyWordsPic = keyPicList.ToDictionary(key => key.KeyWord, value =>new Article(){ Title =value.Title,Description =value.Desc,PicUrl=value.PicLink,Url=value.Content});
            }

            var sinfo = GetWechatSubscribe();
            if (sinfo != null)
            {
                switch (sinfo.MsgType)
                {
                    case WechatEnums.MsgTypeEnum.文字消息:
                        {
                            MessageInfo.SubscribeMsg = sinfo.Content;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public override void Unsubscribe(RequestMessageEvent_Unsubscribe requestMessage)
        {
            Logger.InfoFormat("取消关注:{0}", requestMessage);
            //取消关注
            AsyncHelper.RunSync(() => _wChatUserAppService.UnsubscribeAsync(requestMessage.FromUserName));
        }

        public override void Subscribe(RequestMessageEvent_Subscribe requestMessage)
        {
            Logger.InfoFormat("关注:{0}", requestMessage);
            Logger.InfoFormat("关注场景值:{0}", requestMessage.EventKey);
            //获取微信用户信息
            var wechatUser = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(appId, requestMessage.FromUserName);

            Logger.InfoFormat("关注用户:{0}", wechatUser);
            Logger.InfoFormat("关注ticket：{0}", requestMessage.Ticket);

            //关注公众号
            //_wechatUserManager.SubscribeAsync(requestMessage.FromUserName, wechatUser.nickname, wechatUser.headimgurl, _tenantId, requestMessage.EventKey, requestMessage.Ticket);
            AsyncHelper.RunSync(() => _wChatUserAppService.SubscribeAsync(requestMessage.FromUserName, wechatUser.nickname, wechatUser.headimgurl, requestMessage.EventKey, requestMessage.Ticket) );
            //_wechatUserManager.SubscribeAsync(requestMessage.FromUserName, wechatUser.nickname, wechatUser.headimgurl, _tenantId);
        }

        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            try
            {
                Subscribe(requestMessage);
                if (MessageInfo == null)
                {
                    return new SuccessResponseMessage();
                }
                var sinfo = GetWechatSubscribe();
                if (sinfo != null)
                {
                    if (sinfo.MsgType == WechatEnums.MsgTypeEnum.文字消息)
                    {
                        var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
                        responseMessage.Content = MessageInfo.SubscribeMsg;
                        //Subscribe(requestMessage);
                        return responseMessage;
                    }
                    //else if (sinfo.MsgType == WechatEnums.MsgTypeEnum.纯图片)
                    //{
                    //    string mediaid = GetWechatSubscribeMediaId();
                    //    if (appId != null && mediaid != null)
                    //    {
                    //        IResponseMessageBase reponseMessage = null;
                    //        var strongResponseMessage = CreateResponseMessage<ResponseMessageImage>();
                    //        reponseMessage = strongResponseMessage;
                    //        strongResponseMessage.Image.MediaId = mediaid;
                    //        Subscribe(requestMessage);
                    //        return reponseMessage;
                    //    }
                    //}
                    else
                    {
                        var responseMessagePic = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);
                        responseMessagePic.ArticleCount = 1;
                        responseMessagePic.Articles.Add(GetPicSubscribe());
                        //Subscribe(requestMessage);
                        return responseMessagePic;
                    }
                }
                return new SuccessResponseMessage();
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("微信关注推送消息失败 error：{0} Exception：{1}", ex.Message, ex);
                return new SuccessResponseMessage();
            }
        }

        /// <summary>
        /// 默认回复
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            /* 所有没有被处理的消息会默认返回这里的结果，
            * 因此，如果想把整个微信请求委托出去（例如需要使用分布式或从其他服务器获取请求），
            * 只需要在这里统一发出委托请求，如：
            * var responseMessage = MessageAgent.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
            * return responseMessage;
            */
            if (this.MessageInfo.KeyWords.Keys.Contains("默认"))
            {
                var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
                responseMessage.Content = this.MessageInfo.KeyWords["默认"];
                return responseMessage;
            }
            else
            {
                var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);
                responseMessage.ArticleCount = 1;
                responseMessage.Articles.Add(GetPicSubscribe());
                return responseMessage;
            }
            //return new SuccessResponseMessage();
        }

        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            //说明：实际项目中这里的逻辑可以交给Service处理具体信息，参考OnLocationRequest方法或/Service/LocationSercice.cs
            var defaultResponseMessage = base.CreateResponseMessage<ResponseMessageText>();

            var requestHandler = requestMessage.StartHandler();

            requestHandler.Keywords(new string[] { "使用指南", "使用", "使用手册", "如何使用" }, () =>
            {
                var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);
                responseMessage.ArticleCount = 1;
                responseMessage.Articles.Add(GetPicSubscribe());
                return responseMessage;
            });

            foreach (var item in this.MessageInfo.KeyWords)
            {
                if (item.Key == "默认")
                {
                    requestHandler.Default(() =>
                    {
                        //var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
                        //responseMessage.Content = this.MessageInfo.KeyWords["默认"];
                        var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);
                        responseMessage.ArticleCount = 1;
                        responseMessage.Articles.Add(GetPicSubscribe());
                        return responseMessage;
                    });
                }
                else
                {
                    //如果有逗号表示数组
                    if (item.Key.Contains(','))
                    {
                        requestHandler.Keywords(item.Key.Split(','), () =>
                        {
                            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
                            responseMessage.Content = item.Value;
                            return responseMessage;
                        });
                    }
                    //表示关键字
                    else
                    {
                        requestHandler.Keyword(item.Key, () =>
                        {
                            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
                            responseMessage.Content = item.Value;
                            return responseMessage;
                        });
                    }
                }
            }
            foreach (var item in this.MessageInfo.KeyWordsPic)
            {
                if (item.Key == "默认")
                {
                    requestHandler.Default(() =>
                    {
                        //var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
                        //responseMessage.Content = this.MessageInfo.KeyWords["默认"];
                        var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);
                        responseMessage.ArticleCount = 1;
                        responseMessage.Articles.Add(GetPicSubscribe());
                        return responseMessage;
                    });
                }
                else
                {
                    requestHandler.Keyword(item.Key, () =>
                    {
                        var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);
                        responseMessage.ArticleCount = 1;
                        responseMessage.Articles.Add(item.Value);
                        return responseMessage;
                    });
                }
            }
            return requestHandler.GetResponseMessage() as IResponseMessageBase;
        }

        /// <summary>
        /// 从数据库中获取图文配置
        /// </summary>
        /// <returns></returns>
        public virtual Article GetPicSubscribe()
        {
            //var subscribe = _wechatsubscribeRepository.GetAll().FirstOrDefault();
            var subscribe = GetWechatSubscribe();
            return new Article()
            {
                Title = subscribe.Title,
                Description = subscribe.Desc,
                PicUrl = subscribe.PicLink,
                Url = subscribe.Content
            };
        }
    }
}

using HC.WeChat.Configuration;
using HC.WeChat.MessageHandler;
using HC.WeChat.Web;
using HC.WeChat.WechatAppConfigs;
using HC.WeChat.WechatAppConfigs.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.WeChat.Controllers
{
    public abstract class WeChatMessageHandlerControllerBase : WeChatControllerBase
    {
        readonly Func<string> _getRandomFileName = () => DateTime.Now.ToString("yyyyMMdd-HHmmss") + Guid.NewGuid().ToString("n").Substring(0, 6);
        protected abstract int? GetTenantId();
        private int? TenantId { get; set; }

        protected WechatAppConfigInfo WechatAppConfig { get; set; }

        IMessageHandlerAppServer _messageHandlerAppServer;
        IWechatAppConfigAppService _wechatAppConfigAppService;

        public WeChatMessageHandlerControllerBase(IMessageHandlerAppServer messageHandlerAppServer, IWechatAppConfigAppService wechatAppConfigAppService) : base()
        {
            _messageHandlerAppServer = messageHandlerAppServer;
            _wechatAppConfigAppService = wechatAppConfigAppService;
            //InitAppConfigSetting();
        }

        protected virtual void InitAppConfigSetting()
        {
            TenantId = GetTenantId();
            WechatAppConfig = _wechatAppConfigAppService.GetWechatAppConfig(TenantId).Result;
            //WeChatRegister();
        }

        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://sdk.weixin.senparc.com/weixin
        /// </summary>
        [HttpGet]
        [ActionName("Index")]
        public virtual ActionResult Get(PostModel postModel, string echostr)
        {
            //Logger.Info("get:" + JObject.FromObject(postModel).ToString() + "token:" + token);
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, WechatAppConfig.Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + postModel.Signature + "," + Senparc.Weixin.MP.CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, WechatAppConfig.Token) + "。" +
                    "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
        }

        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// PS：此方法为简化方法，效果与OldPost一致。
        /// v0.8之后的版本可以结合Senparc.Weixin.MP.MvcExtension扩展包，使用WeixinResult，见MiniPost方法。
        /// </summary>
        [HttpPost]
        [ActionName("Index")]
        public virtual ActionResult Post(PostModel postModel)
        {
            //InitAppConfigSetting();
            //Logger.Info("post:" + JObject.FromObject(postModel).ToString() + "TenantId:" + TenantId);
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, WechatAppConfig.Token))
            {
                return Content("参数错误！");
            }

            #region 打包 PostModel 信息

            postModel.Token = WechatAppConfig.Token;//根据自己后台的设置保持一致
            postModel.EncodingAESKey = WechatAppConfig.EncodingAESKey;//根据自己后台的设置保持一致
            postModel.AppId = WechatAppConfig.AppId;//根据自己后台的设置保持一致

            #endregion

            var returnMsg = _messageHandlerAppServer.MessageHandler(postModel, Request.Body, TenantId).Result;
            return Content(returnMsg);
        }

        protected virtual void WeChatRegister()
        {
            //注册公众号
            //AccessTokenContainer.Register(WechatAppConfig.AppId, WechatAppConfig.AppSecret);
        }
    }

    /// <summary>
    /// 获取租户和配置
    /// </summary>
    public abstract class WeChatWebControllerBase : WeChatControllerBase
    {
        protected abstract int? GetTenantId();
        private int? TenantId { get; set; }

        protected WechatAppConfigInfo WechatAppConfig { get; set; }

        IWechatAppConfigAppService _wechatAppConfigAppService;

        public WeChatWebControllerBase(IWechatAppConfigAppService wechatAppConfigAppService) : base()
        {
            _wechatAppConfigAppService = wechatAppConfigAppService;
            //InitAppConfigSetting();
        }

        protected virtual void InitAppConfigSetting()
        {
            TenantId = GetTenantId();
            WechatAppConfig = _wechatAppConfigAppService.GetWechatAppConfig(TenantId).Result;
        }
        
    }
}

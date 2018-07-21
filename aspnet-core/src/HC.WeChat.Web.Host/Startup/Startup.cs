using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Castle.Facilities.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.Extensions;
using HC.WeChat.Authentication.JwtBearer;
using HC.WeChat.Configuration;
using HC.WeChat.Identity;
using Senparc.Weixin.Entities;
using Senparc.Weixin.Threads;
using Microsoft.Extensions.Options;
using Senparc.Weixin.MP.Containers;
using HC.WeChat.Models.WeChat;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;

#if FEATURE_SIGNALR
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using Abp.Owin;
using HC.WeChat.Owin;
#elif FEATURE_SIGNALR_ASPNETCORE
using Abp.AspNetCore.SignalR.Hubs;
#endif

namespace HC.WeChat.Web.Host.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IHostingEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // MVC
            services.AddMvc(
                options => options.Filters.Add(new CorsAuthorizationFilterFactory(_defaultCorsPolicyName))
            ).AddJsonOptions(options =>
                {
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm";
                });
            //添加session
            services.AddSession();

            //设置文件上传大小限制
            services.Configure<FormOptions>(x =>
            {
                x.MemoryBufferThreshold = int.MaxValue;
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
            });

            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

            //添加Senparc.Weixin配置文件（内容可以根据需要对应修改）
            services.Configure<SenparcWeixinSetting>(_appConfiguration.GetSection("SenparcWeixinSetting"));
            services.Configure<WeChatTenantSetting>(_appConfiguration.GetSection("WeChatTenantSetting"));

#if FEATURE_SIGNALR_ASPNETCORE
            services.AddSignalR();
#endif

            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            "*"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                )
            );

            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "WeChat API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);

                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("bearerAuth", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                // Assign scope requirements to operations based on AuthorizeAttribute
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            // Configure Abp and Dependency Injection
            return services.AddAbp<WeChatWebHostModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        {
            app.UseAbp(options => { options.UseAbpRequestLocalization = false; }); // Initializes ABP framework.

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            app.UseStaticFiles();
            //使用session
            app.UseSession();

            app.UseAuthentication();

            app.UseAbpRequestLocalization();

#if FEATURE_SIGNALR
            // Integrate with OWIN
            app.UseAppBuilder(ConfigureOwinServices);
#elif FEATURE_SIGNALR_ASPNETCORE
            app.UseSignalR(routes =>
            {
                routes.MapHub<AbpCommonHub>("/signalr");
            });
#endif
            //404错跳转配置
            //app.UseStatusCodePagesWithRedirects("/GAWX/Error/{0}");
            //app.UseStatusCodePagesWithRedirects("/GAWX/Error");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            #region 微信相关

            ////注册微信
            //AccessTokenContainer.Register(senparcWeixinSetting.Value.WeixinAppId, senparcWeixinSetting.Value.WeixinAppSecret);

            //Senparc.Weixin SDK 配置
            Senparc.Weixin.Config.IsDebug = true;
            Senparc.Weixin.Config.DefaultSenparcWeixinSetting = senparcWeixinSetting.Value;

            //提供网站根目录
            if (env.ContentRootPath != null)
            {
                Senparc.Weixin.Config.RootDictionaryPath = env.ContentRootPath;
            }



            /* 微信配置开始
             * 
             * 建议按照以下顺序进行注册，尤其须将缓存放在第一位！
             */

            //RegisterWeixinCache(app);       //注册分布式缓存（按需，如果需要，必须放在第一个）
            ConfigWeixinTraceLog();         //配置微信跟踪日志（按需）
            RegisterWeixinThreads();        //激活微信缓存及队列线程（必须）
            RegisterSenparcWeixin();        //注册Demo所用微信公众号的账号信息（按需）
            //RegisterSenparcWorkWeixin();    //注册Demo所用企业微信的账号信息（按需）
            //RegisterWeixinPay();            //注册微信支付（按需）
            //RegisterWeixinThirdParty();     //注册微信第三方平台（按需）

            /* 微信配置结束 */

            #endregion

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.InjectOnCompleteJavaScript("/swagger/ui/abp.js");
                options.InjectOnCompleteJavaScript("/swagger/ui/on-complete.js");
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "WeChat API V1");
            }); // URL: /swagger

            app.Run(async (context) =>
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    var url = context.Request.GetAbsoluteUri();
                    if (url.Contains("/gawechat/") && !url.Contains("/index.html"))
                    {
                        context.Response.Redirect(url.Replace("/gawechat/", "/gawechat/index.html"));
                    }
                });

            });
        }

#if FEATURE_SIGNALR
        private static void ConfigureOwinServices(IAppBuilder app)
        {
            app.Properties["host.AppName"] = "WeChat";

            app.UseAbp();
            
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    EnableJSONP = true
                };
                map.RunSignalR(hubConfiguration);
            });
        }
#endif
        /// 配置微信跟踪日志
        /// </summary>
        private void ConfigWeixinTraceLog()
        {
            //这里设为Debug状态时，/App_Data/WeixinTraceLog/目录下会生成日志文件记录所有的API请求日志，正式发布版本建议关闭
            Senparc.Weixin.Config.IsDebug = true;
            Senparc.Weixin.WeixinTrace.SendCustomLog("系统日志", "系统启动");//只在Senparc.Weixin.Config.IsDebug = true的情况下生效

            //自定义日志记录回调
            Senparc.Weixin.WeixinTrace.OnLogFunc = () =>
            {
                //加入每次触发Log后需要执行的代码
            };

            //当发生基于WeixinException的异常时触发
            Senparc.Weixin.WeixinTrace.OnWeixinExceptionFunc = ex =>
            {
                //加入每次触发WeixinExceptionLog后需要执行的代码

                //发送模板消息给管理员
                //var eventService = new EventService();
                //eventService.ConfigOnWeixinExceptionFunc(ex);
            };
        }

        /// <summary>
        /// 激活微信缓存
        /// </summary>
        private void RegisterWeixinThreads()
        {
            ThreadUtility.Register();//如果不注册此线程，则AccessToken、JsTicket等都无法使用SDK自动储存和管理。
        }

        /// <summary>
        /// 注册Demo所用微信公众号的账号信息
        /// </summary>
        private void RegisterSenparcWeixin()
        {
            var senparcWeixinSetting = Senparc.Weixin.Config.DefaultSenparcWeixinSetting;

            //注册公众号
            AccessTokenContainer.Register(
                senparcWeixinSetting.WeixinAppId,
                senparcWeixinSetting.WeixinAppSecret,
                "公众号");

            ////注册小程序（完美兼容）
            //AccessTokenContainer.Register(
            //    senparcWeixinSetting.WxOpenAppId,
            //    senparcWeixinSetting.WxOpenAppSecret,
            //    "【盛派互动】小程序");
        }
    }
}

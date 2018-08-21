using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using System;
using Abp.Dependency;
using Abp.Domain.Uow;
using HC.WeChat.Products;
using HC.WeChat.MemberConfigs;
using HC.WeChat.Retailers;

namespace HC.WeChat.UpdateShopReportDataJob
{
    public class UpdateShopReportDataWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IRetailerRepository _retailerRepository;
        int i = 0;
        private DateTime preDate = DateTime.Now.AddDays(-1);//用于控制在合适时间段中只执行一次档级更新(保证只会去数据库去请求一次levellog的存在)
        public UpdateShopReportDataWorker(AbpTimer timer
            , IRetailerRepository retailerRepository
            ) : base(timer)
        {
            //Timer.Period = 3600000;
            Timer.Period = 1800000;
            _retailerRepository = retailerRepository;
            //启动日志
            //Logger.InfoFormat("启动job时间：{0}", DateTime.Now);
            //DoWork();
        }

        protected override void DoWork()
        {
            if (i==0)
            {
                Logger.InfoFormat("进入店铺数据报表job开始时间：{0}", DateTime.Now);
                Logger.InfoFormat("执行店铺数据报表job逻辑开始时间：{0}", DateTime.Now);
                string startTime = DateTime.Now.ToString("HH");
                if (startTime == "01")
                {
                    _retailerRepository.UpdateShopReportDataJob();
                    Logger.InfoFormat("店铺数据报表job已成功执行：{0}", DateTime.Now);
                }
                Logger.InfoFormat("执行店铺数据报表job逻辑结束时间：{0}", DateTime.Now);
                i++;
            }
           
        }
    }
}

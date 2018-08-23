using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HC.WeChat.Retailers
{
    public interface IRetailerRepository : IRepository<Retailer, Guid>
    {
        Task<List<ShopReportData>> GetShopReportAsync();

        /// <summary>
        /// 每日定时执行报表存储过程
        /// </summary>
        /// <returns></returns>
        Task<bool> UpdateShopReportDataJob();
    }
}

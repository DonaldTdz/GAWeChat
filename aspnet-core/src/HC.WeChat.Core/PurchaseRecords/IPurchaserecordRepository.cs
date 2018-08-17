using Abp.Domain.Repositories;
using HC.WeChat.PurchaseRecords;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HC.WeChat
{
    public interface IPurchaserecordRepository : IRepository<PurchaseRecord, Guid>
    {
        Task<List<UserSpecification>> GetShopFavouriteSpecificationAsync(string shopId,string openIds);
    }
}

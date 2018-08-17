using Abp.Data;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using HC.WeChat.PurchaseRecords;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace HC.WeChat.EntityFrameworkCore.Repositories
{
    public class PurchaserecordRepository : WeChatRepositoryBase<PurchaseRecord, Guid>, IPurchaserecordRepository
    {
        private readonly IActiveTransactionProvider _transactionProvider;

        public PurchaserecordRepository(IDbContextProvider<WeChatDbContext> dbContextProvider
            , IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider)
        {
            _transactionProvider = transactionProvider;
        }

        private DbCommand CreateCommand(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            var command = Context.Database.GetDbConnection().CreateCommand();

            command.CommandText = commandText;
            command.CommandType = commandType;
            command.Transaction = GetActiveTransaction();

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return command;
        }
        private void EnsureConnectionOpen()
        {
            var connection = Context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        private DbTransaction GetActiveTransaction()
        {
            return (DbTransaction)_transactionProvider.GetActiveTransaction(new ActiveTransactionProviderArgs
                 {
                    {"ContextType", typeof(WeChatDbContext) },
                    {"MultiTenancySide", MultiTenancySide }
                 });
        }

        public async Task<List<UserSpecification>> GetShopFavouriteSpecificationAsync(string ShopId,string openIds)
        {
            EnsureConnectionOpen();
            SqlParameter[] sql = new SqlParameter[]
           {
                new SqlParameter("@ShopId",ShopId),
                new SqlParameter("@OpenIdIds",openIds)
           };
            //using (var command = CreateCommand("select* from(select ROW_NUMBER() over(partition by NickName order by num desc) gnum, *from(select WeChatUsers.NickName, PurchaseRecords.Specification, sum(PurchaseRecords.Quantity) num from PurchaseRecords inner join WeChatUsers on PurchaseRecords.OpenId = WeChatUsers.OpenId where ShopId = @ShopId group by WeChatUsers.NickName, PurchaseRecords.Specification)temp) temp2 where gnum = 1", CommandType.Text, sql))
            using (var command = CreateCommand("select * from(select ROW_NUMBER() over(partition by OpenId order by Num desc) gnum, * from (select OpenId,Specification, sum(Quantity) Num from PurchaseRecords where ShopId = @ShopId and charindex(','+OpenId+',',','+@OpenIdIds+',') > 0 group by OpenId,Specification )temp) temp2 where gnum = 1", CommandType.Text, sql))
            //using (var command = CreateCommand("select * from PurchaseRecords where ShopName like @ShopName", CommandType.Text,sql))
            {
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    var result = new List<UserSpecification>();
                    while (dataReader.Read())
                    {
                        var userSpecification = new UserSpecification();
                        userSpecification.OpenId = dataReader["OpenId"].ToString();
                        userSpecification.Specification = dataReader["Specification"].ToString();
                        userSpecification.Num = (int)dataReader["Num"];
                        result.Add(userSpecification);
                    }
                    return result;
                }
            }
        }
    }
}

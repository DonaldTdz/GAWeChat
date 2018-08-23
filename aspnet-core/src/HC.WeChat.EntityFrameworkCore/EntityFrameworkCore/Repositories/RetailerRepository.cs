using Abp.Data;
using Abp.EntityFrameworkCore;
using HC.WeChat.Retailers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HC.WeChat.EntityFrameworkCore.Repositories
{
    public class RetailerRepository : WeChatRepositoryBase<Retailer, Guid>, IRetailerRepository
    {
        private readonly IActiveTransactionProvider _transactionProvider;
        public RetailerRepository(IDbContextProvider<WeChatDbContext> dbContextProvider
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

        public async Task<List<ShopReportData>> GetShopReportAsync()
        {
            EnsureConnectionOpen();

            //using (var command = CreateCommand("select * from ShopReportData", CommandType.Text))
            using (var command = CreateCommand("USP_GetShopReportDate", CommandType.StoredProcedure))
            {
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    var result = new List<ShopReportData>();

                    while (dataReader.Read())
                    {
                        var shopReportData = new ShopReportData();
                        shopReportData.RootId = (int)dataReader["RootId"];
                        shopReportData.CompanyId = (int)dataReader["CompanyId"];
                        shopReportData.AreaId = (int)dataReader["AreaId"];
                        shopReportData.SlsmanNameId = (int)dataReader["SlsmanNameId"];
                        shopReportData.GroupNum = (int)dataReader["GroupNum"];
                        shopReportData.Organization = dataReader["Organization"].ToString();
                        shopReportData.ShopTotal = (int?)dataReader["ShopTotal"];
                        shopReportData.ScanQuantity = (int?)dataReader["ScanQuantity"];
                        shopReportData.ScanFrequency = (int?)dataReader["ScanFrequency"];
                        shopReportData.PriceTotal = (decimal)dataReader["PriceTotal"];
                        shopReportData.CustIntegral = (int?)dataReader["CustIntegral"];
                        shopReportData.RetailerIntegral = (int?)dataReader["RetailerIntegral"];
                        //shopReportData.CreationTime = (DateTime?)dataReader["CreationTime"];
                        result.Add(shopReportData);
                    }
                    return result;
                }
            }
        }

        /// <summary>
        /// 执行存储过程脚本
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UpdateShopReportDataJob()
        {
            EnsureConnectionOpen();

            using (var command = CreateCommand("USP_UpdateShopReportData", CommandType.StoredProcedure))
            {
                int result = await command.ExecuteNonQueryAsync();
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
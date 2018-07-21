using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HC.WeChat.VoteLogs.Dtos;
using HC.WeChat.VoteLogs;
using System;
using HC.WeChat.Dto;

namespace HC.WeChat.VoteLogs
{
    /// <summary>
    /// VoteLog应用层服务的接口方法
    /// </summary>
    public interface IVoteLogAppService : IApplicationService
    {
        /// <summary>
        /// 获取VoteLog的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<VoteLogListDto>> GetPagedVoteLogs(GetVoteLogsInput input);

            /// <summary>
            /// 通过指定id获取VoteLogListDto信息
            /// </summary>
            Task<VoteLogListDto> GetVoteLogByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 导出VoteLog为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetVoteLogsToExcel();

        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetVoteLogForEditOutput> GetVoteLogForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetVoteLogForEditOutput

        /// <summary>
        /// 添加或者修改VoteLog的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateVoteLog(CreateOrUpdateVoteLogInput input);

        /// <summary>
        /// 删除VoteLog信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteVoteLog(EntityDto<Guid> input);

            /// <summary>
            /// 批量删除VoteLog
            /// </summary>
        Task BatchDeleteVoteLogsAsync(List<Guid> input);
        Task<int> GetWXVotesCountAsync();
        Task<APIResultDto> AddVoteLogAsync(VoteLogEditDto input);
        Task<int?> GetCurrentDayVoteByIdAsync(string openId);
    }
}

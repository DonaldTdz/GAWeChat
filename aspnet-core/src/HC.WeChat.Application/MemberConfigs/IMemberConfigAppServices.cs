using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HC.WeChat.MemberConfigs.Dtos;
using HC.WeChat.MemberConfigs;
using System;

namespace HC.WeChat.MemberConfigs
{
    /// <summary>
    /// MemberConfig应用层服务的接口方法
    /// </summary>
    public interface IMemberConfigAppService : IApplicationService
    {
        /// <summary>
        /// 获取MemberConfig的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<MemberConfigListDto>> GetPagedMemberConfigs(GetMemberConfigsInput input);

        /// <summary>
        /// 通过指定id获取MemberConfigListDto信息
        /// </summary>
        Task<MemberConfigListDto> GetMemberConfigByIdAsync(EntityDto<Guid> input);

        /// <summary>
        /// 导出MemberConfig为excel表
        /// </summary>
        /// <returns></returns>
        //  Task<FileDto> GetMemberConfigsToExcel();
        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetMemberConfigForEditOutput> GetMemberConfigForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetMemberConfigForEditOutput
        /// <summary>
        /// 添加或者修改MemberConfig的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateMemberConfig(CreateOrUpdateMemberConfigInput input);

        /// <summary>
        /// 删除MemberConfig信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteMemberConfig(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除MemberConfig
        /// </summary>
        Task BatchDeleteMemberConfigsAsync(List<Guid> input);
        Task<List<MemberConfigListDto>> GetTenanMemberConfigAsync();
        Task CreateOrUpdateMemberConfigDtoAsync(MemberCodeEditDto input);

        Task<List<MemberConfigListDto>> GetWXMemberConfigByTenantIdAsync(int? tenantId);
        Task CreateOrUpdateWXMemberConfigDtoAsync(MemberCodeEditDto input);

        /// <summary>
        /// 获取job配置信息
        /// </summary>
        /// <returns></returns>
        MemberConfigListDto GetJobConfig();

        Task<MemberConfigListDto> GetPreProductConfigAsync();

        Task CreateOrUpdatePreProductConfig(MemberConfigEditDto input);
        Task<string> GetWXPreProductConfigAsync(int? tenantId);
    }
}

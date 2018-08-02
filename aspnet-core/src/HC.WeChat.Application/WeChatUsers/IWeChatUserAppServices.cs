using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HC.WeChat.WeChatUsers.Dtos;
using HC.WeChat.WeChatUsers;
using System;
using HC.WeChat.Dto;

namespace HC.WeChat.WeChatUsers
{
    /// <summary>
    /// WeChatUser应用层服务的接口方法
    /// </summary>
    public interface IWeChatUserAppService : IApplicationService
    {
        /// <summary>
        /// 获取WeChatUser的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<WeChatUserListDto>> GetPagedWeChatUsers(GetWeChatUsersInput input);

        /// <summary>
        /// 通过指定id获取WeChatUserListDto信息
        /// </summary>
        Task<WeChatUserListDto> GetWeChatUserByIdAsync(EntityDto<Guid> input);

        /// <summary>
        /// 通过指定id获取WeChatUserListDto信息
        /// </summary>
        Task<WeChatUserListDto> GetWeChatUserByMemberBarCodeAsync(string memberBarCode, int? tenantId);

        /// <summary>
        /// 导出WeChatUser为excel表
        /// </summary>
        /// <returns></returns>
        //  Task<FileDto> GetWeChatUsersToExcel();
        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetWeChatUserForEditOutput> GetWeChatUserForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetWeChatUserForEditOutput
        /// <summary>
        /// 添加或者修改WeChatUser的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateWeChatUser(CreateOrUpdateWeChatUserInput input);

        /// <summary>
        /// 删除WeChatUser信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteWeChatUser(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除WeChatUser
        /// </summary>
        Task BatchDeleteWeChatUsersAsync(List<Guid> input);

        Task<APIResultDto> BindWeChatUserAsync(UserBindDto input);

        Task<WeChatUserListDto> GetWeChatUserAsync(string openId, int? tenantId);

        /// <summary>
        /// 添加或者修改WeChatUser的方法
        /// </summary>
        Task CreateOrUpdateWeChatUserDto(WeChatUserEditDto input);

        /// <summary>
        /// 绑定会员
        /// </summary>
        Task<APIResultDto> BindMemberAsync(MemberBindDto input);

        /// <summary>
        /// 获取店员信息
        /// </summary>
        Task<PagedResultDto<WeChatUserListDto>> GetPagedShopWeChatUsers(GetShopWeChatUsersInput input);

        /// <summary>
        /// 获取单个微信用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        Task<WeChatUserListDto> GetSingleWeChatUser(Guid userId, int? tenantId);

        /// <summary>
        /// 解除绑定
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CheckWeChatUserBindStatusAsync(WeChatUserEditDto input);

        /// <summary>
        /// 获取店员信息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<WeChatUserListDto>> GetShopEmployeesAsync(int? tenantId, Guid userId);

        /// <summary>
        /// 审核店员 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<APIResultDto> CheckShopEmployeeAsync(WeChatUserEditDto input);

        /// <summary>
        /// 获取未审核店员人数
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetShopEmployeesNoCheckCountAsync(int? tenantId, Guid userId);
        Task<APIResultDto> ExportWeChatUsersExcel(GetWeChatUsersInput input);
        Task<APIResultDto> ExportWeChatUsersIntegralExcelAsync(GetWeChatUsersInput input);

        /// <summary>
        /// 微信用户统计（按零售户分公司统计）
        /// </summary>
        /// <returns></returns>
        Task<WeChatUserStatisticLiDto> GetWeChatUserStatistic();

        /// <summary>
        /// 是否存在该微信用户
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        Task<bool> GetWeChatUserIsExsit(string openId);

        Task<bool> GetIsAttentionByOpenIdAsync(string openId);

        Task SubscribeAsync(string openId, string nickName, string headImgUrl, string scene, string ticket);

        Task UnsubscribeAsync(string openId);

        Task<APIResultDto> GetWechatUserOpenIds(string nextOpenId);

        Task<APIResultDto> SynchronouWechatUserByOpenIds(string[] openIds);
    }
}

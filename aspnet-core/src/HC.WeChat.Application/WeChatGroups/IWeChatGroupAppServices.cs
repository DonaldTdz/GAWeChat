using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HC.WeChat.WeChatGroups.Dtos;
using HC.WeChat.WeChatGroups;
using HC.WeChat.Dto;
using HC.WeChat.WechatEnums;

namespace HC.WeChat.WeChatGroups
{
    /// <summary>
    /// WeChatGroup应用层服务的接口方法
    /// </summary>
    public interface IWeChatGroupAppService : IApplicationService
    {
        /// <summary>
        /// 获取WeChatGroup的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<WeChatGroupListDto>> GetPagedWeChatGroups(GetWeChatGroupsInput input);

        /// <summary>
        /// 通过指定id获取WeChatGroupListDto信息
        /// </summary>
        Task<WeChatGroupListDto> GetWeChatGroupByIdAsync(EntityDto<int> input);

        /// <summary>
        /// 导出WeChatGroup为excel表
        /// </summary>
        /// <returns></returns>
        //  Task<FileDto> GetWeChatGroupsToExcel();
        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetWeChatGroupForEditOutput> GetWeChatGroupForEdit(NullableIdDto<int> input);

        //todo:缺少Dto的生成GetWeChatGroupForEditOutput
        /// <summary>
        /// 添加或者修改WeChatGroup的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateWeChatGroup(CreateOrUpdateWeChatGroupInput input);

        /// <summary>
        /// 删除WeChatGroup信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteWeChatGroup(EntityDto<int> input);

        /// <summary>
        /// 批量删除WeChatGroup
        /// </summary>
        Task BatchDeleteWeChatGroupsAsync(List<int> input);

        /// <summary>
        /// 获取所有分组信息
        /// </summary>
        /// <returns></returns>
        Task<List<WeChatGroupListDto>> GetAllWeChatGroupAsync();

        /// <summary>
        /// 创建分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateWeChatGroup(WeChatGroupListDto input);

        /// <summary>
        /// 修改分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateWeChatGroup(WeChatGroupListDto input);

        /// <summary>
        ///  删除分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteWeChatGroupAsync(WeChatGroupListDto input);

        /// <summary>
        /// 单个标记用户分组
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="tagId"></param>
        /// <returns></returns>
        Task MarkWeChatGroup(string openId, int tagId);

        /// <summary>
        /// 批量标记用户分组
        /// </summary>
        /// <returns></returns>
        Task BatchMarkWeChatGroup();

        /// <summary>
        /// 获取在微信中的分组（标签）
        /// </summary>
        /// <returns></returns>
        Task GetWeChatGroupFromWeChat();

        /// <summary>
        /// 获取用户上的标签
        /// </summary>
        /// <returns></returns>
        Task GetUserForWeChatUser();

        /// <summary>
        /// 检查在微信端是否已存在此标签
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        Task<CheckResult> CheckTagName(string tagName);

        /// <summary>
        /// 检查标签是否存在，不存在就创建
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        Task<int> GetTagIdAsync(UserTypeEnum typeCode);

        /// <summary>
        /// 为用户取消标签
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="openIds"></param>
        /// <returns></returns>
        Task CancelTagAsync(int tagId, List<string> openIds);

        /// <summary>
        /// 通过用户类型获取分组信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<WeChatGroupListDto> GetWeChatGroupByUserType(UserTypeEnum code);
        Task<int> GetTagIdByNameAsync(string groupName, int groupCode);
    }
}

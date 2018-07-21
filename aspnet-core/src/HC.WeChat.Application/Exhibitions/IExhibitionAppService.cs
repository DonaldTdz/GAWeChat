using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HC.WeChat.Exhibitions.Dtos;
using HC.WeChat.Exhibitions;
using System;

namespace HC.WeChat.Exhibitions
{
    /// <summary>
    /// Exhibition应用层服务的接口方法
    /// </summary>
    public interface IExhibitionAppService : IApplicationService
    {
        /// <summary>
        /// 获取Exhibition的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ExhibitionListDto>> GetPagedExhibitions(GetExhibitionsInput input);

        /// <summary>
        /// 获取ExhibitionListDto信息
        /// </summary>
        Task<ExhibitionListDto> GetExhibitionByIdAsync();

        /// <summary>
        /// 导出Exhibition为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetExhibitionsToExcel();

        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetExhibitionForEditOutput> GetExhibitionForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetExhibitionForEditOutput

        /// <summary>
        /// 添加或者修改Exhibition的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateExhibition(ExhibitionEditDto input);

        /// <summary>
        /// 删除Exhibition信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteExhibition(EntityDto<Guid> input);

            /// <summary>
            /// 批量删除Exhibition
            /// </summary>
        Task BatchDeleteExhibitionsAsync(List<Guid> input);
        Task<ExhibitionEditDto> GetExhibitionConfigAsync();
    }
}

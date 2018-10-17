using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using System.Linq;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;

using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using HC.WeChat.Exhibitions.Authorization;
using HC.WeChat.Exhibitions.DomainServices;
using HC.WeChat.Exhibitions.Dtos;
using HC.WeChat.Exhibitions;
using System;
using HC.WeChat.Authorization;
using Abp.Auditing;

namespace HC.WeChat.Exhibitions
{
    /// <summary>
    /// Exhibition应用层服务的接口实现方法
    /// </summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class ExhibitionAppService : WeChatAppServiceBase, IExhibitionAppService
    {
        private readonly IRepository<Exhibition, Guid> _exhibitionRepository;
        private readonly IExhibitionManager _exhibitionManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ExhibitionAppService(
            IRepository<Exhibition, Guid> exhibitionRepository
      , IExhibitionManager exhibitionManager
        )
        {
            _exhibitionRepository = exhibitionRepository;
            _exhibitionManager = exhibitionManager;
        }

        /// <summary>
        /// 获取Exhibition的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ExhibitionListDto>> GetPagedExhibitions(GetExhibitionsInput input)
        {
            var query = _exhibitionRepository.GetAll();
            //TODO:根据传入的参数添加过滤条件

            var exhibitionCount = await query.CountAsync();

            var exhibitions = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            //var exhibitionListDtos = ObjectMapper.Map<List <ExhibitionListDto>>(exhibitions);
            var exhibitionListDtos = exhibitions.MapTo<List<ExhibitionListDto>>();

            return new PagedResultDto<ExhibitionListDto>(
                exhibitionCount,
                exhibitionListDtos
                );
        }

        /// <summary>
        /// 通过指定id获取ExhibitionListDto信息
        /// </summary>
        public async Task<ExhibitionListDto> GetExhibitionByIdAsync()
        {
            var entity = await _exhibitionRepository.GetAll().FirstOrDefaultAsync();
            return entity.MapTo<ExhibitionListDto>();
        }

        /// <summary>
        /// 导出Exhibition为excel表
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetExhibitionsToExcel(){

        //var users = await UserManager.Users.ToListAsync();
        //var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
        //await FillRoleNames(userListDtos);

        //return _userListExcelExporter.ExportToFile(userListDtos);

        //}

        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetExhibitionForEditOutput> GetExhibitionForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetExhibitionForEditOutput();
            ExhibitionEditDto exhibitionEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _exhibitionRepository.GetAsync(input.Id.Value);

                exhibitionEditDto = entity.MapTo<ExhibitionEditDto>();

                //exhibitionEditDto = ObjectMapper.Map<List <exhibitionEditDto>>(entity);


            }
            else
            {
                exhibitionEditDto = new ExhibitionEditDto();
            }

            output.Exhibition = exhibitionEditDto;
            return output;

        }

        /// <summary>
        /// 添加或者修改Exhibition的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateExhibition(ExhibitionEditDto input)
        {
            if (input.Id.HasValue)
            {
                await UpdateExhibitionAsync(input);
            }
            else
            {
                await CreateExhibitionAsync(input);
            }
        }

        /// <summary>
        /// 新增Exhibition
        /// </summary>
        //[AbpAuthorize(ExhibitionAppPermissions.Exhibition_CreateExhibition)]
        protected virtual async Task<ExhibitionEditDto> CreateExhibitionAsync(ExhibitionEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var entity = ObjectMapper.Map<Exhibition>(input);
            entity = await _exhibitionRepository.InsertAsync(entity);
            return entity.MapTo<ExhibitionEditDto>();
        }

        /// <summary>
        /// 编辑Exhibition
        /// </summary>
        //[AbpAuthorize(ExhibitionAppPermissions.Exhibition_EditExhibition)]
        protected virtual async Task UpdateExhibitionAsync(ExhibitionEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _exhibitionRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _exhibitionRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除Exhibition信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(ExhibitionAppPermissions.Exhibition_DeleteExhibition)]
        public async Task DeleteExhibition(EntityDto<Guid> input)
        {

            //TODO:删除前的逻辑判断，是否允许删除
            await _exhibitionRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除Exhibition的方法
        /// </summary>
        [AbpAuthorize(ExhibitionAppPermissions.Exhibition_BatchDeleteExhibitions)]
        public async Task BatchDeleteExhibitionsAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _exhibitionRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 微信获取活动配置
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [DisableAuditing]
        public async Task<ExhibitionEditDto> GetExhibitionConfigAsync()
        {
            var config = await _exhibitionRepository.GetAll().FirstOrDefaultAsync();
            return config.MapTo<ExhibitionEditDto>();
        }
    }
}



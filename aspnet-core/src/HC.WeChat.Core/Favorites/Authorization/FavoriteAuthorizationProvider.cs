
using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using HC.WeChat.Authorization;

namespace HC.WeChat.Favorites.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="FavoriteAppPermissions" /> for all permission names. Favorite
    ///</summary>
    public class FavoriteAppAuthorizationProvider : AuthorizationProvider
    {
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
    //在这里配置了Favorite 的权限。
    var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

    var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

    var favorite = administration.CreateChildPermission(FavoriteAppPermissions.Favorite , L("Favorite"));
favorite.CreateChildPermission(FavoriteAppPermissions.Favorite_CreateFavorite, L("CreateFavorite"));
favorite.CreateChildPermission(FavoriteAppPermissions.Favorite_EditFavorite, L("EditFavorite"));
favorite.CreateChildPermission(FavoriteAppPermissions.Favorite_DeleteFavorite, L("DeleteFavorite"));
favorite.CreateChildPermission(FavoriteAppPermissions.Favorite_BatchDeleteFavorites , L("BatchDeleteFavorites"));



    //// custom codes
 
    //// custom codes end
    }

    private static ILocalizableString L(string name)
    {
    return new LocalizableString(name, WeChatConsts.LocalizationSourceName);
    }
    }
    }
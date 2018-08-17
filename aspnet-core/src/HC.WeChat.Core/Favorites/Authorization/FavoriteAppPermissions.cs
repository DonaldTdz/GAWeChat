

namespace HC.WeChat.Favorites.Authorization
{
	 /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="FavoriteAppAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class FavoriteAppPermissions
	{
		/// <summary>
    /// Favorite管理权限_自带查询授权
    ///</summary>
		public const string Favorite = "Pages.Favorite";

		/// <summary>
    /// Favorite创建权限
    ///</summary>
		public const string Favorite_CreateFavorite = "Pages.Favorite.CreateFavorite";

		/// <summary>
    /// Favorite修改权限
    ///</summary>
		public const string Favorite_EditFavorite = "Pages.Favorite.EditFavorite";

		/// <summary>
    /// Favorite删除权限
    ///</summary>
		public const string Favorite_DeleteFavorite = "Pages.Favorite.DeleteFavorite";

        /// <summary>
    /// Favorite批量删除权限
    ///</summary>
		public const string Favorite_BatchDeleteFavorites = "Pages.Favorite.BatchDeleteFavorites";



		//// custom codes
 
        //// custom codes end
    }

}


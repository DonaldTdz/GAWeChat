

using AutoMapper;
using HC.WeChat.Favorites;

namespace HC.WeChat.Favorites.Dtos.CustomMapper
{

	/// <summary>
    /// 配置Favorite的AutoMapper
    ///</summary>
	internal static class CustomerFavoriteMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Favorite, FavoriteListDto>
    ();
    configuration.CreateMap <FavoriteEditDto, Favorite>
        ();



        //// custom codes
         
        //// custom codes end

        }
        }
        }

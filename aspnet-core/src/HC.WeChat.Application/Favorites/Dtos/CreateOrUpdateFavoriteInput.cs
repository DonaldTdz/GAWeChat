

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.WeChat.Favorites;

namespace HC.WeChat.Favorites.Dtos
{
    public class CreateOrUpdateFavoriteInput
    {
        [Required]
        public FavoriteEditDto Favorite { get; set; }



		//// custom codes
 
        //// custom codes end
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Sakila.Models
{
    public class CustomerDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? FavoriteArtistId { get; set; }
        public int? FavoriteMovieId { get; set; }
        public int? FavoriteCategoryId { get; set; }
    }
}

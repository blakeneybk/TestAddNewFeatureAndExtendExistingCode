using System;
using System.Collections.Generic;
using System.Text;

namespace Sakila.Models
{
    public class MovieDetails
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public uint ReleaseYear { get; set; }
        public uint Length { get; set; }
        public string Rating { get; set; }
    }
}

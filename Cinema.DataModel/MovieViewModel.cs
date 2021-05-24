using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.DataModel
{
    public class MovieViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IFormFile Poster { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string Rating { get; set; }
        public string Type { get; set; }
        public string Genre { get; set; }
        public string Actors { get; set; }
        public string Directors { get; set; }
        public string Trailer { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Models
{
    public class MovieList
    {
        public string Title { get; set; }
        public string Year { get; set; }                    
        public string ID { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }
}
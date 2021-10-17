using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWSCookBook.Models
{
    public class Recette
    {
        public string URL { get; set; }
        public string AltText { get; set; }
        public string ImageURL { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public List<string> Ingredients { get; set; } = new List<string>();
        public List<string> Preparation { get; set; } = new List<string>();
    }
}

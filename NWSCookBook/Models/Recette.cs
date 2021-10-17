using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWSCookBook.Models
{
    public class Recette
    {
        public string URL;
        public string ImageURL;
        public string Name;
        public string Author;
        public IEnumerable<string> Ingredients = Array.Empty<string>();
        public IEnumerable<string> Preparation = Array.Empty<string>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NWSCookBook.Models;
using NWSCookBook.ScrapperSystem;

namespace NWSCookBook.Services
{
    public class RecetteService
    {
        public IEnumerable<Recette> Recettes = new List<Recette>();

        public RecetteService Seed()
        {
            Scrapper scrapper = new Scrapper();

            IEnumerable<string> links = scrapper.GetRecipesLinks();
            Recettes = scrapper.GenerateRecipes(links);

            return (this);
        }
    }
}

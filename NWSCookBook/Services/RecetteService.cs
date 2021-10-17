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
        public List<Recette> Recettes = new List<Recette>();

        public void Seed()
        {
            Scrapper scrapper = new Scrapper();

            scrapper.GetRecipesLinks();
        }
    }
}

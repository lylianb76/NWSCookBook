using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NWSCookBook.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NWSCookBook.Services;

namespace NWSCookBook.Controllers
{
    public class HomeController : Controller
    {
        private RecetteService RecetteService;

        public HomeController(RecetteService recetteService)
        {
            RecetteService = recetteService;
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View(RecetteService.Recettes);
        }

        [HttpGet("/Recette/{id}")]
        public IActionResult Recette(string id)
        {
            Recette recette = RecetteService.Recettes.FirstOrDefault(i => i.URL == id);

            if (recette == null)
                return (RedirectToAction("Index"));

            return View(recette);
        }

        [HttpGet("/Refresh")]
        public IActionResult Recette()
        {
            RecetteService.Seed();
            return (Ok());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

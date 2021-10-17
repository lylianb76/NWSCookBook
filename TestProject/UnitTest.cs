using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NWSCookBook.ScrapperSystem;

namespace TestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestScrap()
        {
            Scrapper scrapper = new Scrapper();

            IEnumerable<string> links = scrapper.GetRecipesLinks();
            var r = scrapper.GenerateRecipes(links);
        }
    }
}

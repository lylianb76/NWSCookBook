using System;
using System.Net.Http;

namespace NWSCookBook.ScrapperSystem
{
    public class Scrapper
    {
        public static string BaseURL = "https://github.com/NWS-A1/NWSCookBook/tree/master/Recettes";
        private string HTML;

        public string[] GetRecipesLinks()
        {
            HttpClient client = new HttpClient();

            HTML = client.GetStringAsync(BaseURL).Result;

            UnformattedData data = UnformattedData.Create(HTML, "//tbody/tr");

            return (null);
        }

        /*
        public void DownloadData(Hero hero, Rank rank)
        {
            string url = String.Format("{0}TalentDetails?Hero={1}&League={2}", BaseURL, hero.Name, (int)rank);
            HttpClient client = new HttpClient();

            HTML = client.GetStringAsync(url).Result;
        }

        public IEnumerable<HeroWinrate> GetBestPicksForMap(Map map, Rank rank)
        {
            string url = String.Format("{0}HeroAndMapStatistics?League={1}&Map={2}", BaseURL, (int)rank, map.Name);
            HttpClient client = new HttpClient();
            
            string answer = client.GetStringAsync(url).Result;
            UnformattedData data = UnformattedData.Create(answer, "//tbody/tr");

            IEnumerable<HeroWinrate> results = data.Then(i =>
            {
                string name = i.Then("td[2]/a")
                    .ExtractAttribute("title");

                return new HeroWinrate
                {
                    Hero = HeroStatic.GetWithName(name),
                    WinRate = float.Parse(i.Then("td[6]").ExtractContent('%'))
                };
            });

            return (results);
        }

        public IEnumerable<HeroWinrate> GetHeroBestMatchups()
        {
            UnformattedData data = UnformattedData.Create(HTML, "//div[@id=\"RadGridSitewideCharacterWinPercentVsOtherCharacters\"]/table/tbody/tr");

            IEnumerable<HeroWinrate> results = data.Then(d =>
            {
                string name = d.Then("td[2]/a")
                    .ExtractAttribute("title");

                return (new HeroWinrate
                {
                    Hero = HeroStatic.GetWithName(name),
                    WinRate = float.Parse(d.Then("td[4]").ExtractContent('%'))
                });
            });

            return (results);
        }

        public IEnumerable<HeroWinrate> GetHeroDuos()
        {
            UnformattedData data = UnformattedData.Create(HTML, "//div[@id=\"RadGridSitewideCharacterWinPercentWithOtherCharacters\"]/table/tbody/tr");

            IEnumerable<HeroWinrate> results = data.Then(d =>
            {
                string name = d.Then("td[2]/a")
                    .ExtractAttribute("title");

                return (new HeroWinrate
                {
                    Hero = HeroStatic.GetWithName(name),
                    WinRate = float.Parse(d.Then("td[4]").ExtractContent('%'))
                });
            });

            return (results);
        }

        public IEnumerable<MapWinrate> GetMaps()
        {
            UnformattedData data = UnformattedData.Create(HTML, "//div[@id=\"RadGridMapStatistics\"]/table/tbody/tr");

            IEnumerable<MapWinrate> results = data.Then(d =>
            {
                string name = d.Then("td[2]").ExtractContent('<');

                return (new MapWinrate
                {
                    Map = MapStatic.GetWithName(name),
                    WinRate = float.Parse(d.Then("td[4]").ExtractContent('%'))
                });
            });

            return (results);
        }

        public IEnumerable<TalentWinrate> GetTalents()
        {
            List<TalentWinrate> winrates = new List<TalentWinrate>();
            UnformattedData data = UnformattedData.Create(HTML, "//table[@id=\"ctl00_MainContent_RadGridHeroTalentStatistics_ctl00\"]/tbody/tr");
            int level = -1;

            IEnumerable<TalentWinrate> results = data.Then(d =>
            {
                UnformattedData isSpan = d.Then("td[2]/span");

                if (isSpan.Nodes != null)
                {
                    ++level;
                    return (null);
                }

                string name = d.Then("td[4]").ExtractContent('<');
                float.TryParse(d.Then("td[8]").ExtractContent('%'), out float winRate);

                return (new TalentWinrate()
                {
                    Level = level,
                    Talent = new Talent
                    {
                        Name = name
                    },
                    WinRate = winRate
                });
            }).Where(i => i != null);

            return (results);
        }
    }*/
}

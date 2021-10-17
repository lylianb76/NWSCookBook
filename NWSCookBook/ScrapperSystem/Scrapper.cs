using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NWSCookBook.Models;

namespace NWSCookBook.ScrapperSystem
{
    public class Scrapper
    {
        public static string BaseURL = "https://github.com/NWS-A1/NWSCookBook/tree/master/Recettes";
        private string HTML;

        public IEnumerable<string> GetRecipesLinks()
        {
            HttpClient client = new HttpClient();

            HTML = client.GetStringAsync(BaseURL).Result;

            UnformattedData data = UnformattedData.Create(HTML, "//a[contains(@class, 'js-navigation-open') and contains(@class, 'Link--primary')]");
            
            return (data.Then(i => $"https://raw.githubusercontent.com{i.ExtractAttribute("href").Replace("blob/", "")}"));
        }

        public IEnumerable<Recette> GenerateRecipes(IEnumerable<string> links)
        {
            Task<Recette>[] results = links.Select(i => Task.Run(() => ExtractRecipe(i))).ToArray();

            Task.WaitAll(results);
            return (results
                .Select(i => i.Result)
                .Where(i => i != null));
        }

        public async Task<Recette> ExtractRecipe(string link)
        {
            try
            {
                int count = 0;
                string str;
                string empty;
                char[] temp;
                HttpClient client = new HttpClient();
                Recette recette = new Recette();

                string md = await client.GetStringAsync(link);
                string[] lines = md
                    .Split(new[] { '\n', '\r' })
                    .Select(i => i.Trim())
                    .Where(i => !string.IsNullOrWhiteSpace(i))
                    .ToArray();

                if (!Extract(i => recette.Name = i, "# ", lines, ref count))
                    return (null);

                if (!Extract(i => recette.Author = i, "## [AUTHOR] ", lines, ref count))
                    return (null);
                
                if (!lines[count].StartsWith("!["))
                    return (null);

                str = lines[count].Replace("![", "");

                recette.AltText = new string(str.TakeWhile(c => c != ']').ToArray());

                str = str.Replace($"{recette.AltText}]", "");

                temp = str.TakeWhile(i => i != '(').ToArray();

                if (temp.Length != 0)
                {
                    empty = new string(temp);
                    str = str.Replace(empty, "");
                }
                
                empty = new string(str.TakeWhile(i => i != '\"').ToArray());
                recette.ImageURL = empty.Substring(1).Trim();

                str = str.Replace(empty, "").Substring(1);

                recette.AltText = new string(str.TakeWhile(i => i != '"').ToArray());

                while (!lines[count].StartsWith("## [INGREDIENTS]"))
                {
                    ++count;
                }
                ++count;

                while (!lines[count].StartsWith("## [PREPARATION]"))
                {
                    recette.Ingredients.Add(lines[count].Replace("* ", ""));
                    ++count;
                }
                ++count;

                if (recette.Ingredients.Count == 0)
                    return (null);

                int count2 = 0;
                while (lines.Length > count)
                {
                    recette.Preparation.Add(lines[count].Replace($"{count2}. ", ""));
                    ++count;
                    ++count2;
                }

                recette.URL = CreateMD5($"{recette.Name} {recette.Author}");

                return (recette);
            }
            catch (Exception e)
            {
                return (null);
            }
        }

        public bool Extract(Action<string> fct, string token, string[] lines, ref int count)
        {
            if (!lines[count].StartsWith(token))
                return (false);

            fct(lines[count].Replace(token, ""));
            ++count;
            return (true);
        }

        private static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
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
        }*/
    }
}

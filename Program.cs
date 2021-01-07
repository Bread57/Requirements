using System;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using AngleSharp.Dom;
using Requirements.Models;

namespace Requirements
{
    class Program
    {
        static void Main()//many to many with genres-games()
        {
            AddLevels().Wait();
            AddGenres().Wait();
            GetLinks().Wait();

            //GamesContext db = new GamesContext();
            //foreach (var i in db.Games)
            //{
            //    AddProduct(i.Id).Wait();
            //}
            // GameInfoParsing("DARK SOULS 2: SCHOLAR OF THE FIRST SIN").Wait();
            //Console.WriteLine(string.Join('\n', AddRandomKeys(16, 5).Select(x=> x.KeyNumber)));
            //Console.WriteLine(GetPriceFromGabeStore("Fortnite").Result);
            //GameInfoParsing("DOOM Eternal").Wait();
            //DownLoadGameImage("https://gepig.com/game_cover_460w/5613.jpg", 1).Wait();
            //GetGenre("Genre: Action, FPS");
            //Console.WriteLine(GetCorrectLink("DOOM eternal") == "https://gamesystemrequirements.com/game/doom-eternal");
        }
        static async Task AddLevels()
        {
            GamesContext db = new GamesContext();
            (string Name, decimal Threshold, decimal Discount)[] names = { ("Beginner", 0M, 0M), ("Bronze", 1000M, 0.01M), ("Silver", 2500M, 0.03M), ("Gold", 10000M, 0.05M), ("Platinum", 20000M, 0.1M) };
            List<UserLevel> levels = new List<UserLevel>();
            foreach (var i in names)
            {
                UserLevel level = new UserLevel() { Name = i.Name, PriceThreshold = i.Threshold, Discont = i.Discount };
                levels.Add(level);
            }
            await db.Levels.AddRangeAsync(levels);
            await db.SaveChangesAsync();

        }
        static async Task AddGenres()
        {
            //    new Dictionary<string, int>()
            //{
            //    {"Action",false },
            //    {"FPS",false },
            //    { "Adventure",false},
            //    { "Role-playing game",false},
            //    { "Sports",false},
            //    { "Racing",false},
            //    { "Strategy",false},
            //    {"Simulation",false },
            //    {"Survival horror",false },
            //    {"Battle Royale",false },
            //    {"Survival",false }

            //};
            GamesContext db = new GamesContext();
            string[] genres_name = new string[] { "Action", "FPS", "Adventure", "Role-playing game", "Sports", "Racing", "Strategy", "Simulation", "Survival horror", "Battle Royale", "Survival" };
            foreach (string i in genres_name)
            {
                await db.Genres.AddAsync(new Genre() { Name = i });
            }
            await db.SaveChangesAsync();
        }
        private static async Task AddProduct(int game_id)
        {
            Product product = new Product(game_id, AddRandomKeys(8, 100));
            ProductContext db = new ProductContext();
            await db.Products.AddAsync(product);
            await db.SaveChangesAsync();
        }
        private static List<Key> AddRandomKeys(int length, int count)
        {
            char[] array = new char[]
            {
                'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
                'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                '1','2','3','4','5','6','7','8','9','0'
            };
            Random random = new Random();
            StringBuilder builder = new StringBuilder();
            List<Key> keys = new List<Key>();
            for (int i = 0; i < count; i++)
            {
                for (int k = 0; k < length; k++)
                {
                    builder.Append(array[random.Next(0, 62)]);
                }
                keys.Add(new Key() { KeyNumber = builder.ToString() });
                builder.Clear();
            }
            return keys;
        }
        private static async Task<decimal> GetPriceFromGabeStore(string game_name)
        {
            var configuration = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(configuration);
            var document = await context.OpenAsync($"https://gabestore.ru/game/{GetLinkGameName(game_name)}");
            decimal price = 0;
            foreach (var i in document.All)
            {
                if (i.ClassName == "b-card__price-currentprice")
                {
                    if (!decimal.TryParse(i.TextContent.TrimEnd(' ', '₽'), out price))
                    {
                        return 0;
                    }
                }
            }
            return price;
        }
        private static async Task GetLinks()
        {
            var configuration = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(configuration);
            var document = await context.OpenAsync("https://gamesystemrequirements.com/games");
            foreach (var i in document.All)
            {
                if (i.ClassName == "games_main_box" && i.LocalName == "a")
                {
                    Debug.WriteLine(i.Attributes["title"]?.Value);
                    await GameInfoParsing(i.Attributes["title"]?.Value);
                }
            }
        }
        private static async Task DownLoadGameImage(string link, int game_id)
        {
            WebRequest request = WebRequest.Create(link);
            WebResponse response = await request.GetResponseAsync();

            byte[] data;
            using (Stream stream = response.GetResponseStream())
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    data = reader.ReadBytes(5000000);
                }
            }
            Console.WriteLine(data.Length);
            using (FileStream file = new FileStream($@"E:\GameStoreImages\{game_id}.jpg", FileMode.OpenOrCreate))
            {
                using (BinaryWriter writer = new BinaryWriter(file))
                {
                    writer.Write(data);
                }
            }
            Console.WriteLine("File was saved");
        }
        private static string GetLinkGameName(string game_name)
        {
            StringBuilder correct_link = new StringBuilder();
            foreach (char i in game_name)
            {
                if (char.IsLetterOrDigit(i))
                {
                    correct_link.Append(char.ToLower(i));
                }
                else if (char.IsWhiteSpace(i) || i == '-')
                {
                    correct_link.Append('-');
                }
                else if (i == '&')
                {
                    correct_link.Append("and");
                }
            }
            return correct_link.ToString();
        }
        private static string GetCorrectLink(string game_name)
        {
            return $"https://gamesystemrequirements.com/game/{GetLinkGameName(game_name)}";
        }
        private static async Task GameInfoParsing(string game_name)
        {

            GamesContext db = new GamesContext();
            if (await db.Games.FirstOrDefaultAsync(x => x.Name == game_name) != null)
            {
                return;
            }
            decimal price = await GetPriceFromGabeStore(game_name);
            if (price < 1)
            {
                return;
            }
            string link = GetCorrectLink(game_name);
            var configuration = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(configuration);
            var document = await context.OpenAsync(link);

            StringBuilder requirements = new StringBuilder();
            string name = "";
            string publisher_name = "";
            string developer_name = "";
            DateTime release_date = new DateTime();
            string image_link = "";
            int count = 0;
            bool need_parse = true;
            Game game = new Game();
            List<int> genres_id = new List<int>();

            foreach (var i in document.All)
            {
                if (i.TextContent == "Error 404")
                {
                    throw new FileNotFoundException();
                }
                if (i.ClassName == "game_head_details_row")
                {
                    if (i.TextContent.StartsWith("Pub"))
                    {
                        publisher_name = GetContent(i.TextContent);
                    }
                    else if (i.TextContent.StartsWith("Dev"))
                    {
                        developer_name = GetContent(i.TextContent);
                    }
                    else if (i.TextContent.StartsWith("Rel"))
                    {
                        release_date = GetDate(i.TextContent);
                    }
                    else if (i.TextContent.StartsWith("Gen"))
                    {
                        genres_id = await GetGenre(i.TextContent);
                    }
                }
                else if (i.ClassName == "tbl")
                {
                    if (i.TextContent.StartsWith("Note"))
                    {
                        break;
                    }
                    if (need_parse)
                    {
                        requirements.Append(i.TextContent + "\n");
                        count++;
                        if (count == 2)
                        {
                            need_parse = false;
                            count = 0;
                        }
                    }
                    else
                    {
                        count++;
                        if (count == 2)
                        {
                            need_parse = true;
                            count = 0;
                        }
                    }
                }
                else if (i.ClassName == "game_head_cover" && i.FirstElementChild.LocalName == "img")
                {
                    image_link = i.FirstElementChild.Attributes["src"].Value;
                }
                else if (i.ClassName == "game_head_title")
                {
                    name = i.TextContent;
                }
            }

            int? developer = db.Developers.AsNoTracking().FirstOrDefault(n => n.Name == developer_name)?.Id;
            int? publisher = db.Publishers.AsNoTracking().FirstOrDefault(n => n.Name == publisher_name)?.Id;

            game.Price = price;
            game.Name = game_name;
            game.ReleaseDate = release_date;
            game.Requirements = requirements.ToString();

            if (developer == null || publisher == null)
            {
                Developer new_developer = null;
                Publisher new_publisher = null;
                if (developer == null)
                {
                    new_developer = new Developer(developer_name);
                    db.Developers.Add(new_developer);
                }
                if (publisher == null)
                {
                    new_publisher = new Publisher(publisher_name);
                    db.Publishers.Add(new_publisher);
                }
                await db.SaveChangesAsync();
                if (developer == null)
                {
                    developer = new_developer.Id;
                }
                if (publisher == null)
                {
                    publisher = new_publisher.Id;
                }
            }

            game.DeveloperId = (int)developer;
            game.PublisherId = (int)publisher;

            db.Games.Add(game);
            await db.SaveChangesAsync();

            foreach (var i in genres_id)
            {
                game.Genres.Add(new GameGenre { GameId = game.Id, GenreId = i });
            }

            db.Games.Update(game);
            await db.SaveChangesAsync();
            Console.WriteLine(game.Id);

            await AddProduct(game.Id);

            await DownLoadGameImage(image_link, game.Id);

            Debug.WriteLine(game_name);
        }
        static async Task<List<int>> GetGenre(string text)
        {
            GamesContext db = new GamesContext();
            Dictionary<string, int> possible_genres = await db.Genres.AsNoTracking().ToDictionaryAsync(key => key.Name, value => value.Id);
            //    new Dictionary<string, int>()
            //{
            //    {"Action",false },
            //    {"FPS",false },
            //    { "Adventure",false},
            //    { "Role-playing game",false},
            //    { "Sports",false},
            //    { "Racing",false},
            //    { "Strategy",false},
            //    {"Simulation",false },
            //    {"Survival horror",false },
            //    {"Battle Royale",false },
            //    {"Survival",false }

            //};

            bool was_colon = false;
            StringBuilder genre = new StringBuilder();
            List<int> genres_id = new List<int>();
            foreach (char i in text)
            {
                if (was_colon)
                {
                    if (char.IsLetterOrDigit(i) || i == '-')
                    {
                        genre.Append(i);
                    }
                    else if (char.IsWhiteSpace(i))
                    {
                        if (genre.Length != 0)
                        {
                            genre.Append(i);
                        }
                    }
                    else if (i == ',')
                    {
                        if (genre.Length > 0 && !genre.Equals(' '))
                        {
                            if (possible_genres.ContainsKey(genre.ToString()))
                            {
                                genres_id.Add(possible_genres[genre.ToString()]);
                            }
                            genre.Clear();
                        }
                    }
                }
                if (i == ':')
                {
                    was_colon = true;
                }
            }
            if (genre.Length > 0)
            {
                if (possible_genres.ContainsKey(genre.ToString()))
                {
                    genres_id.Add(possible_genres[genre.ToString()]);
                }
            }

            return genres_id;
        }
        static DateTime GetDate(string text)
        {
            bool was_colon = false;
            StringBuilder date = new StringBuilder();
            foreach (char i in text)
            {
                if (was_colon)
                {
                    if (char.IsLetterOrDigit(i))
                    {
                        date.Append(i);
                    }
                    else if (char.IsWhiteSpace(i))
                    {
                        if (date.Length > 0)
                        {
                            date.Append(i);
                        }
                    }
                }
                if (i == ':')
                {
                    was_colon = true;
                }
                if (i == '(')
                {
                    break;
                }
            }
            return Convert.ToDateTime(date.ToString());
        }
        private static string GetContent(string text)
        {
            StringBuilder content = new StringBuilder();
            bool was_whitespace = false;
            for (int i = 0; i < text.Length && text[i] != '©' && text[i] != '(' && text[i] != ','; i++)
            {
                if (char.IsWhiteSpace(text[i]) && was_whitespace == false)
                {
                    was_whitespace = true;
                    continue;
                }
                if (was_whitespace)
                {
                    content.Append(text[i]);
                }
            }
            return content.ToString();
        }
    }
}

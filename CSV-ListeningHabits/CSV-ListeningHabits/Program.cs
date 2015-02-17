using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_ListeningHabits
{
    class Program
    {
        // Global List
        public static List<Play> musicDataList = new List<Play>();
        static void Main(string[] args)
        {
            // initalize dataset into list
            InitList();
            string text = musicDataList[18964].Time.ToString();
            bool test = text.Contains("2014");
            Console.WriteLine(text);
            List<string> countries = new List<string> { "USA", "CANADA", "AUSTRALIA", "ENGLAND", "CHINA", "INDIA", "RUSSIA", "FRANCE", "USA", "USA", "CANADA" };
            IEnumerable<IGrouping<int, string>> groups = countries.GroupBy(x => x.Length);

            Console.WriteLine(TotalPlaysByArtistNameInYear("Skrillex", "2014"));

            Console.WriteLine(TotalPlaysByArtistName("Skrillex"));

            


            // keep console open
            Console.ReadLine();
        }
        /// <summary>
        /// A function to initalize the List from the csv file
        /// needed for testing
        /// </summary>
        public static void InitList()
        {
            // load data
            using (StreamReader reader = new StreamReader("scrobbledata.csv"))
            {
                // Get and don't use the first line
                string firstline = reader.ReadLine();
                // Loop through the rest of the lines
                while (!reader.EndOfStream)
                {
                    musicDataList.Add(new Play(reader.ReadLine()));
                }
            }
        }
        /// <summary>
        /// A function that will return the total ammount of plays in the dataset
        /// </summary>
        /// <returns>total number of plays</returns>
        public static int TotalPlays()
        {
            return musicDataList.Count;
        }
        /// <summary>
        /// A function that returns the number of plays ever by an artist
        /// </summary>
        /// <param name="artistName">artist name</param>
        /// <returns>total number of plays</returns>
        public static int TotalPlaysByArtistName(string artistName)
        {
            //WRONG because of Contains. it might happen like Skrillex feat smth will be true too
            //return musicDataList.Count(x => x.Artist.ToLower().Contains(artistName.ToLower()));
            return musicDataList.Count(x => x.Artist.ToLower() == artistName.ToLower());
        }
        /// <summary>
        /// A function that returns the number of plays by a specific artist in a specific year
        /// </summary>
        /// <param name="artistName">artist name</param>
        /// <param name="year">one year</param>
        /// <returns>total plays in year</returns>
        public static int TotalPlaysByArtistNameInYear(string artistName, string year)
        {
            //return musicDataList.Count(x => x.Time.Year.ToString() == year && x.Artist.ToLower() == artistName.ToLower());
            return musicDataList.Count(x => x.Artist.ToLower() == artistName.ToLower() && x.Time.ToString().Contains(year));

            //return musicDataList.Where(x => x.Artist.ToLower() == artistName.ToLower()).Where(x => x.Time.Year == int.Parse(year)).Count();
            //my method
            //return musicDataList.Where(x => x.Time.ToString().Contains(year)).Where(y => y.Artist == artistName).Count();
        }
        /// <summary>
        /// A function that returns the number of unique artists in the entire dataset
        /// </summary>
        /// <returns>number of unique artists</returns>
        public static int CountUniqueArtists()
        {
            //return musicDataList.Select(x => x.Artist).Distinct().ToList().Count;
            return musicDataList.Select(x => x.Artist).Distinct().Count();

        }
        /// <summary>
        /// A function that returns the number of unique artists in a given year
        /// </summary>
        /// <param name="year">year to check</param>
        /// <returns>unique artists in year</returns>
        public static int CountUniqueArtists(string year)
        {
            //return musicDataList.Where(x => x.Time.Year.Equals(int.Parse(year))).Select(y => y.Artist).Distinct().ToList().Count;
            return musicDataList.Where(x => x.Time.Year == int.Parse(year)).Select(y => y.Artist).Distinct().Count();

        }
        /// <summary>
        /// A function that returns a List of unique strings which contains
        /// the Title of each track by a specific artists
        /// </summary>
        /// <param name="artistName">artist</param>
        /// <returns>list of song titles</returns>
        public static List<string> TrackListByArtist(string artistName)
        {
            return musicDataList.Where(x => x.Artist == artistName).Select(x => x.Title).Distinct().ToList();
        }
        /// <summary>
        /// A function that returns the first time an artist was ever played
        /// </summary>
        /// <param name="artistName">artist name</param>
        /// <returns>DateTime of first play</returns>
        public static DateTime FirstPlayByArtist(string artistName)
        {
            return musicDataList.Where(x => x.Artist.ToLower().Equals(artistName.ToLower())).Select(x => x.Time).OrderBy(x => x).First();
        }
        /// <summary>
        ///                     ***BONUS***
        /// A function that will determine the most played artist in a specified year
        /// </summary>
        /// <param name="year">year to check</param>
        /// <returns>most popular artist in year</returns>
        public static string MostPopularArtistByYear(string year)
        {
            //IEnumerable<IGrouping<string, int>> groups = musicDataList.Where(x => x.Time.Year == int.Parse(year)).GroupBy(x => x.Artist);
            //return musicDataList.Where(x => x.Time.Year == int.Parse(year)).GroupBy(x => x.Artist).OrderByDescending(x => x.Count()).First().First().Artist; ?? [0].[1]
            return musicDataList.Where(x => x.Time.Year == int.Parse(year)).GroupBy(x => x.Artist).OrderByDescending(x => x.Count()).First().Key;

        }
    }

    public class Play
    {
        // Properties
        public DateTime Time { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public Play(string lineInput)
        {
            // Split using the tab character due to the tab delimited data format
            string[] playData = lineInput.Split('\t');
            
            // Get the time in milliseconds and convert to C# DateTime
            DateTime posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
            this.Time = posixTime.AddMilliseconds(long.Parse(playData[0]));

            // need to populate the rest of the properties
            this.Artist = playData[1];
            this.Title = playData[2];
            this.Album = playData[3];
        }
    }
}

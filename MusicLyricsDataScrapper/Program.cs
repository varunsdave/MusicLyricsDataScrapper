using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HtmlAgilityPack;


namespace MusicLyricsDataScrapper
{
    class Program
    {
        static string baseUrl = "http://smriti.com";
        static void Main(string[] args)
        {

            
            string Url = "http://smriti.com/hindi-songs/movies-o";


            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(Url);

            smritiScrapper(Url);

        }


        private static void smritiScrapper(string Url)
        {

            HtmlWeb web = new HtmlWeb();

            HtmlDocument doc = web.Load(Url);
            var movieTitlesList = doc.DocumentNode.SelectNodes("//div[@class=\"latest\"]").Descendants("a");
            // mine movies list
            foreach (var movieLink in movieTitlesList)
            {
                string linkString = movieLink.InnerText;
                if (!(movieLink.InnerText.Contains("Browse") || movieLink.InnerText.Contains("main index") || movieLink.InnerText.Contains("Non-Film") || movieLink.InnerText.Contains("Pakistani")))
                {
                    int a = 1;
                    // get song list from a movie
                    string movieUrl = baseUrl + movieLink.GetAttributeValue("href", null);
                    Console.WriteLine(movieUrl);
                    mineMovieSongs(movieUrl);

                }


            }


        }

        private static void mineMovieSongs(string movieUrl)
        {

            HtmlWeb web = new HtmlWeb();

            HtmlDocument doc = web.Load(movieUrl);
            var songTitleList = doc.DocumentNode.SelectNodes("//div[@class=\"onesong\"]").Descendants("a");
            foreach (var song in songTitleList)
            {
                string songLinkTxt = song.InnerText;
                if (songLinkTxt.ToLower().Equals("text"))
                {
                    string songLink = baseUrl + song.GetAttributeValue("href", null);
                    saveSong(songLink);
                }
            }

        }

        private static void saveSong(string songLink)
        {
            HtmlWeb web = new HtmlWeb();

            HtmlDocument doc = web.Load(songLink);
            var songTitleList = doc.DocumentNode.SelectNodes("//div[@class=\"songbody\"]/p")[0].InnerText;
            //Console.WriteLine(songLink);
            string songTitle = doc.DocumentNode.SelectNodes("//div[@class=\"latest\"]/h1")[0].InnerText;
            string songLyrics = songTitleList;
            if (songLyrics.Length > 0 && songTitle.Length > 0)
            {
                string replacedTitle = "";
                Song s;
                if (songTitle.Contains("-"))
                {
                    replacedTitle = songTitle.Replace("\\-", "_");
                    replacedTitle = replacedTitle.Replace('#', '_');
                    s = new Song(replacedTitle, songLyrics);
                }
                else
                {
                    s = new Song(songTitle, songLyrics);
                }

                writeSongToFile(s);
            }
        }

        private static void writeSongToFile(Song s)
        {

            try
            {
                using (StreamWriter outfile = new StreamWriter(@"D:\Development\MusicLyricsDataScrapper\output_lyrics\" + s.getLetter() + "_" + s.getTitle()))
                {
                    outfile.Write(s.getLyrics());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("found exception with : " + s.getLetter() + "+" + s.getTitle());
            }

            
        }
    }
}

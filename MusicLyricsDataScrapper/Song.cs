using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLyricsDataScrapper
{
    class Song
    {
        private string startLetter;
        private string songTitle;
        private string songLyrics;
        public Song(string title, string lyrics)
        {
            songTitle = title;
            songLyrics = lyrics;
            var t = lyrics.ToCharArray();
            startLetter = t[0].ToString();
        }
        public string getTitle()
        {
            return songTitle;
        }
        public string getLyrics()
        {
            return songLyrics;
        }
        public string getLetter()
        {
            return startLetter;
        }
    }
}

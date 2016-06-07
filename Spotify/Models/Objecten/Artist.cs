using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spotify.Models.Objecten
{
    public class Artist
    {
        public int ID { get; set; }
        public string  Name { get; set; }
        public List<Song> Songs { get; set; }
        public List<Album> Albums { get; set; }
    }
}
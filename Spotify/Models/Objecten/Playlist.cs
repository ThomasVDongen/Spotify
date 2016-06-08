using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spotify.Models.Objecten
{
    public class Playlist
    {
        public int ID { get; set; }
        public List<Song> Songs { get; set; }
        public string Name { get; set; }

    }
}
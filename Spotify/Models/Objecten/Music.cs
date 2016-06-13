using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spotify.Models.Objecten
{
    public class Music
    {
        public int ID { get; set; }
        public List<Album> Albums { get; set; }
        public List<Song> Songs { get; set; }
        public List<Artist> Artists { get; set; }
        public List<Playlist> Playlists { get; set; }

    }
}
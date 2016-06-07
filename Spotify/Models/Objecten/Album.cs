using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spotify.Models.Objecten
{
    public class Album
    {
        public int ID { get; set; }
        public string Titel { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Artist Artist { get; set; }
        public List<Song> Songs { get; set; }
        
    }
}

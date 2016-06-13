using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spotify.Models.Objecten;

namespace Spotify.Models
{
    public class ZoekenModel
    {
        public List<Song> Songs { get; set; }
        public List<Album> Albums { get; set; } 
        public List<Artist> Artists { get; set; } 
    }
}
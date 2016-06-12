using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security.Facebook;

namespace Spotify.Models.Objecten
{
    public class Song
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Speelduur { get; set; }
        public DateTime Releasedate { get; set; }
        public List<Artist> Artists { get; set; }
        public List<Genre> Genres { get; set; } 

    }
}
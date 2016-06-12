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
        [Display(Name = "Naam:")]
        public string Name { get; set; }
        [Display(Name = "Speelduur:")]
        public double Speelduur { get; set; }
        [Display(Name = "Releasedate:")]
        public DateTime Releasedate { get; set; }
        [Display(Name = "Artiesten:")]
        public List<Artist> Artists { get; set; }
        [Display(Name = "Genres:")]
        public List<Genre> Genres { get; set; } 

    }
}
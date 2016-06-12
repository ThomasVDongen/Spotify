using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spotify.Models.Objecten
{
    public class Genre
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Requirements.Models
{
    public class GameGenre
    {
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}

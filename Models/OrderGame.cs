using System;
using System.Collections.Generic;
using System.Text;

namespace Requirements.Models
{
    public class OrderGame
    {
        public int GameCount { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}

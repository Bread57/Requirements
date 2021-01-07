using System;
using System.Collections.Generic;
using System.Text;

namespace Requirements.Models
{
    public class Product
    {
        public Product()
        {
            Keys = new List<Key>();
        }
        public Product(int game_id, List<Key> keys)
        {
            Keys = keys;
            GameId = game_id;
        }
        public int Id { get; set; }
        public int GameId { get; set; }
        public List<Key> Keys { get; set; }
    }
}

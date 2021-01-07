using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Requirements.Models
{
    public class Game
    {
        public Game()
        {
            Genres = new List<GameGenre>();
            Orders = new List<OrderGame>();
        }
        public Game(string name, Publisher publisher, Developer developer)
        {
            Genres = new List<GameGenre>();
            Orders = new List<OrderGame>();
            Name = name;
            PublisherId = publisher.Id;
            Publisher = publisher;
            DeveloperId = developer.Id;
            Developer = developer;
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int PublisherId { get; set; }
        [Required]
        public Publisher Publisher { get; set; }
        public int DeveloperId { get; set; }
        [Required]
        public Developer Developer { get; set; }
        
        public decimal Price { get; set; }
        public string Requirements { get; set; }
        public List<GameGenre> Genres { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<OrderGame> Orders { get; set; }
    }
}

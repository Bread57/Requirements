using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Requirements.Models
{
    public class Order
    {
        public Order()
        {
            Games = new List<OrderGame>();
        }
        public int Id { get; set; }
        public DateTime OrderTime { get; set; }
        public int GamesCount { get; set; }
        [Required]
        public decimal TotalSum { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
        [Required]
        public string OrderNumber { get; set; }
        public List<OrderGame> Games { get; set; }
    }
}

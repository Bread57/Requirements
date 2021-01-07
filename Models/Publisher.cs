using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Requirements.Models
{
    public class Publisher
    {
        public Publisher(string name)
        {
            Name = name;
        }
        public Publisher() { }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}

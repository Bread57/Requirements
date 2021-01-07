using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Requirements.Models
{
    public class Developer
    {
        public Developer(string name)
        {
            Name = name;
        }
        public Developer() { }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}

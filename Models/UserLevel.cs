using System;
using System.Collections.Generic;
using System.Text;

namespace Requirements.Models
{
    public class UserLevel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PriceThreshold { get; set; }
        public decimal Discont { get; set; }
    }
}

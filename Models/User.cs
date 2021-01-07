using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics;

namespace Requirements.Models
{
    public class User
    {
        public User()
        {
            Orders = new List<Order>();
            TotalBuyingsSum = 0;
            UserLevelId = 1;
        }
        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = EncodePassword(password);
            Orders = new List<Order>();
            TotalBuyingsSum = 0;
            UserLevelId = 1;
        }
        public User(string name, string email, string password, List<Order> orders)
        {
            Name = name;
            Email = email;
            Password = EncodePassword(password);
            Orders = orders;
            TotalBuyingsSum = 0;
            UserLevelId = 1;
        }
        public int Id { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [StringLength(27, ErrorMessage = "Name's length must be larger or equal than 4 and shorter than or equal than 27 characters", MinimumLength = 4)]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(400, ErrorMessage = "Password's length must be larger or equal than 8 and shorter than or equal than 16 characters", MinimumLength = 8)]
        public string Password { get; set; }
        [Required]
        public int UserLevelId { get; set; }
        [Required]
        public decimal TotalBuyingsSum { get; set; }
        [Required]
        public UserLevel UserLevel { get; set; }
        public List<Order> Orders { get; set; }

        private string EncodePassword(string password)
        {
            SHA256 hash = SHA256.Create();
            string encoded_password = BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "");
            return encoded_password;
        }
        public bool IsCorrectPassword(string password)
        {
            password = EncodePassword(password);
            if (Password == password)
            {
                return true;
            }
            return false;
        }

    }
}

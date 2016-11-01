using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserSearch.Model
{
    [Table("Users")]
    public class User
    {
        [Key]
        public String UserId { get; set; }
        public String Fullname { get; set; }
        public int Age { get; set; }
        public String Address { get; set; }
        public String Interests { get; set; }
        public byte[] Photo { get; set; }
    }
}

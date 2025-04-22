using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string Login {  get; set; }
        [Required]
        public string Password { get; set; }
        public List<Group> Subscriptions { get; set; } // подписка на группы
        public List<Group> GroupsToPost { get; set; }
        public List<Post> Posts { get; set; }
        
    }
}

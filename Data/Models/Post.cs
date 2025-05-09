using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text {  get; set; }
        public DateTime Created {  get; set; }
        public DateTime? Updated { get; set; }
        [JsonIgnore]
        public int AuthorId { get; set; }
        public User Author { get; set; }
        public List<Comment> Comments { get; }
    }
}

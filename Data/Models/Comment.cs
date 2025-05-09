using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        [JsonIgnore]
        public int PostId { get; set; }
        public Post Post { get; set; }
        [JsonIgnore]
        public int CommentAuthorId { get; set; }    
        public User CommentAuthor { get; set; }
    }
}

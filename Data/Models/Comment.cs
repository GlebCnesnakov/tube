using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required, MaxLength(300)]
        public string Text { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int CommentAuthorId { get; set; }    
        public User CommentAuthor { get; set; }
    }
}

using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Validation.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Post { get; set; }
        public string CommentAuthor { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}

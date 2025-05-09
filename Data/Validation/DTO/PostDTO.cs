using Data.Models;

namespace Data.Validation.DTO;


public class PostDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Text {  get; set; }
    public DateTime Created {  get; set; }
    public DateTime? Updated { get; set; }
    public string Author { get; set; }
    public List<Comment> Comments { get; set; }
}
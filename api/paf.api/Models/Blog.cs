namespace paf.api.Models;

public class Blog:BaseEntity
{
    public User User { get; set; } = default!;
    public string Heading { get; set; } = default!;
    public string Content { get; set; } = default!;
    public DateTime Date { get; set; } = DateTime.Now;
    //public List<string> Labels { get; set; } = default!;
    public List<Comment> Comments { get; set; }= default!;
    public string Category { get; set; } = default!;
    //public string Image { get; set; }=default!;
}

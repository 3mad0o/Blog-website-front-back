namespace paf.api.Models;

public class Comment:BaseEntity
{
    public User User { get; set; } = default!;
    public string TheComment { get; set; } = default!;
    public DateTime Date { get; set; } = DateTime.Now;

}
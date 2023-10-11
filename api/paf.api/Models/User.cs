namespace paf.api.Models;

public class User:BaseEntity
{
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public int Age { get; set; } = default!;
    public string Gender { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
    public List<Blog> UserBlogs { get; set; } = default!;
}

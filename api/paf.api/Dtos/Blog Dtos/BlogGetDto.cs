using paf.api.Dtos.User_Dtos;
using paf.api.Models;

namespace paf.api.Dtos.Blog_Dtos
{
    public record BlogGetDto(int Id,UserGetDto User,string Heading,string Content, DateTime Date,List<Comment> Comments,string Category);
}

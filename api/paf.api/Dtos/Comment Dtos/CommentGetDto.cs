using paf.api.Dtos.User_Dtos;

namespace paf.api.Dtos.Comment_Dtos
{
    public record CommentGetDto(int Id,UserGetDto User,string TheComment,DateTime Date);
}

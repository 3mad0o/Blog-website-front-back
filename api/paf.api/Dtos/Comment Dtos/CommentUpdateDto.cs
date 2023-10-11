using paf.api.Dtos.User_Dtos;

namespace paf.api.Dtos.Comment_Dtos
{
    public record CommentUpdateDto(int Id, int UserId, string TheComment, DateTime Date);
}

namespace paf.api.Dtos.Comment_Dtos
{
    public record CommentCreateDto(int UserId,int BlogId,string TheComment,DateTime Date);
}

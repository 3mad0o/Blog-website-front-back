using paf.api.Dtos.Comment_Dtos;

namespace paf.api.Interfaces
{
    public interface ICommentService
    {
        Task<int> CreateComment(CommentCreateDto commentCreate);
        Task<int> UpdateComment(CommentUpdateDto commentUpdate);
        Task<int> DeleteComment(int id);
        Task<CommentGetDto> GetComment(int id);
        Task<List<CommentGetDto>> GetBlogComments(int BlogId);
    }
}

using paf.api.Dtos.Blog_Dtos;
using paf.api.Dtos.Comment_Dtos;

namespace paf.api.Interfaces
{
    public interface IBlogService
    {
        Task<int> CreateBlog(BlogCreateDto blogCreate);
        void DeleteBlog(int id);

        Task UpdateBlog(BlogUpdateDto blogUpdate);

        Task<List<BlogGetDto>> GetBlogs();
        Task<List<BlogGetDto>> GetUserBlogs(int UserId);
        Task<BlogGetDto> getBlog(int BlogId);
        Task<string> UploadImage(IFormFile file, int id);
        Task<List<BlogGetDto>> GetByCategory(string Category);
        //Task<int> AddComment(CommentCreateDto commentCreate);
    }
}

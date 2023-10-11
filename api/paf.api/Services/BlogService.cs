using AutoMapper;
using FluentValidation;

using paf.api.Dtos.Blog_Dtos;

using paf.api.Interfaces;
using paf.api.Models;
using paf.api.validation.Blog;
using System.Linq.Expressions;

namespace paf.api.Services
{
    public class BlogService : IBlogService
    {
        private readonly IGenericRepostiory<Blog> blogRepository;
        private readonly IGenericRepostiory<User> userRepository;
        private readonly ICommentService commentService;
        private readonly IMapper mapper;
        private readonly BlogCreateValidator blogCreateValidator;
        private readonly BlogUpdateValidator blogUpdateValidator;
        private readonly IWebHostEnvironment webHostEnvironment;

        public BlogService(IGenericRepostiory<Blog> BlogRepository,
            IGenericRepostiory<User> UserRepository,
            ICommentService commentService,
            IMapper mapper,
            BlogCreateValidator blogCreateValidator,
            BlogUpdateValidator blogUpdateValidator,
            IWebHostEnvironment webHostEnvironment)
        {
            blogRepository = BlogRepository;
            userRepository = UserRepository;
            this.commentService = commentService;
            this.mapper = mapper;
            this.blogCreateValidator = blogCreateValidator;
            this.blogUpdateValidator = blogUpdateValidator;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<BlogGetDto>> GetByCategory(string Category)
        {
            
            var entities = await blogRepository.GetFilteredAsync(new Expression<Func<Blog, bool>>[] { e => e.Category == Category },null,null,e=>e.User);
            var entitiesModified = mapper.Map<List<BlogGetDto>>(entities);
            return entitiesModified;
        }

        /*public async Task<int> AddComment(CommentCreateDto commentCreate)
         {

         }*/

        public async Task<int> CreateBlog(BlogCreateDto blogCreate)
        {
            await blogCreateValidator.ValidateAndThrowAsync(blogCreate);
            var entity= mapper.Map<Blog>(blogCreate);
            entity.User = await userRepository.GetByIdAsync(blogCreate.UserId);
            var id =await blogRepository.InsertAsync(entity);
            await blogRepository.saveChangesAsync();
            return id;
        }

        public async void DeleteBlog(int id)
        {
            var entity =await blogRepository.GetByIdAsync(id,b=>b.Comments);
            
            if(entity == null) {
                throw new NotFoundException("something went wrong !it seems that no blog found to be deleted");
            }
            var comments = entity?.Comments.ToList();
            foreach (var comment in comments)
            {
              await commentService.DeleteComment(comment.Id);
            }
            blogRepository.Delete(entity);
            await blogRepository.saveChangesAsync();
        }

        public async Task<BlogGetDto> getBlog(int BlogId)
        {
            var entity =await blogRepository.GetByIdAsync(BlogId,e=>e.User/*,e=>e.Comments*/);
            if (entity == null)
            {
                throw new NotFoundException("something went wrong !it seems that no blog found");
            }
            return mapper.Map<BlogGetDto>(entity);

        }

        public async Task<List<BlogGetDto>> GetBlogs()
        {
            var entities =await blogRepository.GetAsync(null,null,e=>e.User/*,e=>e.Comments*/);
            var entitiesModified=mapper.Map<List<BlogGetDto>>(entities);
           
            return entitiesModified;
        }

        public async Task<List<BlogGetDto>> GetUserBlogs(int UserId)
        {
            var entities = await blogRepository.GetFilteredAsync(new Expression<Func<Blog, bool>>[] { e => e.User.Id == UserId }, null, null,e=>e.User/*,e=>e.Comments*/);   
            var entitiesModified = mapper.Map<List<BlogGetDto>>(entities);
            return entitiesModified;
        }

        public async Task UpdateBlog(BlogUpdateDto blogUpdate)
        {
            await blogUpdateValidator.ValidateAndThrowAsync(blogUpdate);
            var entity2= await blogRepository.GetByIdAsync(blogUpdate.Id);
            var entity =mapper.Map<Blog>(entity2);
            blogRepository.update(entity);
            await blogRepository.saveChangesAsync();

        }

        public async Task<string> UploadImage(IFormFile file,int id)
        {
            var path = webHostEnvironment.WebRootPath + "\\uploads\\";
            var fullPath = path + Guid.NewGuid().ToString() + file.FileName;
            try
            {
                if (file.Length > 0)
                {
                 
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);

                    }
                    var filename = file.FileName;
                    using (FileStream fileStream = File.Create(fullPath))
                    {
                        await file.CopyToAsync(fileStream);
                        fileStream.Flush();
                    }

                }
                var entity = await getBlog(id);
                var entityBlog= mapper.Map<Blog>(entity);
                //entityBlog.Image = fullPath;
                await blogRepository.saveChangesAsync();

                return fullPath;




            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}

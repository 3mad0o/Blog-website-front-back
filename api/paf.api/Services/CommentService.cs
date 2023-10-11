using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using paf.api.Dtos.Comment_Dtos;
using paf.api.Interfaces;
using paf.api.Models;
using paf.api.validation.Comment;

namespace paf.api.Services
{
    public class CommentService : ICommentService
    {
        private readonly IGenericRepostiory<User> userRepository;
        private readonly IGenericRepostiory<Blog> blogRepository;
        private readonly IGenericRepostiory<Comment> commentRepository;
        private readonly IMapper mapper;
        private readonly CommentCreateValidator commentCreateValidator;
        private readonly CommentUpdateValidator commentUpdateValidator;

        public CommentService(IGenericRepostiory<User> UserRepository,
                                IGenericRepostiory<Blog> BlogRepository,
                                IGenericRepostiory<Comment> CommentRepository,
                                IMapper mapper,
                                CommentCreateValidator commentCreateValidator,
                                CommentUpdateValidator commentUpdateValidator)
        {
            userRepository = UserRepository;
            blogRepository = BlogRepository;
            commentRepository = CommentRepository;
            this.mapper = mapper;
            this.commentCreateValidator = commentCreateValidator;
            this.commentUpdateValidator = commentUpdateValidator;
        }


        public async Task<int> CreateComment(CommentCreateDto commentCreate)
        {
            var theUser = await userRepository.GetByIdAsync(commentCreate.UserId);
            var theBlog = await blogRepository.GetByIdAsync(commentCreate.BlogId);
            var theComment = mapper.Map<Comment>(commentCreate);
            theComment.Date = DateTime.Now;
            theComment.User = theUser;
            if (theBlog.Comments == null)
            {
                theBlog.Comments = new List<Comment>();
            }
            theBlog.Comments.Add(theComment);
            await blogRepository.saveChangesAsync();
            await commentRepository.saveChangesAsync();
            return theComment.Id;
        }

        public async Task<int> DeleteComment(int id)
        {
            var entity = await commentRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new NotFoundException("something went wrong! try again later");
            }
              commentRepository.Delete(entity);
            await commentRepository.saveChangesAsync();
            return entity.Id;
        }

        public async Task<CommentGetDto> GetComment(int id)
        {
            var entity = await commentRepository.GetByIdAsync(id,e=>e.User);
            var entityModified =mapper.Map<CommentGetDto>(entity);
            return entityModified;
        }

        public async Task<List<CommentGetDto>> GetBlogComments(int BlogId)
        {
            var theBlog = await blogRepository.GetByIdAsync(BlogId,e=>e.Comments);
            if (theBlog == null)
            {
                throw new NotFoundException("something went wrong! try again later");
            }
            if (theBlog.Comments == null)
            {
                return new List<CommentGetDto>();
            }
            else
            {
                var BlogComments=new List<Comment>();
                foreach (var Comment in theBlog.Comments)
                {
                    var entity= await commentRepository.GetByIdAsync(Comment.Id, e => e.User);
                    BlogComments.Add(entity);
                }
                var Comments = mapper.Map<List<CommentGetDto>>(BlogComments);
                return Comments;
            }
        }

        public  async Task<int> UpdateComment(CommentUpdateDto commentUpdate)
        {
            //await commentUpdateValidator.ValidateAndThrowAsync(commentUpdate);
            var theUser =await userRepository.GetByIdAsync(commentUpdate.UserId); 
            if(theUser == null ) {
                throw new NotFoundException("something went wrong! try again later");
                    }
            var entity = mapper.Map<Comment>(commentUpdate);
            entity.User = theUser;
             commentRepository.update(entity);
            await commentRepository.saveChangesAsync();
            return entity.Id;
        }

      
    }
}

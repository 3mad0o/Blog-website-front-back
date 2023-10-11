using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using paf.api.Dtos.Comment_Dtos;
using paf.api.Interfaces;

namespace paf.api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Authorize]
    public class CommentController:ControllerBase
    {
        private readonly ICommentService commentService;

        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> CreateComment(CommentCreateDto commentCreate)
        {
            var entity= await commentService.CreateComment(commentCreate);
            return Ok(entity);
        }
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateComment(CommentUpdateDto commentUpdate)
        {
            commentService.UpdateComment(commentUpdate);
            return Ok();
        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var Theid= await commentService.DeleteComment(id);
            return Ok(id);
        }
        [HttpGet]
        [Route("Get/{BlogId}")]
        public async Task<IActionResult> GetBlogComments(int BlogId)
        {
            var entities = await commentService.GetBlogComments(BlogId);
            return Ok(entities);
        }

        [HttpGet]
        [Route("getComment/{id}")]
        public async Task<IActionResult> getComment(int id)
        {
            var entity= await commentService.GetComment(id);
            return Ok(entity);
        }
    }
}

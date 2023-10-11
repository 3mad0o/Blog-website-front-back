using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using paf.api.Dtos.Blog_Dtos;
using paf.api.Dtos.Comment_Dtos;
using paf.api.Interfaces;

namespace paf.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
   [Authorize]
    public class BlogController:ControllerBase
    {
        private readonly IBlogService blogService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public BlogController(IBlogService blogService,IWebHostEnvironment webHostEnvironment)
        {
            this.blogService = blogService;
            this.webHostEnvironment = webHostEnvironment;
        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateBlog(BlogCreateDto blogCreate)
        {
            var Id= await blogService.CreateBlog(blogCreate);
            return Ok(Id);
        }
        [HttpPut]
        [Route("Update")]
        public  IActionResult UpdateBlog(BlogUpdateDto blogUpdate)
        {
             blogService.UpdateBlog(blogUpdate);
            return Ok();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetBlogs()
        {
            var entities=await blogService.GetBlogs();
            return Ok(entities);
        }
        [HttpGet]
        [Route("Get/{Id}")]
        public async Task<IActionResult> GetBlog(int Id)
        {
            var entity =await blogService.getBlog(Id);
            return Ok(entity);
        }
        [HttpGet]
        [Route("UserBlogs/{Id}")]
        public async Task<IActionResult> GetUserBlogs(int Id)
        {
            var entities= await blogService.GetUserBlogs(Id);
            return Ok(entities);
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            blogService.DeleteBlog(id);
            return Ok();
        }
        [HttpPost]
        [Route("uploadImage")]
       public async Task<IActionResult> UploadImage(int id,IFormFile file)
        {
          var theFilePath = await blogService.UploadImage(file, id);
            return Ok(theFilePath);
        }
        [HttpGet]
        [Route("getByCategory/{Category}")]
        public async Task<IActionResult> GetBLogByCategory(string Category)
        {
            var entities=await blogService.GetByCategory(Category);
            return Ok(entities);
        }
        /*
       /* [HttpGet]
        [Route("Get/BlogImage/{filename}")]
        public Task<IActionResult> GetBlogImage(string filename)
        {
            var path = webHostEnvironment.WebRootPath + "\\uploads\\";
            var filePath = path+ filename;
            if (System.IO.File.Exists(filePath))
            {
                byte[] b =  System.IO.File.ReadAllBytes(filePath);
                return 
            }
        }*/
        /*[HttpPost]
         [Route("AddComment")]
         public async Task<IActionResult> AddComment(CommentCreateDto commentCreate)
         {
             var Id= await blogService.AddComment(commentCreate);
             return Ok(Id);
         }*/
    }
}

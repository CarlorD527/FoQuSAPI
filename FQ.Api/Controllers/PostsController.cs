using FQ.Application.Dtos.Posts;
using FQ.Application.Interfaces;
using FQ.Application.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FQ.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostsApplication _postApplication;

        public PostsController(IPostsApplication postsApplication)
        {

            _postApplication = postsApplication;
        }

  
        [HttpGet("Get")]
        public async Task<ActionResult> listPosts()
        {

            var response = await _postApplication.GetAllPosts();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPostById(string id)
        {
            var post = await _postApplication.GetByIdPost(id);

            if (post != null)
            {
                return Ok(post);
            }

            return NotFound();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterPost([FromForm] AddPostDto requestDto)

        {
            var response = await _postApplication.addPost(requestDto);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePostById(string id)
        {
            var deleteResponse = await _postApplication.deletePost(id);


            return Ok(deleteResponse);
        }

    }
}

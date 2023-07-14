using FQ.Application.Bases;
using FQ.Application.Dtos.Posts;
using FQ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FQ.Application.Interfaces
{
    public interface IPostsApplication
    {
        Task<BaseResponse<List<Post>>> GetAllPosts();
        Task<BaseResponse<List<Post>>> GetByIdPost(string id);
        Task<BaseResponse<bool>> addPost(AddPostDto postDto);
        Task<BaseResponse<bool>> deletePost(string id);
    }
}

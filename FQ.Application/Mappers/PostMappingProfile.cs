using AutoMapper;
using FQ.Application.Dtos;
using FQ.Application.Dtos.Posts;
using FQ.Domain.Entities;

namespace FQ.Application.Mappers
{
    public class PostMappingProfile : Profile
    {
        public PostMappingProfile()
        {
            CreateMap<Post, AddPostDto>()
                .ReverseMap();
        }
    }
}

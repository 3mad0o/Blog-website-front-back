using AutoMapper;
using paf.api.Dtos.Blog_Dtos;
using paf.api.Dtos.Comment_Dtos;
using paf.api.Dtos.Identity_Dtos;
using paf.api.Dtos.User_Dtos;
using paf.api.Models;

namespace paf.api.Dtos
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<UserCreateDto, User>()
                .ForMember(e => e.Id, opt => opt.Ignore());
            CreateMap<User, UserGetDto>();
            CreateMap<UserUpdateDto,User>();


            CreateMap<BlogCreateDto,Blog>()
                   .ForMember(e => e.Id, opt => opt.Ignore());
            CreateMap<BlogUpdateDto, Blog>();
            CreateMap<Blog, BlogGetDto>();


            CreateMap<CommentCreateDto,Comment>()
                     .ForMember(e => e.Id, opt => opt.Ignore());
            CreateMap<CommentUpdateDto, Comment>();
            CreateMap<Comment, CommentGetDto>();


            CreateMap<UserRegisterDto, UserCreateDto>();
            

        }
    }
}

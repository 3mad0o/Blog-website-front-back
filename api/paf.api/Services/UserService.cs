using AutoMapper;
using FluentValidation;
using paf.api.Dtos.User_Dtos;
using paf.api.Interfaces;
using paf.api.Models;
using paf.api.validation.user;
using System.Linq.Expressions;

namespace paf.api.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepostiory<User> userRepository;
        private readonly IMapper mapper;
        private readonly UserCreateValidatior userCreateValidator;
        private readonly UserUpdateValidator userUpdateValidator;

        public UserService(IGenericRepostiory<User> UserRepository,
                           IMapper mapper,
                           UserCreateValidatior userCreateValidator,
                           UserUpdateValidator userUpdateValidator
                           )
        {
            userRepository = UserRepository;
            this.mapper = mapper;
            this.userCreateValidator = userCreateValidator;
            this.userUpdateValidator = userUpdateValidator;
        }
        public async Task<int> CreateUser(UserCreateDto user)
        {
            await userCreateValidator.ValidateAndThrowAsync( user );
            var entity =mapper.Map<User>(user);
           await userRepository.InsertAsync(entity);
            await userRepository.saveChangesAsync();
            return entity.Id;
        }

        public async void DeleteUser(UserDeleteDto userDeleteDto)
        {
            var entity =await userRepository.GetByIdAsync(userDeleteDto.Id);
            if (entity == null)
            {
                throw new NotFoundException("cant find the user to delete it");
            }
            userRepository.Delete(entity);
            await userRepository.saveChangesAsync();

        }

        public async Task<UserGetDto> GetUser(string Email)
        {
           var entity= await userRepository.GetFilteredAsync(new Expression<Func<User, bool>>[] {e=>e.Email ==Email}, null, null, e => e.UserBlogs);
            if (entity == null)
            {
                throw new NotFoundException("the user has not registerd");
            }
            var theUser = entity.FirstOrDefault();
            var theUserModified = mapper.Map<UserGetDto>(theUser);
            return theUserModified;
        }

        public async Task<UserGetDto> GetUserById(int Id)
        {
            var entity =await userRepository.GetByIdAsync(Id);
            if(entity == null)
            {
                throw new NotFoundException("the user not found");
            }
            var theUserModified = mapper.Map<UserGetDto>(entity);
            return theUserModified;
        }

        public async void UpdateUser(UserUpdateDto user)
        {
            await userUpdateValidator.ValidateAndThrowAsync(user);
            var entity=mapper.Map<User>(user);
            userRepository.update(entity);
            await userRepository.saveChangesAsync();
        }
        public async Task<int> NumberOfUsers()
        {
            var entities = await userRepository.GetAsync(null,null);
            return entities.Count();
        }
    }
}

using paf.api.Dtos.User_Dtos;
using paf.api.Models;

namespace paf.api.Interfaces
{
    public interface IUserService
    {
        Task<int> CreateUser(UserCreateDto user);
        void UpdateUser(UserUpdateDto user);
        void DeleteUser(UserDeleteDto user);
        Task<UserGetDto> GetUser(string Email);
        Task<UserGetDto> GetUserById(int Id);
        Task<int> NumberOfUsers();


    }
}

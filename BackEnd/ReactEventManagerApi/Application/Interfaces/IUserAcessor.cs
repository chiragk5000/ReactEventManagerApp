using Domain;

namespace Application.Interfaces
{
    public interface IUserAcessor
    {
        string GetUserNameClaim();
         Task<User> GetUserAsync();
        Task<User> GetUserWithPhotosAsync();
    }
}

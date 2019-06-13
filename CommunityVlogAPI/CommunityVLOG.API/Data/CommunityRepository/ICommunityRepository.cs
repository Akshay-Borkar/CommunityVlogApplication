using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityVLOG.API.Helpers;
using CommunityVLOG.API.Models;

namespace CommunityVLOG.API.Data.CommunityRepository
{
    public interface ICommunityRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<PageList<User>> GetUsers(UserParams userParams);
         Task<User> GetUser(int id);
         Task<Photo> GetPhoto(int id);
         Task<Photo> GetMainPhoto(int id);
         Task<Like> GetLike(int userId, int recipientId);
    }
}
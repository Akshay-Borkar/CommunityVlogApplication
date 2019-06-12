using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityVLOG.API.Helpers;
using CommunityVLOG.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityVLOG.API.Data.CommunityRepository
{
    public class CommunityRepository : ICommunityRepository
    {
        private readonly DataContext _context;

        public CommunityRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Photo> GetMainPhoto(int id)
        {
            return await _context.Photos.Where(u => u.UserId == id).FirstOrDefaultAsync(p => p.IsMainPhots);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);

            return photo;
        }

        public async Task<User> GetUser(int id)
        {
            var user =await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            return user;            
        }

        public async Task<PageList<User>> GetUsers(UserParams userParams)
        {
            var user = _context.Users.Include(p => p.Photos);
            return await PageList<User>.CreateAsync(user, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
using System;
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

        public async Task<Like> GetLike(int userId, int recipientId)
        {
            return await _context.Likes.FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeId == recipientId);
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
            var user = _context.Users.Include(p => p.Photos).OrderByDescending(u => u.LastActive).AsQueryable();
            
            user = user.Where(u => u.Id != userParams.UserId);

            user = user.Where(u => u.Gender == userParams.Gender);

            if(userParams.Likers){
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                user = user.Where(p => userLikers.Contains(p.Id));
            }
            if(userParams.Likees){
                var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                user = user.Where(p => userLikees.Contains(p.Id));
            }

            if(userParams.MinAge != 18 || userParams.MaxAge != 99 ){
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge -1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

                user = user.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }

            if(!string.IsNullOrEmpty(userParams.OrderBy)){
                switch(userParams.OrderBy){
                    case "created":
                        user = user.OrderByDescending(u => u.Created);
                        break;

                    default:
                        user = user.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            return await PageList<User>.CreateAsync(user, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<List<User>> fetchUsers()
        {
            var user = await _context.Users.Include(p => p.Photos).ToListAsync();

            return user;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers){
            var user = await _context.Users.Include(p => p.Likees).Include(p => p.Likers).FirstOrDefaultAsync(u => u.Id == id);

            if(likers){
                return user.Likers.Where(u => u.LikeeId == id).Select(i => i.LikerId);
            }else{
                return user.Likees.Where(u => u.LikerId == id).Select(i => i.LikeeId);
            }
        }
    }
}
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TL.PCStore.Models;

namespace TL.PCStore.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PcStoreDbContext db;
        private static readonly ILog Log = LogManager.GetLogger(typeof(UserRepository));

        public UserRepository()
        {
            db = new PcStoreDbContext();
        }
        public async Task<bool> AddUser(User user)
        {
            try
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<User> Find(int id)
        {
            try
            {
                return await db.Set<User>().FirstOrDefaultAsync(i => i.Id == id);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public IList<User> GetAllUsers()
        {
            try
            {
                return db.Users.ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                return await db.Set<User>().FirstOrDefaultAsync(i => i.Email == email);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }

        }

        public async Task<User> GetUserByEmailAndPassword(string email, string password)
        {
            try
            {
                return await db.Set<User>().FirstOrDefaultAsync(user => user.Email == email && user.Password.Equals(password));
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<bool> UpdateUser(User user)
        {
            try
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }
    }
}
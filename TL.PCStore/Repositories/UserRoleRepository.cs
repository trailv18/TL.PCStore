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
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly PcStoreDbContext db;
        private static readonly ILog Log = LogManager.GetLogger(typeof(UserRoleRepository));

        public UserRoleRepository()
        {
            db = new PcStoreDbContext();
        }

        public async Task<bool> AddUserRole(int userId, string roleName)
        {
            try
            {
                var role = await db.Set<Role>().FirstOrDefaultAsync(i => i.Name == roleName);
                var userRole = new UserRole
                {
                    UserId = userId,
                    RoleId = role.Id
                };
                db.UserRoles.Add(userRole);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<bool> DeleteUserRole(UserRole userRole)
        {
            try
            {
                var item = db.UserRoles.Find(userRole.Id);
                db.UserRoles.Remove(item);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<UserRole> Find(int id)
        {
            try
            {
                return await db.Set<UserRole>().FirstOrDefaultAsync(i => i.Id == id);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public IList<UserRole> GetAllUserRoles()
        {
            try
            {
                return db.UserRoles.ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<bool> UpdateUserRole(UserRole userRole)
        {
            try
            {
                db.Entry(userRole).State = EntityState.Modified;
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
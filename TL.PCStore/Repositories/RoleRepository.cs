using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TL.PCStore.Models;

namespace TL.PCStore.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly PcStoreDbContext db;
        private static readonly ILog Log = LogManager.GetLogger(typeof(RoleRepository));

        public RoleRepository()
        {
            db = new PcStoreDbContext();
        }
        public async Task<bool> AddRole(Role role)
        {
            try
            {
                db.Roles.Add(role);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<bool> DeleteRole(Role role)
        {
            try
            {
                var item = db.Roles.Find(role.Id);
                db.Roles.Remove(item);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<Role> Find(int id)
        {
            try
            {
                return await db.Set<Role>().FirstOrDefaultAsync(i => i.Id == id);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public IList<Role> GetAllRoles()
        {
            try
            {
                return db.Roles.ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }

        }

        public async Task<Role> GetRoleByName(string name)
        {
            try
            {
                return await db.Set<Role>().FirstOrDefaultAsync(i => i.Name == name);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<bool> UpdateRole(Role role)
        {
            try
            {
                db.Entry(role).State = EntityState.Modified;
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
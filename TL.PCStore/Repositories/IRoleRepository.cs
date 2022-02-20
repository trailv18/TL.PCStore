using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TL.PCStore.Models;

namespace TL.PCStore.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> Find(int id);
        Task<Role> GetRoleByName(string name);
        Task<bool> AddRole(Role role);
        Task<bool> UpdateRole(Role role);
        Task<bool> DeleteRole(Role role);
        IList<Role> GetAllRoles();
    }
}
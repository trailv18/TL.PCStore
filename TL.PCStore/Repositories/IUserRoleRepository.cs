using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL.PCStore.Models;

namespace TL.PCStore.Repositories
{
    public interface IUserRoleRepository
    {
        Task<UserRole> Find(int id);
        Task<bool> AddUserRole(int userId, string role);
        Task<bool> UpdateUserRole(UserRole userRole);
        Task<bool> DeleteUserRole(UserRole userRole);
        IList<UserRole> GetAllUserRoles();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL.PCStore.Models;

namespace TL.PCStore.Repositories
{
    public interface IUserRepository
    {
        Task<User> Find(int id);
        Task<User> GetUserByEmail(string email);
        IList<User> GetAllUsers();

        Task<User> GetUserByEmailAndPassword(string email, string password);
        Task<bool> AddUser(User user);
        Task<bool> UpdateUser(User user);
    }
}

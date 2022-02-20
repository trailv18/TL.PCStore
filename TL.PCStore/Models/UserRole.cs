using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TL.PCStore.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        public int RoleId { get; set; }
        public Role Roles { get; set; }

        public int UserId { get; set; }
        public User Users { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace FixIt_Model.Users
{
    public class UserRolesList
    {
        public List<Role> userRoles = new List<Role>();

        public static readonly Role GUEST = new Role { Name = "guest", Description = "Guest User" };

        public static readonly Role ADMIN = new Role { Name = "admin", Description = "This user has access to all the resources." };

        public static readonly Role MODERATOR_FOOD = new Role { Name = "moderator_food", Description = "This user handles all the food related things." };

        public static readonly Role Moderator_Building = new Role { Name = "moderator_building", Description = "This user handles all the building related things." };

        public UserRolesList()
        {
            userRoles.Add(GUEST);
            userRoles.Add(ADMIN);
            userRoles.Add(Moderator_Building);
            userRoles.Add(MODERATOR_FOOD);
        }
    }
}


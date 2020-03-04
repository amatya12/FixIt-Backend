using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FixIt_Backend.Extensions
{
    public static class JwtUserExtension
    {
        public static int GetJwtUserId(this ControllerBase controller)
        {
            if (!int.TryParse(controller.User.FindFirst(ClaimTypes.Sid).Value, out var userId))
            {
                throw new Exception("User not found");
            }

            return userId;
        }
        public static string GetJwtGuestUserId(this ControllerBase controller)
        {
            return controller.User.FindFirst(ClaimTypes.Sid).Value;
        }
    }
}

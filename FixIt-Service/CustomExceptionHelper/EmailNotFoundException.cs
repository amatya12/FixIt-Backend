using System;
using System.Collections.Generic;
using System.Text;

namespace FixIt_Service.CustomExceptionHelper
{
    [Serializable]
    public class EmailNotFoundException : Exception
    {
        public EmailNotFoundException()
            : base("Email not Found.")
        {

        }
    }
}

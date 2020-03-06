using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FixIt_Backend.Helpers
{
    public class ErrorCode
    {
        public const int CATEGORY_CREATE_FAILED = 100;
        public const int CATEGORY_EDIT_FAILED = 101;

        public const int ISSUE_CREATE_FAILED = 102;
        public const int ISSUE_EDIT_FAILED = 103;

        public const int SUBCATEGORY_CREATE_FAILED = 104;
        public const int SUBCATEGORY_EDIT_FAILED = 105;

        public const int DEPARTMENT_EDIT_FAILED = 106;
        public const int DEPARTMENT_CREATE_FAILED = 107;

    }
}

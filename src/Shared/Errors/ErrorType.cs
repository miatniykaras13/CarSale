using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Errors
{
    public enum ErrorType
    {
        UNKNOWN,
        NOT_FOUND,
        CONFLICT,
        VALIDATION,
        INTERNAL
    }
}

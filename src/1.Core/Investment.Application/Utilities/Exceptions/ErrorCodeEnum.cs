using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Utilities.Exceptions;

public enum ErrorCodeEnum
{
    None = 0,
    EntityDuplicated=1,
    Validation=2,
    EntityNotFound=3,
}

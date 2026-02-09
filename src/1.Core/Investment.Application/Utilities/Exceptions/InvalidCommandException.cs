using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Utilities.Exceptions;

public class InvalidCommandException:Exception
{
    public virtual string Title { get; }
    public object Errors { get; }
    public ErrorCodeEnum ErrorCode{ get;  }

    public InvalidCommandException(
        string title,
        object errors, 
        ErrorCodeEnum errorCode
        )
    {
        Title = title;
        Errors = errors;
        ErrorCode = errorCode;
    }
}

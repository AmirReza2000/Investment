using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Utilities.Exceptions;

public class EntityNotFoundException<TEntity> : InvalidCommandException where TEntity : class
{

    public EntityNotFoundException()
        : base("Entity Not Found.", $"{typeof(TEntity).Name} not found", ErrorCodeEnum.EntityNotFound)
    {
    }
}

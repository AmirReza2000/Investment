using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Investment.Application.Utilities.Resources;
using Investment.Domain.Common;

namespace Investment.Application.Utilities.Exceptions;

public class EntityDuplicateException<TEntity> : InvalidCommandException
    where TEntity : class
{
    public EntityDuplicateException(List<string> fields)
        : base(
            "Entity is duplicate.",
            fields
            .GroupBy(field => field)
            .ToDictionary(g => g.Key, g => g.Append(
                string.Format(
                format: Resources.Messages.Errors.EntityFieldDuplicate,
                arg1: typeof(TEntity).Name, arg0: g.Key))
            )


            , ErrorCodeEnum.EntityDuplicated
        )
    { }

    public EntityDuplicateException(string field)
        : base(
            "Entity is duplicate.",
             new Dictionary<string, string[]>()
                  .Append(new KeyValuePair<string, string[]>
                       (
                        field,
                        new string[] {  string.Format(
                        format: Resources.Messages.Errors.EntityFieldDuplicate,
                        arg1: typeof(TEntity).Name, arg0: field) }

                        )
                  )
            , ErrorCodeEnum.EntityDuplicated
        )
    { }

}

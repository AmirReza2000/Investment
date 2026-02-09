using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Investment.Application.Utilities;

public class BaseUpdateCommand:IRequest
{
    [BindNever]
    [JsonIgnore]
    public int Id { get; set; }
}

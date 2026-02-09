using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Users.Commands;

public class UserCreateCommand : IRequest<int>
{
    public required string EmailAddress { get; set; }

    public required string Password { get; set; }
}

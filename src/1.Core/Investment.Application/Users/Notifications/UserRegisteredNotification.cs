using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Investment.Application.Users.Notifications;

public record UserRegisteredNotification(string emailAddress,string validationToken) : INotification;

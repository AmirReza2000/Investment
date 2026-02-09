using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Users.Entities;
using Investment.Domain.Users.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Investment.Infrastructure.Domain.Users.Repositories;

public class UserQueryRepository (InvestmentDbContext dbContext): IUserQueryRepository
{

}

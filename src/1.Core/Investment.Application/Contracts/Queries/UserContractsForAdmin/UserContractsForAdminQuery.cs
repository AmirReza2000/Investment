using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Contracts.Queries.UserContractsForAdmin;

public record UserContractsForAdminQuery(int? UserContractId, int? UserId) : IRequest<UserContractsForAdminResponse[]>;

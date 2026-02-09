using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Investment.Application.Contracts.Queries.ContractMarketTypes;

public record ContractMarketTypesQuery(bool? ActiveFilter) : IRequest<ContractMarketTypeResponse[]>;

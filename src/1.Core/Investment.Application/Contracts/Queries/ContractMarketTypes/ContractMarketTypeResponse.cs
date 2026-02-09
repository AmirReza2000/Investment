using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Contracts.Queries.ContractMarketTypes;

public record ContractMarketTypeResponse(int Id, string Title, bool IsActive, decimal Rate);

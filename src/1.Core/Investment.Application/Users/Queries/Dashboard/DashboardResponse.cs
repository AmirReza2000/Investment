
using System;

using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Users.Queries.Dashboard;

public record DashboardResponse(decimal TotalBalance, decimal FreeAmount, int CountContracts, int ActiveContracts, decimal TotalProfit, decimal LockedInContract, decimal FutureProfit);

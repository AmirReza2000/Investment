using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Investment.Application.Contracts.Command.CreateContractMarketType;

public class CreateContractMarketTypeCommand : IRequest<int>
{
    public required string Title { get; set; }

    public required decimal Rate { get; set; }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Investment.Application.Utilities;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Investment.Application.Contracts.Command.UpdateMarketType;

public class UpdateContractMarketTypeCommand:BaseUpdateCommand
{
    public required string Title { get; set; }

    public required bool IsActive { get; set; }

    public required decimal Rate { get; set; }
}


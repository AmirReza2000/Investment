using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Investment.Application.Utilities;
using Investment.Domain.Contracts.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Investment.Application.Contracts.Command.UpdateContract;

public class UpdateContractCommand : BaseUpdateCommand
{
    public required string Title { get; set; }

    public required string ImageName { get; set; }

    public required decimal Rate { get; set; }

    public required short MinDurationOfDay { get; set; }

    public required int MinAmount { get; set; }

    public int? MaxAmount { get; set; }

    public required bool IsActive { get; set; }

    public Metadata? Metadate { get; set; }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;
using Investment.Domain.Contracts.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NBitcoin.OpenAsset;

namespace Investment.Application.Contracts.Command.CreateContract;

public class CreateContractCommand : IRequest<int>
{
    public required string Title { get; set; }

    public required IFormFile ImageFile { get; set; }

    public required decimal Rate { get; set; }

    public required short MinDurationOfDay { get; set; }

    public required int MinAmount { get; set; }

    public int? MaxAmount { get; set; }

    public Metadata? Metadate { get; set; }
}

using Investment.Domain.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Contracts.Queries.Contracts;

public class ContractsResponse
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string ImageName { get; set; }
    public decimal Rate { get; set; }
    public short MinDurationOfDay { get; set; }
    public int MinAmount { get; set; }
    public int? MaxAmount { get; set; }
    public bool IsActive { get; set; }
    public Metadata? Metadata { get; set; }
    public string? ImageUrl { get; set; }
}
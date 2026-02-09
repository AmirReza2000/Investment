using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Investment.Application.Utilities.DTO.Nowpayment;

public class CryptoApiStatusResponse
{
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}

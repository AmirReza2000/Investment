using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Utilities.DTO.Nowpayment;

public class AuthenticationRequest
{
    [JsonProperty("email")]
    public required string Email { get; set; }

    [JsonProperty("password")]
    public required string Password { get; set; }
}

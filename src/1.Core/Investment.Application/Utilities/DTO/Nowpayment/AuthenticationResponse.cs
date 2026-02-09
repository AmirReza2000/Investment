using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Investment.Application.Utilities.DTO.Nowpayment;

public record AuthenticationResponse([property: JsonProperty("token")] string Token);

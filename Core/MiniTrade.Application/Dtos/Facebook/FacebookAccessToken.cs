using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniTrade.Application.Dtos.Facebook
{
  public class FacebookAccessToken
  {
    [JsonPropertyName("access_Tokem")]
    public string AccessToken { get; set; }
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
  }
}

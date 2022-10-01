using Newtonsoft.Json;

namespace MiniTrade.Application.Dtos.Facebook
{
  public class FacebookUserInfoResponse
  {
    [JsonProperty("id")]
    public string? Id { get; set; }
    [JsonProperty("name")]
    public string? Name { get; set; }
    [JsonProperty("email")]
    public string? Email { get; set; }

  }
}

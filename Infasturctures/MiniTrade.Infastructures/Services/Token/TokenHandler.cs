using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniTrade.Application.Abstractions.Token;
using MiniTrade.Domain.Entities.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace MiniTrade.Infastructures.Services.Token
{
  public class TokenHandler : ITokenHandler
  {
    private readonly IConfiguration configuration;
    public TokenHandler(IConfiguration configuration)
    {
      this.configuration = configuration;
    }

    public Application.Abstractions.Token.Token CreateAccessToken(int second,AppUser user)
    {
      Application.Abstractions.Token.Token token = new();
      ///Security key simetriği alınıyor
      SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(configuration["JWT:SecurityKey"]));
      //şifrelenmiş kimliği oluşturuyoruz
      SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
      // oluşturulacak token ayarlarını veriyoruz
      token.Expiraton = DateTime.Now.AddSeconds(second);
      JwtSecurityToken jwtToken = new(
        audience: configuration["JWT:Audience"],
        issuer: configuration["JWT:Issuer"],
        expires: token.Expiraton,
        notBefore: DateTime.Now,
        signingCredentials: signingCredentials,
        claims:new List<Claim> { new (ClaimTypes.Name,user.NameSurname) }
        );
      //Token oluşturucu sınıfını newleyelim
      JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
      token.AccessToken=jwtSecurityTokenHandler.WriteToken(jwtToken);
      token.RefreshToken=CreateRefreshToken();
      return token;
      
    }

    public string CreateRefreshToken()
    {
      byte[] number = new byte[32];
      using RandomNumberGenerator random = RandomNumberGenerator.Create();
      random.GetBytes(number);
      return Convert.ToBase64String(number);
    }
  }
}

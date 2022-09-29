using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniTrade.Application.Abstractions.Token;
using System.IdentityModel.Tokens.Jwt;

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

    public Application.Abstractions.Token.Token CreateAccessToken(int minute)
    {
      Application.Abstractions.Token.Token token = new();
      ///Security key simetriği alınıyor
      SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(configuration["JWT:SecurityKey"]));
      //şifrelenmiş kimliği oluşturuyoruz
      SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
      // oluşturulacak token ayarlarını veriyoruz
      token.Expiraton = DateTime.Now.AddMinutes(minute);
      JwtSecurityToken jwtToken = new(
        audience: configuration["JWT:Audience"],
        issuer: configuration["JWT:Issuer"],
        expires: token.Expiraton,
        notBefore: DateTime.Now,
        signingCredentials: signingCredentials
        );
      //Token oluşturucu sınıfını newleyelim
      JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
      token.AccessToken=jwtSecurityTokenHandler.WriteToken(jwtToken);
      return token;
      
    }
  }
}
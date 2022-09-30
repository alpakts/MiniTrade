using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniTrade.Application.Abstractions.Token;
using MiniTrade.Application.Dtos.Facebook;
using MiniTrade.Domain.Entities.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniTrade.Application.Features.Commands.User
{
  public class LoginWithFacebookCommandRequest : IRequest<LoginWithFacebookCommandResponse>
  {
    public string AuthToken { get; set; }
    public class LoginWithFacebookCommandHandler : IRequestHandler<LoginWithFacebookCommandRequest, LoginWithFacebookCommandResponse>
    {

      private readonly UserManager<AppUser> _userManager;
      private readonly IConfiguration _configuration;
      private readonly HttpClient _httpClient;
      private readonly ITokenHandler _tokenHandler;

      public LoginWithFacebookCommandHandler(UserManager<AppUser> userManager, IConfiguration configuration, IHttpClientFactory httpClientFactory, ITokenHandler tokenHandler)
      {
        _userManager = userManager;
        _configuration = configuration;

        _httpClient = httpClientFactory.CreateClient();
        _tokenHandler = tokenHandler;
      }

      public async Task<LoginWithFacebookCommandResponse> Handle(LoginWithFacebookCommandRequest request, CancellationToken cancellationToken)
      {
        string accesToken = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["LoginProviders:Facebook:AppId"]}&client_secret={_configuration["LoginProviders:Facebook:AppSecret"]}&grant_type=client_credentials");
        FacebookAccessToken? facebookAccessToken = JsonSerializer.Deserialize<FacebookAccessToken>(accesToken);
        string ValidationInfo = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={request.AuthToken}&access_token={facebookAccessToken.AccessToken}");
        FacebookTokenValidationInfo tokenValidationInfo = JsonSerializer.Deserialize<FacebookTokenValidationInfo>(ValidationInfo);

        if (tokenValidationInfo.Data.IsValid)
        {
          string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={request.AuthToken}");
          FacebookUserInfoResponse facebookUserInfoResponse = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);
          var info = new UserLoginInfo("FACEBOOK", tokenValidationInfo.Data.UserId, "FACEBOOK");
          var user = await _userManager.FindByEmailAsync(facebookUserInfoResponse.Email);
          bool result = user == null;
          if (user == null)
          {


            user = new()
            {
              Id = Guid.NewGuid().ToString(),
              UserName = facebookUserInfoResponse.Name,
              Email = facebookUserInfoResponse.Email,
              NameSurname = facebookUserInfoResponse.Name,

            };
            var createResult = await _userManager.CreateAsync(user);
            result = createResult.Succeeded;


          }
          if (result)
          {
            await _userManager.AddLoginAsync(user, info);
            var token = new Token();
            token = _tokenHandler.CreateAccessToken(15);
            return new()
            {
              Token = token
            };
            
          }

        }
        throw new Exception("Kullanıcı geçersiz");


      }
    }

  }
  public class LoginWithFacebookCommandResponse
  {
    public Token Token { get; set; }
  }
}

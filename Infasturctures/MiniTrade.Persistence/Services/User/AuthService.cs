using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MiniTrade.Application.Abstractions.Services.User;
using MiniTrade.Application.Abstractions.Services.User.Authentication;
using MiniTrade.Application.Abstractions.Token;
using MiniTrade.Application.Dtos.Facebook;
using MiniTrade.Application.Features.Commands.Users;
using MiniTrade.Application.MappingProfiles;
using MiniTrade.Domain.Entities.Identity;
using System.Text.Json;


namespace MiniTrade.Persistence.Services.User
{
  public  class AuthService : IAuthService
  {
    private readonly HttpClient _client;
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenHandler _tokenHandler;
    private readonly IConfiguration _configuration;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IUserService _userService;
    public AuthService(UserManager<AppUser> userManager,
      IHttpClientFactory httpClientFactory,
      ITokenHandler tokenHandler,
      IConfiguration configuration,
      SignInManager<AppUser> signInManager,
      IUserService userService)
    {
      _userManager = userManager;
      _client = httpClientFactory.CreateClient();
      _tokenHandler = tokenHandler;
      _configuration = configuration;
      _signInManager = signInManager;
      _userService = userService;
    }
    private async Task<Token> CreateExternalUserAsycn(AppUser user,string email,string name,UserLoginInfo Info,int accesTokenLifetime)
    {
      bool result = user == null;
      if (user == null)
      {
        user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
          user = new()
          {
            Id = Guid.NewGuid().ToString(),
            UserName = name,
            Email = email,
            NameSurname = name,

          };
          var createResult = await _userManager.CreateAsync(user);
          result = createResult.Succeeded;
        }

      }
      if (result)
      {
        await _userManager.AddLoginAsync(user, Info);
      }
      else
      {
        throw new Exception("İnvalid External Authentication");
      }
      var token = new Token();
      token = _tokenHandler.CreateAccessToken(15, user);
      return token;
    }
    public async Task<Token> GoogleLoginAsync(string idToken,string provider,int accessTokenLifetime)
    {
      var setting = new GoogleJsonWebSignature.ValidationSettings()
      {
        Audience = new List<string> { _configuration["LoginProviders:Google"] },

      };
      var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, setting);
      var info = new UserLoginInfo(provider, payload.Scope, provider);
      var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
      var token= await CreateExternalUserAsycn(user,payload.Email,payload.Name,info, accessTokenLifetime);
      await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiraton, 60);
      return token;


    }

    public async Task<Token> LoginFacebookAsync(string authToken, int accesTokenLifetime)
    {
      string accesToken = await _client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["LoginProviders:Facebook:AppId"]}&client_secret={_configuration["LoginProviders:Facebook:AppSecret"]}&grant_type=client_credentials");
      FacebookAccessToken? facebookAccessToken = JsonSerializer.Deserialize<FacebookAccessToken>(accesToken);
      string ValidationInfo = await _client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={facebookAccessToken.AccessToken}");
      FacebookTokenValidationInfo? tokenValidationInfo = JsonSerializer.Deserialize<FacebookTokenValidationInfo>(ValidationInfo);

      if (tokenValidationInfo?.Data.IsValid != null)
      {
        string userInfoResponse = await _client.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={authToken}");
        FacebookUserInfoResponse? facebookUserInfoResponse = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);
        UserLoginInfo info = new UserLoginInfo("FACEBOOK", tokenValidationInfo.Data.UserId, "FACEBOOK");
        var user = await _userManager.FindByEmailAsync(facebookUserInfoResponse?.Email);
        Token token = await CreateExternalUserAsycn(user, facebookUserInfoResponse?.Email, facebookUserInfoResponse?.Name, info, accesTokenLifetime);
        await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiraton, 60);
        return token;
      }
      throw new Exception("İnvalid External Authentication");
    }
    public async Task<Token> LoginAsync(string userNameOrEmail, string password,int accessTokenLifetime)
    {
      AppUser user = await _userManager.FindByNameAsync(userNameOrEmail);
      if (user == null)
        user = await _userManager.FindByEmailAsync(userNameOrEmail);
      if (user == null) throw new Exception("kullanıcı adı yada şifre hatalı");

      var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
      if (result.Succeeded)// authentication başarılı 
      {
        Token token=  _tokenHandler.CreateAccessToken(accessTokenLifetime, user);
        await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiraton, 60);
        return token;
      }
      throw new Exception("geçersiz kullanıcı");
    
    }

    public async Task<Token> LoginWithRefreshTokenAsync(string refreshToken)
    {
      AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
      if (user!=null && user?.RefreshTokenExpiresAt>DateTime.Now)
      {
        Token token=_tokenHandler.CreateAccessToken(15, user);
        await _userService.UpdateRefreshToken(token.RefreshToken,user,token.Expiraton,60);
        return token;
      }
      throw new Exception("Kullanıcı bulunamadı");
    }
  }
}

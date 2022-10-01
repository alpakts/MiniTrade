using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniTrade.Application.Abstractions.Services.User;
using MiniTrade.Application.Dtos.User;
using MiniTrade.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Persistence.Services.User
{
  public class UserService : IUserService
  {
    UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
      _userManager = userManager;
    }

    public async Task<CreateUserResponse> CreateAsycn(CreateUser createUser)
    {
      
      var result = await _userManager.CreateAsync(new AppUser()
      {
        Id = Guid.NewGuid().ToString(),
        Email = createUser.Email,
        UserName = createUser.UserName,
        NameSurname = createUser.NameSurname,


      }, createUser.Password);
      if (result.Succeeded)
      {
        return new()
        {
          Message = "Kullanıcı başarıyla oluşturuldu",
          Succeeded = result.Succeeded
        };

      }
      return new()
      {
        Succeeded = result.Succeeded,
        Message = "Kullanıcı oluşturulurken bir hatayla karşılaşıldı",
        Errors = result.Errors.ToString()
      };
    }

    public async Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime AccesTokenExpiresAt, int refreshTokenLifetime)
    {
      if (user != null)
      {
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiresAt = AccesTokenExpiresAt.AddSeconds(refreshTokenLifetime);
        await _userManager.UpdateAsync(user);
      } else
        throw new Exception("BU id ile kayıtlı kullanıcı bulunamadı");
      
      
      
    }
  }
}

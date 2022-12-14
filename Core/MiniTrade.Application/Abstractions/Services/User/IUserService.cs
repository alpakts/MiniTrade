using MiniTrade.Application.Dtos.User;
using MiniTrade.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application.Abstractions.Services.User
{
  public interface IUserService
  {
    Task<CreateUserResponse> CreateAsycn(CreateUser createUser);
    Task  UpdateRefreshToken(string refreshToken,AppUser user,DateTime AccesTokenExpiresAt,int refreshTokenLifetime);
  }
}

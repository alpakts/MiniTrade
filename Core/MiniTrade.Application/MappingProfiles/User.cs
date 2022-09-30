using AutoMapper;
using MiniTrade.Application.Dtos.User;
using MiniTrade.Application.Features.Commands.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application.MappingProfiles
{
  public  class User:Profile
  {
    public User()
    {
      CreateMap<CreateUser, CreateUserCommandRequest>().ReverseMap();
      CreateMap<CreateUserCommandResponse, CreateUserResponse>().ReverseMap();
    }
  }
}

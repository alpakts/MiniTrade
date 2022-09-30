using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniTrade.Application.Abstractions.Services.User;
using MiniTrade.Application.Dtos.User;
using MiniTrade.Domain.Entities.Identity;
using MiniTrade.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application.Features.Commands.Users
{
  public  class CreateUserCommandRequest:IRequest<CreateUserCommandResponse>
  {
    public string NameSurname { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {

      private readonly IUserService _userService;
      private readonly IMapper _mapper;

      public CreateUserCommandHandler(IUserService userService, IMapper mapper)
      {
        _userService = userService;
        _mapper = mapper;
      }

      public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
      {
        CreateUser createUser=_mapper.Map<CreateUserCommandRequest,CreateUser >(request);
        CreateUserResponse createdUser = await _userService.CreateAsycn(createUser);
        CreateUserCommandResponse response= _mapper.Map<CreateUserResponse,CreateUserCommandResponse>(createdUser) ;
        return response;
      }
    }
    
  }
  public class CreateUserCommandResponse
  {
    public bool Succeeded { get; set; }
    public string? Message { get; set; }
    public string? Errors { get; set; }
  }
}


using MediatR;
using Microsoft.AspNetCore.Identity;
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
      private readonly UserManager<AppUser> _userManager;

      public CreateUserCommandHandler(UserManager<AppUser> userManager)
      {
        _userManager = userManager;
      }

      public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
      {
        var result=await _userManager.CreateAsync(new AppUser()
        {
          Id=Guid.NewGuid().ToString(),
          Email=request.Email,
          UserName=request.UserName,
          NameSurname=request.NameSurname,

          
        },request.Password);
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
    }
    
  }
  public class CreateUserCommandResponse
  {
    public bool Succeeded { get; set; }
    public string? Message { get; set; }
    public string? Errors { get; set; }
  }
}


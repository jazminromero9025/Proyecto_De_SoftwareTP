using Application.Models;
using Application.UseCases.Users;
using Application.UseCases.Users.command;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
       
        Task<UserDto?> AuthenticateAsync(LoginCommand command);

    }
}

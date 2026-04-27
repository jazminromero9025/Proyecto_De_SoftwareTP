using Application.Interfaces;
using Application.Models;
using Application.UseCases.Users;
using Application.UseCases.Users.command;
using Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCases.Users.Queries;

namespace Application.Services
{
    public class UserService : IUserService
    {
       private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
        _userRepository = userRepository;
        }



        
           public async Task<UserDto?> AuthenticateAsync(LoginCommand command)
        {
            // 1. Creamos el objeto Query para hablar con el repositorio
            
            var query = new GetUserByEmailQuery(command.Email);

            // 2. Le pedimos al repo que busque al usuario
            var user = await _userRepository.GetByEmailAsync(query);

            // 3. Verificamos si el usuario existe
            if (user == null)
            {
                return null; // El email no está en la DB
            }

            // 4. Comparamos la contraseña que nos mandaron con la precargada
            
            if (user.PasswordHash != command.Password)
            {
                return null; // La contraseña no coincide
            }

            // Si encriptamos la contraseña, acá cambiaríamos l alinea de arriba por:
            // bool isValid = Encriptador.Verify(command.Password, user.PasswordHash);

            // 5. Si todo está OK, mapeamos a DTO para devolver el ROL
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role 
            };
        }




    






    }
}

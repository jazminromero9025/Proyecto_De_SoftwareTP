using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        // El repositorio recibe la consulta y devuelve el usuario con todo (incluida la pass)
        Task<User?> GetByEmailAsync(IGetUserByEmailQuery query);

    }
}

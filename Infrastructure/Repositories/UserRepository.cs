using Application.Interfaces;
using Application.UseCases.Users;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<User?> GetByEmailAsync(IGetUserByEmailQuery query)
        {
            // Buscamos en la tabla Users comparando el Email que viene DENTRO del objeto query
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == query.Email);
        }





    }
    






}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.UseCases.Users.Queries
{
    public class GetUserByEmailQuery  : IGetUserByEmailQuery
    {
        public string Email { get; set; }
        public GetUserByEmailQuery(string email) { Email = email; }

    }
}

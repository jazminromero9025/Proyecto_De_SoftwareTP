using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILoginCommand
    {
        string Email { get; set; }
        string Password { get; set; }

    }
}

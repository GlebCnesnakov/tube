using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth
{
    public interface IAuthService
    {
        Task Register(string username, string password);
        Task<string> Authenticate(string username, string password);
    }
}

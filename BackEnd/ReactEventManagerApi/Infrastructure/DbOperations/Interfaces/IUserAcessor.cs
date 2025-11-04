using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DbOperations.Interfaces
{
    public interface IUserAcessor
    {
        string GetUserNameClaim();
         Task<User> GetUserAsync();
    }
}

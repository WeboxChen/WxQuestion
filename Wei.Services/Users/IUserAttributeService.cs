using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wei.Core.Domain.Users;

namespace Wei.Services.Users
{
    public interface IUserAttributeService
    {
        UserAttribute Get(int id);

        UserAttribute Get(string code);
    }
}

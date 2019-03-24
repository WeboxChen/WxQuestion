using System.Linq;
using Wei.Core.Data;
using Wei.Core.Domain.Users;

namespace Wei.Services.Users
{
    public class UserAttributeService : IUserAttributeService
    {
        private readonly IRepository<UserAttribute> _userAttributeRepository;
        
        public UserAttributeService(IRepository<UserAttribute> userAttributeRepository)
        {
            this._userAttributeRepository = userAttributeRepository;
        }

        public UserAttribute Get(int id)
        {
            return _userAttributeRepository.GetById(id);
        }

        public UserAttribute Get(string code)
        {
            return _userAttributeRepository.Table.FirstOrDefault(x => x.Name == code);
        }
    }
}

using Rang.Demo.CleanArchitecture.Domain.Entity;
using System.Collections.Generic;

namespace Rang.Demo.CleanArchitecture.Persistence.Ef
{
    public class EntityGatewayLoader
    {
        private IEnumerable<Club> _clubs;
        private IEnumerable<User> _users;

        public IEnumerable<Club> Clubs { get => _clubs ?? new List<Club>(); set => _clubs = value; }
        public IEnumerable<User> Users { get => _users ?? new List<User>(); set => _users = value; }
    }
}

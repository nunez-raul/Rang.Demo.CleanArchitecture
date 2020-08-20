using Rang.Demo.CleanArchitecture.Application.Common;
using Rang.Demo.CleanArchitecture.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rang.Demo.CleanArchitecture.Application.Infrastructure.PlugIn
{
    public interface IEntityGateway
    {
        //write
        Task<User> AddUserAsync(User user);
        Task<Club> AddClubAsync(Club club);

        //read
        Task<User> GetUserByUsernameAsync(string username);
        Task<IEnumerable<User>> GetUsersByListOfIdsAsync(IEnumerable<Guid> ids);
        Task<IEnumerable<User>> GetUsersByListOfUsernamesAsync(IEnumerable<string> usernames);
        Task<Page<User>> GetUsersByPageAsync(int pageNumber, int usersPerPage);

        Task<Club> GetClubByIdAsync(Guid Id);
        Task<Club> GetClubByNameAsync(string name);
    }
}

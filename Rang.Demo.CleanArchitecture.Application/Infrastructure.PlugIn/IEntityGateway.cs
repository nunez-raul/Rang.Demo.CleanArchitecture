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
        Task<Member> AddMemberAsync(Member member);
        Task<Club> AddClubAsync(Club club);

        //read
        Task<Member> GetMemberByUsernameAsync(string username);
        Task<IEnumerable<Member>> GetMembersByListOfIdsAsync(IEnumerable<Guid> ids);
        Task<IEnumerable<Member>> GetMembersByListOfUsernamesAsync(IEnumerable<string> names);
        Task<Page<Member>> GetMembersByPageAsync(int pageNumber, int membersPerPage);

        Task<Club> GetClubByIdAsync(Guid Id);
        Task<Club> GetClubByNameAsync(string name);
    }
}

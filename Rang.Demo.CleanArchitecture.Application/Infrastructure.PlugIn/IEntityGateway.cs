﻿using Rang.Demo.CleanArchitecture.Application.Common;
using Rang.Demo.CleanArchitecture.Domain.Entity;
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
        Task<Page<Member>> GetMembersByPageAsync(int pageNumber, int membersPerPage);

        Task<Club> GetClubByNameAsync(string name);
    }
}

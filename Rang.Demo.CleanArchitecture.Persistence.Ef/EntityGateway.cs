using Microsoft.EntityFrameworkCore;
using Rang.Demo.CleanArchitecture.Application.Common;
using Rang.Demo.CleanArchitecture.Application.Infrastructure.PlugIn;
using Rang.Demo.CleanArchitecture.Domain.Entity;
using Rang.Demo.CleanArchitecture.Persistence.Ef.Ef.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rang.Demo.CleanArchitecture.Persistence.Ef
{
    public class EntityGateway : IEntityGateway
    {
        //fields
        protected ModelRepositoryContext _modelRepository;

        //constructors
        public EntityGateway(ModelRepositoryContext context)
        {
            _modelRepository = context ?? throw new ArgumentNullException(nameof(context));
        }

        //methods
        public async Task<Member> AddMemberAsync(Member member)
        {
            var memberModel = member.GetModel();
            _modelRepository.MemberModelDbSet.Add(memberModel);
            await _modelRepository.SaveChangesAsync().ConfigureAwait(false);

            return member;
        }

        public async Task<Member> GetMemberByUsernameAsync(string username)
        {
            var model = await _modelRepository.MemberModelDbSet
                .Where(m => m.Username.ToLowerInvariant() == username.ToLowerInvariant()).FirstOrDefaultAsync();

            return model != null
                ? new Member(model)
                : null;
        }

        public async Task<Page<Member>> GetMembersByPageAsync(int pageNumber, int membersPerPage)
        {
            if (membersPerPage == 0)
            {
                throw new ArgumentException("0 is an invalid value.", nameof(membersPerPage));
            }

            var totalMembers = _modelRepository.MemberModelDbSet.LongCount();
            var totalPages = (totalMembers + (membersPerPage - 1)) / membersPerPage;
            if (totalMembers > 0 && pageNumber > 0)
            {
                var memberModelsOnPage =
                    await _modelRepository.MemberModelDbSet
                        .Skip(membersPerPage * (pageNumber - 1))
                        .Take(membersPerPage)
                        .ToListAsync();

                return new Page<Member>(pageNumber, membersPerPage, totalPages, totalMembers,
                        memberModelsOnPage.Select(memberModel => new Member(memberModel))
                        .ToList());
            }

            return new Page<Member>(pageNumber, membersPerPage, totalPages, totalMembers, new List<Member>());
        }


        public async Task<Club> AddClubAsync(Club club)
        {
            var clubModel = club.GetModel();
            _modelRepository.ClubModelDbSet.Add(clubModel);
            await _modelRepository.SaveChangesAsync().ConfigureAwait(false);

            return club;
        }

        public async Task<Club> GetClubByNameAsync(string name)
        {
            var model = await _modelRepository.ClubModelDbSet
               .Where(c => c.Name.ToLowerInvariant() == name.ToLowerInvariant()).FirstOrDefaultAsync();

            return model != null
                ? new Club(model)
                : null;
        }
    }
}

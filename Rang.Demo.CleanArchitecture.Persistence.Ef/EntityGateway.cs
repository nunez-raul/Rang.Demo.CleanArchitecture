using Microsoft.EntityFrameworkCore;
using Rang.Demo.CleanArchitecture.Application.Infrastructure.PlugIn;
using Rang.Demo.CleanArchitecture.Domain.Entity;
using Rang.Demo.CleanArchitecture.Persistence.Ef.Ef.Context;
using System;
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

        public async Task<Member> GetMemberByCodenameAsync(string codename)
        {
            var model = await _modelRepository.MemberModelDbSet
                .Where(m => m.Codename == codename).FirstOrDefaultAsync();

            return model != null
                ? new Member(model)
                : null;
        }
    }
}

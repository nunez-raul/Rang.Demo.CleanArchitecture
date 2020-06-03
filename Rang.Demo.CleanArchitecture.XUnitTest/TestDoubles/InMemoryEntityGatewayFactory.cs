using Microsoft.EntityFrameworkCore;
using Rang.Demo.CleanArchitecture.Application.Infrastructure.PlugIn;
using Rang.Demo.CleanArchitecture.Domain.Entity;
using Rang.Demo.CleanArchitecture.Persistence.Ef;
using Rang.Demo.CleanArchitecture.Persistence.Ef.Ef.Context;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rang.Demo.CleanArchitecture.XUnitTest.TestDoubles
{
    public static class InMemoryEntityGatewayFactory
    {
        public static async Task<IEntityGateway> CreateEntityGateway()
        {
            return new EntityGateway(
                new ModelRepositoryContext(
                    new DbContextOptionsBuilder<ModelRepositoryContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options));
        }

        public static async Task<IEntityGateway> CreateEntityGatewayAsync(Member[] members)
        {
            var modelRepositoryContext = new ModelRepositoryContext(
                    new DbContextOptionsBuilder<ModelRepositoryContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options);

            await modelRepositoryContext.MemberModelDbSet.AddRangeAsync(members.Select(m => m.GetModel()).ToArray());
            await modelRepositoryContext.SaveChangesAsync();


            return new EntityGateway(modelRepositoryContext);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Rang.Demo.CleanArchitecture.Application.Infrastructure.PlugIn;
using Rang.Demo.CleanArchitecture.Domain.Entity;
using Rang.Demo.CleanArchitecture.Persistence.Ef;
using Rang.Demo.CleanArchitecture.Persistence.Ef.Ef.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rang.Demo.CleanArchitecture.XUnitTest.TestDoubles
{
    public static class InMemoryEntityGatewayFactory
    {
        public static IEntityGateway CreateEntityGateway()
        {
            return new EntityGateway(
                new ModelRepositoryContext(
                    new DbContextOptionsBuilder<ModelRepositoryContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options));
        }

        public static async Task<IEntityGateway> CreateEntityGatewayAsync(User[] users)
        {
            var modelRepositoryContext = new ModelRepositoryContext(
                    new DbContextOptionsBuilder<ModelRepositoryContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options);

            await modelRepositoryContext.UserModelDbSet
                .AddRangeAsync(users.Select(m => m.GetModel()).ToArray());
            await modelRepositoryContext.SaveChangesAsync();


            return new EntityGateway(modelRepositoryContext);
        }

        public static async Task<IEntityGateway> CreateEntityGatewayAsync(Club[] clubs)
        {
            var modelRepositoryContext = new ModelRepositoryContext(
                    new DbContextOptionsBuilder<ModelRepositoryContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options);

            await modelRepositoryContext.ClubModelDbSet
                .AddRangeAsync(clubs.Select(m => m.GetModel()).ToArray());
            await modelRepositoryContext.SaveChangesAsync();


            return new EntityGateway(modelRepositoryContext);
        }

        public static async Task<IEntityGateway> CreateEntityGatewayAsync(EntityGatewayLoader input)
        {
            if (input == null)
                input = new EntityGatewayLoader();

            var modelRepositoryContext = new ModelRepositoryContext(
                    new DbContextOptionsBuilder<ModelRepositoryContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options);

            if (input.Clubs.Any())
            {
                await modelRepositoryContext.ClubModelDbSet
                    .AddRangeAsync(input.Clubs.Select(m => m.GetModel()).ToArray());
            }

            if (input.Users.Any())
            {
                await modelRepositoryContext.UserModelDbSet
                    .AddRangeAsync(input.Users.Select(m => m.GetModel()).ToArray());
            }

            await modelRepositoryContext.SaveChangesAsync();

            return new EntityGateway(modelRepositoryContext);
        }

        public static EntityGatewayLoader GetLoader()
        {
            return new EntityGatewayLoader();
        }
    }
}

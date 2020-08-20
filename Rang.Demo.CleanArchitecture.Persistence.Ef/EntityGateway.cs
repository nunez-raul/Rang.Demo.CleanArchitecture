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
        public async Task<User> AddUserAsync(User user)
        {
            var userModel = user.GetModel();
            _modelRepository.UserModelDbSet.Add(userModel);
            await _modelRepository.SaveChangesAsync().ConfigureAwait(false);

            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var model = await _modelRepository.UserModelDbSet
                .Where(m => m.Username.ToLowerInvariant() == username.ToLowerInvariant()).FirstOrDefaultAsync();

            return model != null
                ? new User(model)
                : null;
        }

        public async Task<IEnumerable<User>> GetUsersByListOfIdsAsync(IEnumerable<Guid> ids)
        {
            if (ids == null || !ids.Any())
                return new List<User>();

            var models = await _modelRepository
                .UserModelDbSet
                .Where(userModel => ids.Contains(userModel.Id))
                .ToListAsync();

            return models.Any()
                ? models.Select(userModel => new User(userModel))
                : new List<User>();
        }

        public async Task<IEnumerable<User>> GetUsersByListOfUsernamesAsync(IEnumerable<string> userNames)
        {
            if (userNames == null || !userNames.Any())
                return new List<User>();

            var models = await _modelRepository
                .UserModelDbSet
                .Where(userModel => userNames.Select(n => n.ToLowerInvariant()).Contains(userModel.Username.ToLowerInvariant()))
                .ToListAsync();

            return models.Any() 
                ? models.Select(usrModel => new User(usrModel))
                : new List<User>();
        }

        public async Task<Page<User>> GetUsersByPageAsync(int pageNumber, int usersPerPage)
        {
            if (usersPerPage == 0)
            {
                throw new ArgumentException("0 is an invalid value.", nameof(usersPerPage));
            }

            var totalUsers = _modelRepository.UserModelDbSet.LongCount();
            var totalPages = (totalUsers + (usersPerPage - 1)) / usersPerPage;
            if (totalUsers > 0 && pageNumber > 0)
            {
                var userModelsOnPage =
                    await _modelRepository.UserModelDbSet
                        .Skip(usersPerPage * (pageNumber - 1))
                        .Take(usersPerPage)
                        .ToListAsync();

                return new Page<User>(pageNumber, usersPerPage, totalPages, totalUsers,
                        userModelsOnPage.Select(userModel => new User(userModel))
                        .ToList());
            }

            return new Page<User>(pageNumber, usersPerPage, totalPages, totalUsers, new List<User>());
        }

        public async Task<Club> AddClubAsync(Club club)
        {
            var clubModel = club.GetModel();
            _modelRepository.ClubModelDbSet.Add(clubModel);
            await _modelRepository.SaveChangesAsync().ConfigureAwait(false);

            return club;
        }

        public async Task<Club> GetClubByIdAsync(Guid Id)
        {
            var model = await _modelRepository.ClubModelDbSet
               .Where(c => c.Id == Id).FirstOrDefaultAsync();

            return model != null
                ? new Club(model)
                : null;
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

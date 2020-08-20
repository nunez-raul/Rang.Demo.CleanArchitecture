using Microsoft.EntityFrameworkCore;
using Rang.Demo.CleanArchitecture.Domain.Model;
using Rang.Demo.CleanArchitecture.Persistence.Ef.Ef.TypeConfig;
using System.Threading.Tasks;

namespace Rang.Demo.CleanArchitecture.Persistence.Ef.Ef.Context
{
    public class ModelRepositoryContext : DbContext
    {
        //DbSets
        public virtual DbSet<UserModel> UserModelDbSet { get; set; }
        public virtual DbSet<ClubModel> ClubModelDbSet { get; set; }

        //constructors
        public ModelRepositoryContext(DbContextOptions<ModelRepositoryContext> options)
            : base(options) { }

        //methods
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserModelConfiguration());
            modelBuilder.ApplyConfiguration(new ClubModelConfiguration());
        }
    }
}

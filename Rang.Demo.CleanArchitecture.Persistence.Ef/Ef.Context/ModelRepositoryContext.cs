using Microsoft.EntityFrameworkCore;
using Rang.Demo.CleanArchitecture.Domain.Model;
using Rang.Demo.CleanArchitecture.Persistence.Ef.Ef.TypeConfig;
using System.Threading.Tasks;

namespace Rang.Demo.CleanArchitecture.Persistence.Ef.Ef.Context
{
    public class ModelRepositoryContext : DbContext
    {
        //DbSets
        public virtual DbSet<MemberModel> MemberModelDbSet { get; set; }
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
            modelBuilder.ApplyConfiguration(new MemberModelConfiguration());
            modelBuilder.ApplyConfiguration(new ClubModelConfiguration());
        }
    }
}

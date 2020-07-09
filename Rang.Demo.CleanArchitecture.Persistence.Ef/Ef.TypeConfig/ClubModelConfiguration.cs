using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rang.Demo.CleanArchitecture.Domain.Entity;
using Rang.Demo.CleanArchitecture.Domain.Model;

namespace Rang.Demo.CleanArchitecture.Persistence.Ef.Ef.TypeConfig
{
    internal class ClubModelConfiguration : IEntityTypeConfiguration<ClubModel>
    {
        public void Configure(EntityTypeBuilder<ClubModel> builder)
        {
            builder.ToTable("Clubs").HasKey(e => e.Id);

            builder.Property(e => e.Id).IsRequired();
            builder.Property(e => e.Name)
                .HasMaxLength(Club.NAME_MAX_LENGTH)
                .IsRequired(false);
        }
    }
}

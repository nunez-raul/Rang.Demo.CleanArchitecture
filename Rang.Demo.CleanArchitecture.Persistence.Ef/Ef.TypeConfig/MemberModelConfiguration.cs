using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rang.Demo.CleanArchitecture.Domain.Entity;
using Rang.Demo.CleanArchitecture.Domain.Model;

namespace Rang.Demo.CleanArchitecture.Persistence.Ef.Ef.TypeConfig
{
    internal class MemberModelConfiguration : IEntityTypeConfiguration<MemberModel>
    {
        public void Configure(EntityTypeBuilder<MemberModel> builder)
        {
            builder.ToTable("Members").HasKey(e => e.Id);

            builder.Property(e => e.Id).IsRequired();
            builder.Property(e => e.Username)
                .HasMaxLength(Member.USERNAME_MAX_LENGTH)
                .IsRequired(false);
        }
    }
}

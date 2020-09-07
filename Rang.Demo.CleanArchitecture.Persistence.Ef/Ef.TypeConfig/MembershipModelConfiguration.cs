using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rang.Demo.CleanArchitecture.Domain.Model;

namespace Rang.Demo.CleanArchitecture.Persistence.Ef.Ef.TypeConfig
{
    internal class MembershipModelConfiguration : IEntityTypeConfiguration<MembershipModel>
    {
        public void Configure(EntityTypeBuilder<MembershipModel> builder)
        {
            builder.ToTable("Memberships").HasKey(e => new { e.UserId, e.ClubId });

            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.ClubId).IsRequired();

            builder.HasOne(membershipModel => membershipModel.UserModel)
            .WithMany(userModel => userModel.MembershipModels)
            .HasForeignKey(membershipModel => membershipModel.UserId);

            builder.HasOne(membershipModel => membershipModel.ClubModel)
                .WithMany(clubModel => clubModel.MembershipModels)
                .HasForeignKey(membershipModel => membershipModel.ClubId);

        }
    }
}

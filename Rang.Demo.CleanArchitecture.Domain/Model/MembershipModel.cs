using System;

namespace Rang.Demo.CleanArchitecture.Domain.Model
{
    public class MembershipModel : BaseModel
    {
        //properties
        public Guid UserId { get; set; }
        public UserModel UserModel { get; set; }

        public Guid ClubId { get; set; }
        public ClubModel ClubModel { get; set; }
    }
}

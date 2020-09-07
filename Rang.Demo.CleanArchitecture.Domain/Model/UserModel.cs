using System.Collections.Generic;

namespace Rang.Demo.CleanArchitecture.Domain.Model
{
    public class UserModel : BaseModel
    {
        //properties
        public string Username { get; set; }
        public ICollection<MembershipModel> MembershipModels { get; set; }
    }
}

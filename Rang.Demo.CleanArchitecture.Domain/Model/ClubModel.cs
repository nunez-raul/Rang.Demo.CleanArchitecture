using System.Collections.Generic;

namespace Rang.Demo.CleanArchitecture.Domain.Model
{
    public class ClubModel : BaseModel
    {
        //properties
        public string Name { get; set; }
        public ICollection<MembershipModel> MembershipModels { get; set; }

        //constructors
        public ClubModel()
        {
            MembershipModels = new List<MembershipModel>();
        }
    }
}

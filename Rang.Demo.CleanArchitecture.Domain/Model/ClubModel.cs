using System.Collections.Generic;

namespace Rang.Demo.CleanArchitecture.Domain.Model
{
    public class ClubModel : BaseModel
    {
        //properties
        public string Name { get; set; }
        public ICollection<MemberModel> Members { get; set; }

        //constructors
        public ClubModel()
        {
            Members = new List<MemberModel>();
        }
    }
}

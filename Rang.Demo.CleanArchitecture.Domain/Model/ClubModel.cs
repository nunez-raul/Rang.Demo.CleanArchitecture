using System.Collections.Generic;

namespace Rang.Demo.CleanArchitecture.Domain.Model
{
    public class ClubModel : BaseModel
    {
        //properties
        public string Name { get; set; }
        public ICollection<ClubMemberModel> ClubMembers { get; set; }

        //constructors
        public ClubModel()
        {
            ClubMembers = new List<ClubMemberModel>();
        }
    }
}

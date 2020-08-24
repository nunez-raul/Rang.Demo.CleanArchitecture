using System.Collections.Generic;

namespace Rang.Demo.CleanArchitecture.Domain.Model
{
    public class ClubModel : BaseModel
    {
        //properties
        public string Name { get; set; }
        public ICollection<ClubMemberModel> ClubMemberModels { get; set; }

        //constructors
        public ClubModel()
        {
            ClubMemberModels = new List<ClubMemberModel>();
        }
    }
}

using Rang.Demo.CleanArchitecture.Domain.Model;
using System.Collections.Generic;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.In
{
    public class AddMembersToClubInputModel
    {
        //fields
        protected ClubModel _clubModel;
        protected IList<UserModel> _memberModelsToAdd;

        //properties
        public ClubModel ClubModel { get => _clubModel; set => _clubModel = value; }
        public IList<UserModel> MemberModelsToAdd { get => _memberModelsToAdd; set => _memberModelsToAdd = value; } 

        //constructors
        public AddMembersToClubInputModel()
        {
            _clubModel = new ClubModel();
            _memberModelsToAdd = new List<UserModel>();
        }

        //methods
        public ClubModel ToClubModel()
        {
            return _clubModel;
        }
    }
}

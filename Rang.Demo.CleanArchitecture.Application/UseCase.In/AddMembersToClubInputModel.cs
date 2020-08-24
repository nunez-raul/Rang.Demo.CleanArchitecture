using Rang.Demo.CleanArchitecture.Domain.Model;
using System.Collections.Generic;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.In
{
    public class AddMembersToClubInputModel
    {
        //fields
        protected ClubModel _clubModel;
        protected IEnumerable<UserModel> _userModelsToAdd;

        //properties
        public ClubModel ClubModel { get => _clubModel; set => _clubModel = value; }
        public IEnumerable<UserModel> UserModelsToAdd { get => _userModelsToAdd; set => _userModelsToAdd = value; } 

        //constructors
        public AddMembersToClubInputModel()
        {
            _clubModel = new ClubModel();
            _userModelsToAdd = new List<UserModel>();
        }

        //methods
        public ClubModel ToClubModel()
        {
            return _clubModel;
        }
    }
}

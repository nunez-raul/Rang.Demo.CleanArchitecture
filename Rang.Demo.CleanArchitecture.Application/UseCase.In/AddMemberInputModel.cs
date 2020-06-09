using Rang.Demo.CleanArchitecture.Domain.Model;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.In
{
    public class AddMemberInputModel
    {
        //fields
        protected MemberModel _model;

        //properties
        public string Username { get => _model.Username; set => _model.Username = value; }

        //constructors
        public AddMemberInputModel()
        {
            _model = new MemberModel();
        }

        //methods
        public MemberModel ToMemberModel()
        {
            return _model;
        }
    }
}

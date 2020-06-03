using Rang.Demo.CleanArchitecture.Domain.Model;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.In
{
    public class AddMemberInputModel
    {
        //fields
        protected MemberModel _model;

        //properties
        public string Codename { get => _model.Codename; set => _model.Codename = value; }

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

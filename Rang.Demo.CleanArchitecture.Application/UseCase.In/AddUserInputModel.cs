using Rang.Demo.CleanArchitecture.Domain.Model;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.In
{
    public class AddUserInputModel
    {
        //fields
        protected UserModel _model;

        //properties
        public string Username { get => _model.Username; set => _model.Username = value; }

        //constructors
        public AddUserInputModel()
        {
            _model = new UserModel();
        }

        //methods
        public UserModel ToUserModel()
        {
            return _model;
        }
    }
}

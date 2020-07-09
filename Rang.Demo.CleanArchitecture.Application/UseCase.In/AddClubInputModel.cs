using Rang.Demo.CleanArchitecture.Domain.Model;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.In
{
    public class AddClubInputModel
    {
        //fields
        protected ClubModel _model;

        //properties
        public string Name { get => _model.Name; set => _model.Name = value; }

        //constructors
        public AddClubInputModel()
        {
            _model = new ClubModel();
        }

        //methods
        public ClubModel ToClubModel()
        {
            return _model;
        }
    }
}

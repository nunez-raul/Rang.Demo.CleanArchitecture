using Rang.Demo.CleanArchitecture.Domain.Model;
using System;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.Out
{
    public class AddClubOutputModel
    {
        //fields
        protected ClubModel _model;

        //properties
        public Guid Id { get => _model.Id; }
        public string Name { get => _model.Name; }

        public AddClubOutputModel(ClubModel model)
        {
            _model = model;
        }
    }
}

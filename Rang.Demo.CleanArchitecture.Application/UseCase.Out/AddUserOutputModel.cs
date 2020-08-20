using Rang.Demo.CleanArchitecture.Domain.Model;
using System;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.Out
{
    public class AddUserOutputModel
    {
        //fields
        protected UserModel _model;

        //properties
        public Guid Id { get => _model.Id; }
        public string Username { get => _model.Username; }

        public AddUserOutputModel(UserModel model)
        {
            _model = model;
        }
    }
}

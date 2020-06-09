using Rang.Demo.CleanArchitecture.Domain.Model;
using System;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.Out
{
    public class AddMemberOutputModel
    {
        //fields
        protected MemberModel _model;

        //properties
        public Guid Id { get => _model.Id; }
        public string Username { get => _model.Username; }

        public AddMemberOutputModel(MemberModel model)
        {
            _model = model;
        }
    }
}

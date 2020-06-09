using Rang.Demo.CleanArchitecture.Domain.Common;
using Rang.Demo.CleanArchitecture.Domain.Model;
using System;
using System.Linq;

namespace Rang.Demo.CleanArchitecture.Domain.Entity
{
    public class Member : BaseEntity<MemberModel>
    {
        //fields
        public const int USERNAME_MAX_LENGTH = 100;

        //properties
        public Guid Id { get => _model.Id; }
        public string Username { get => _model.Username; set => _model.Username = value; }

        //constructors
        public Member()
            : base(new MemberModel()) { }

        public Member(MemberModel model)
            : base(model) { }

        //methods
        public override MemberModel GetModel()
        {
            return _model;
        }

        protected override void InitializeMe()
        {
            //used for initializing member collections
        }

        protected override bool ValidateMe()
        {
            var baseMessage = "The supplied {0} exceeded maximum length allowed of: {1}.";

            if (Username == null || string.IsNullOrWhiteSpace(Username))
            {
                AddModelValidationError(ModelValidationStatusCode.RequiredInformationMissing, "username is a required field.");
            }
            else
            {
                if (Username.Trim().Length > USERNAME_MAX_LENGTH)
                {
                    AddModelValidationError(ModelValidationStatusCode.CapacityExceeded, string.Format(baseMessage, "username", USERNAME_MAX_LENGTH));
                }
            }

            return !ModelValidationErrors.Any();
        }
    }
}

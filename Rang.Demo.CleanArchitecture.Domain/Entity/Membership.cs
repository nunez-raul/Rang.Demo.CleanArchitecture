using Rang.Demo.CleanArchitecture.Domain.Common;
using Rang.Demo.CleanArchitecture.Domain.Model;
using System;
using System.Linq;

namespace Rang.Demo.CleanArchitecture.Domain.Entity
{
    public class Membership : BaseEntity<MembershipModel>
    {
        //properties
        public Guid Id { get => _model.Id; }
        public Guid UserId { get => _model.UserId; set => _model.UserId = value; }

        //constructors
        public Membership()
            : base(new MembershipModel()) { }

        public Membership(MembershipModel model)
            : base(model) { }

        //methods
        public override MembershipModel GetModel()
        {
            return _model;
        }

        protected override void InitializeMe()
        {
            //used for initializing member collections
        }

        protected override bool ValidateMe()
        {
            if (UserId == null || UserId == Guid.Empty)
            {
                AddModelValidationError(ModelValidationStatusCode.RequiredInformationMissing, "user Id is a required field.");
            }

            return !ModelValidationErrors.Any();
        }
    }
}

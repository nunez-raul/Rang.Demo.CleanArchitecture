using Rang.Demo.CleanArchitecture.Domain.Common;
using Rang.Demo.CleanArchitecture.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rang.Demo.CleanArchitecture.Domain.Entity
{
    public class Club : BaseEntity<ClubModel>
    {
        //fields
        public const int NAME_MAX_LENGTH = 100;

        //properties
        public Guid Id { get => _model.Id; }
        public string Name { get => _model.Name; set => _model.Name = value; }
        public ICollection<Member> Members { get; private set; }

        //constructors
        public Club()
            : base(new ClubModel()) { }

        public Club(ClubModel model)
            : base(model) { }

        //methods
        public override ClubModel GetModel()
        {
            return _model;
        }

        protected override void InitializeMe()
        {
            //used for initializing member collections
            if (_model.Members.Count > 0)
            {
                //prevent duplicates
                var dictionary = _model.Members.Select(memberModel => new Member(memberModel)).ToDictionary(m => m.Id);
                Members = dictionary.Values.ToList();
            }
            else
            {
                Members = new List<Member>();
            }
        }

        protected override bool ValidateMe()
        {
            var baseMessage = "The supplied {0} exceeded maximum length allowed of: {1}.";

            //Name
            if (Name == null || string.IsNullOrWhiteSpace(Name))
            {
                AddModelValidationError(ModelValidationStatusCode.RequiredInformationMissing, "name is a required field.");
            }
            else
            {
                if (Name.Trim().Length > NAME_MAX_LENGTH)
                {
                    AddModelValidationError(ModelValidationStatusCode.CapacityExceeded, string.Format(baseMessage, "name", NAME_MAX_LENGTH));
                }
            }

            //Members
            ValidateMemberList();

            return !ModelValidationErrors.Any();
        }

        protected virtual void ValidateMemberList()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Member m in Members)
            {
                if (!m.IsValid)
                {
                    foreach (var validationError in m.ModelValidationErrors)
                    {
                        sb.AppendLine(string.Format("{0}: {1}", validationError.Key, string.Join(",", validationError.Value)));
                    }

                    AddModelValidationError(ModelValidationStatusCode.InternalMemberFailedValidation, sb.ToString());
                    sb.Clear();
                }
            }
        }
    }
}

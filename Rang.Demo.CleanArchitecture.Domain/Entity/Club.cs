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
        public ICollection<ClubMember> ClubMembers { get; private set; }

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
            if (_model.ClubMemberModels.Count > 0)
            {
                ClubMembers = _model.ClubMemberModels.Select(clubMemberModel => new ClubMember(clubMemberModel)).ToList();
                ValidateClubMemberList();
            }
            else
            {
                ClubMembers = new List<ClubMember>();
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

            //Club Members
            ValidateClubMemberList();

            return !ModelValidationErrors.Any();
        }

        protected virtual void ValidateClubMemberList()
        {
            StringBuilder sb = new StringBuilder();
            foreach (ClubMember cm in ClubMembers)
            {
                if(cm == null)
                {
                    throw new ArgumentNullException("ClubMemberModels");
                }

                if (!cm.IsValid)
                {
                    foreach (var validationError in cm.ModelValidationErrors)
                    {
                        sb.AppendLine(string.Format("{0}: {1}", validationError.Key, string.Join(",", validationError.Value)));
                    }

                    AddModelValidationError(ModelValidationStatusCode.InternalMemberFailedValidation, sb.ToString());
                    sb.Clear();
                }
            }

            if (!ModelValidationErrors.ContainsKey(ModelValidationStatusCode.InvalidDataSupplied))
                CheckDuplicatedClubMemberModels();
        }

        private bool CheckDuplicatedClubMemberModels()
        {
            var hasDuplicates = _model.ClubMemberModels
                .GroupBy(model => model.UserId)
                .Where(g => g.Count() > 1).Any();

            if (hasDuplicates)
            {
                AddModelValidationError(ModelValidationStatusCode.InvalidDataSupplied, "There are some duplicated Club members.");
                return true;
            }
            return false;
        }
    }
}

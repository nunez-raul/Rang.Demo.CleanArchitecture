using Rang.Demo.CleanArchitecture.Application.Common;
using Rang.Demo.CleanArchitecture.Application.Infrastructure.PlugIn;
using Rang.Demo.CleanArchitecture.Application.UseCase.In;
using Rang.Demo.CleanArchitecture.Application.UseCase.In.Boundary;
using Rang.Demo.CleanArchitecture.Application.UseCase.Out;
using Rang.Demo.CleanArchitecture.Application.UseCase.Out.Boundary;
using Rang.Demo.CleanArchitecture.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.Interactor
{
    public class AddMembersToClubInteractor :
        BaseEntityGatewayInteractor, IAddMembersToClubInteractor
    {
        // fields
        protected IAddMembersToClubPresenter _presenter;
        protected AddMembersToClubInputModel _inputModel;
        protected Club _loadedClub;
        protected IEnumerable<Member> _loadedMembers;

        // constructor
        public AddMembersToClubInteractor(IAddMembersToClubPresenter presenter, IEntityGateway entityGateway)
            : base(entityGateway)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        // methods
        public async Task<CommandResult<AddMembersToClubOutputModel>> AddMembersToClubAsync(AddMembersToClubInputModel inputModel)
        {
            _inputModel = inputModel ?? throw new ArgumentNullException(nameof(inputModel));


            if (IsClubMissingFromInput())
                return PresentRequiredClubMissingResult();
            
            
            if (AreAllMembersToAddMissingFromInput())
                return PresentRequiredMembersToAddMissingResult();
            
            
            if (! await LoadClubFromStorageByInputModelAsync())
                return PresentClubNotFoundResult();


            if (!await LoadMembersFromStorageByInputModelAsync())
                return PresentMembersInListNotFoundResult();


            throw new NotImplementedException();
        }

        protected virtual bool IsClubMissingFromInput()
        {
            if (_inputModel.ClubModel == null ||
                ((_inputModel.ClubModel.Id == null || _inputModel.ClubModel.Id == Guid.Empty) && string.IsNullOrWhiteSpace(_inputModel.ClubModel.Name)))
                return true;
            
            return false;
        }

        protected virtual CommandResult<AddMembersToClubOutputModel> PresentRequiredClubMissingResult()
        {
            _presenter.PresentErrorMessage("A valid Club is required.");

            return new CommandResult<AddMembersToClubOutputModel>
            {
                Status = CommandResultStatusCode.MissingClub,
                ModelValidationErrors = null,
                OutputModel = null
            };
        }

        protected virtual async Task<bool> LoadClubFromStorageByInputModelAsync()
        {
             if( _inputModel.ClubModel.Id == Guid.Empty)
                _loadedClub = await _entityGateway.GetClubByNameAsync(_inputModel.ClubModel.Name);
             else
                _loadedClub = await _entityGateway.GetClubByIdAsync(_inputModel.ClubModel.Id);

            return _loadedClub != null;
        }

        protected virtual CommandResult<AddMembersToClubOutputModel> PresentClubNotFoundResult()
        {
            _presenter.PresentInformationMessage("Couldn't find the selected club.");

            return new CommandResult<AddMembersToClubOutputModel>
            {
                Status = CommandResultStatusCode.ClubNotFound,
                ModelValidationErrors = null,
                OutputModel = null
            };
        }

        protected virtual bool AreAllMembersToAddMissingFromInput()
        {
            return _inputModel.MemberModelsToAdd == null || !_inputModel.MemberModelsToAdd.Any();
        }

        protected virtual CommandResult<AddMembersToClubOutputModel> PresentRequiredMembersToAddMissingResult()
        {
            _presenter.PresentErrorMessage(string.Format("No members were selected to add to the {0} club.", _inputModel.ClubModel.Name));

            return new CommandResult<AddMembersToClubOutputModel>
            {
                Status = CommandResultStatusCode.MissingMembersToAdd,
                ModelValidationErrors = null,
                OutputModel = null
            };
        }

        protected virtual async Task<bool> LoadMembersFromStorageByInputModelAsync()
        {
            var idsOfMembersToAdd = _inputModel.MemberModelsToAdd
                .Where(model => model.Id != Guid.Empty)
                .Select(memberModel => memberModel.Id)
                .ToList();

            var usernamesOfMembersToAddWithNoIdSupplied = _inputModel.MemberModelsToAdd
                .Where(memberModel => memberModel.Id == Guid.Empty)
                .Select(models => models.Username)
                .ToList();

            if (idsOfMembersToAdd.Count + usernamesOfMembersToAddWithNoIdSupplied.Count != _inputModel.MemberModelsToAdd.Count)
                return false;

            var membersRecoveredById = await _entityGateway
                .GetMembersByListOfIdsAsync(idsOfMembersToAdd);

            var membersRecoveredByUsername = await _entityGateway
                .GetMembersByListOfUsernamesAsync(usernamesOfMembersToAddWithNoIdSupplied);

            _loadedMembers = membersRecoveredById.Concat(membersRecoveredByUsername);

            return _inputModel.MemberModelsToAdd.Count == _loadedMembers.Count();
        }
        
        protected virtual CommandResult<AddMembersToClubOutputModel> PresentMembersInListNotFoundResult()
        {
            _presenter.PresentInformationMessage("Couldn't find some of the members selected.");

            return new CommandResult<AddMembersToClubOutputModel>
            {
                Status = CommandResultStatusCode.MembersInListNotFound,
                ModelValidationErrors = null,
                OutputModel = null
            };
        }
    }
}

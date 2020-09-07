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
        protected IEnumerable<User> _loadedUsers;

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
            
            
            if (AreAllUsersToAddMissingFromInput())
                return PresentRequiredUsersToAddMissingResult();
            
            
            if (! await LoadClubFromStorageByInputModelAsync())
                return PresentClubNotFoundResult();


            if (!await LoadUsersFromStorageByInputModelAsync())
                return PresentUsersInListNotFoundResult();


            foreach(var user in _loadedUsers)
            {
                var membership = new Membership { UserId = user.Id };
                _loadedClub.Memberships.Add(membership);
            }

            if (!_loadedClub.IsValid)
            {
                return PresentValidationErrors();
            }


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

        protected virtual bool AreAllUsersToAddMissingFromInput()
        {
            return _inputModel.UserModelsToAdd == null || !_inputModel.UserModelsToAdd.Any();
        }

        protected virtual CommandResult<AddMembersToClubOutputModel> PresentRequiredUsersToAddMissingResult()
        {
            _presenter.PresentErrorMessage(string.Format("No users were selected to add to the {0} club.", _inputModel.ClubModel.Name));

            return new CommandResult<AddMembersToClubOutputModel>
            {
                Status = CommandResultStatusCode.MissingUsersToAdd,
                ModelValidationErrors = null,
                OutputModel = null
            };
        }

        protected virtual async Task<bool> LoadUsersFromStorageByInputModelAsync()
        {
            var idsOfUsersToAdd = _inputModel.UserModelsToAdd
                .Where(model => model.Id != Guid.Empty)
                .Select(memberModel => memberModel.Id)
                .ToList();

            var usernamesOfMembersToAddWithNoIdSupplied = _inputModel.UserModelsToAdd
                .Where(model => model.Id == Guid.Empty)
                .Select(models => models.Username)
                .ToList();

            if (idsOfUsersToAdd.Count + usernamesOfMembersToAddWithNoIdSupplied.Count != _inputModel.UserModelsToAdd.Count())
                return false;

            var usersRecoveredById = await _entityGateway
                .GetUsersByListOfIdsAsync(idsOfUsersToAdd);

            var usersRecoveredByUsername = await _entityGateway
                .GetUsersByListOfUsernamesAsync(usernamesOfMembersToAddWithNoIdSupplied);

            _loadedUsers = usersRecoveredById.Concat(usersRecoveredByUsername);

            return _inputModel.UserModelsToAdd.Count() == _loadedUsers.Count();
        }
        
        protected virtual CommandResult<AddMembersToClubOutputModel> PresentUsersInListNotFoundResult()
        {
            _presenter.PresentInformationMessage("Couldn't find some of the users selected.");

            return new CommandResult<AddMembersToClubOutputModel>
            {
                Status = CommandResultStatusCode.UsersInListNotFound,
                ModelValidationErrors = null,
                OutputModel = null
            };
        }

        protected virtual CommandResult<AddMembersToClubOutputModel> PresentValidationErrors()
        {
            _presenter.PresentValidationErrors(_loadedClub.ModelValidationErrors);

            return new CommandResult<AddMembersToClubOutputModel>
            {
                Status = CommandResultStatusCode.FailedModelValidation,
                ModelValidationErrors = _loadedClub.ModelValidationErrors,
                OutputModel = null
            };
        }
    }
}

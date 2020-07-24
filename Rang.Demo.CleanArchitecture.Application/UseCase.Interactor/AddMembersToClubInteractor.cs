using Rang.Demo.CleanArchitecture.Application.Common;
using Rang.Demo.CleanArchitecture.Application.Infrastructure.PlugIn;
using Rang.Demo.CleanArchitecture.Application.UseCase.In;
using Rang.Demo.CleanArchitecture.Application.UseCase.In.Boundary;
using Rang.Demo.CleanArchitecture.Application.UseCase.Out;
using Rang.Demo.CleanArchitecture.Application.UseCase.Out.Boundary;
using Rang.Demo.CleanArchitecture.Domain.Entity;
using System;
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
        protected Club _club;

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
            {
                PresentMissingClubResult();
                return new CommandResult<AddMembersToClubOutputModel>
                {
                    Status = CommandResultStatusCode.MissingClub,
                    ModelValidationErrors = null,
                    OutputModel = null
                };
            }

            if(AreMembersToAddMissingFromInput())
            {
                PresentMissingMembersToAddResult();
                return new CommandResult<AddMembersToClubOutputModel>
                {
                    Status = CommandResultStatusCode.MissingMembersToAdd,
                    ModelValidationErrors = null,
                    OutputModel = null
                };
            }

            if(await LoadClubByNameAsync())
            {

            }

            throw new NotImplementedException();
        }

        protected virtual bool IsClubMissingFromInput()
        {
            if(_inputModel.ClubModel == null || 
                ((_inputModel.ClubModel.Id == null || _inputModel.ClubModel.Id == Guid.Empty) && string.IsNullOrWhiteSpace(_inputModel.ClubModel.Name)))
            {
                return true;
            }

            return false;
        }

        protected virtual async Task<bool> LoadClubByNameAsync()
        {
            _club = await _entityGateway.GetClubByNameAsync(_inputModel.ClubModel.Name);
            
            return _club != null;
        }

        protected bool AreMembersToAddMissingFromInput()
        {
            return _inputModel.MemberModelsToAdd == null || !_inputModel.MemberModelsToAdd.Any();
        }

        protected virtual void PresentMissingClubResult()
        {
            _presenter.PresentErrorMessage("A valid Club is required.");
        }

        protected virtual void PresentMissingMembersToAddResult()
        {
            _presenter.PresentErrorMessage(string.Format("No members were selected to add to the {0} club.", _inputModel.ClubModel.Name));
        }
    }
}

using Rang.Demo.CleanArchitecture.Application.Common;
using Rang.Demo.CleanArchitecture.Application.Infrastructure.PlugIn;
using Rang.Demo.CleanArchitecture.Application.UseCase.In;
using Rang.Demo.CleanArchitecture.Application.UseCase.In.Boundary;
using Rang.Demo.CleanArchitecture.Application.UseCase.Out;
using Rang.Demo.CleanArchitecture.Application.UseCase.Out.Boundary;
using Rang.Demo.CleanArchitecture.Domain.Entity;
using System;
using System.Threading.Tasks;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.Interactor
{
    public class AddClubInteractor :
        BaseEntityGatewayInteractor, IAddClubInteractor
    {
        // fields
        protected IAddClubPresenter _presenter;

        // constructor
        public AddClubInteractor(IAddClubPresenter presenter, IEntityGateway entityGateway)
            : base(entityGateway)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        // methods
        public async Task<CommandResult<AddClubOutputModel>> AddClubAsync(AddClubInputModel inputModel)
        {
            if (inputModel == null)
                throw new ArgumentNullException(nameof(inputModel));

            var club = new Club(inputModel.ToClubModel());

            if (!club.IsValid)
            {
                PresentValidationErrors(club);
                return new CommandResult<AddClubOutputModel>
                {
                    Status = CommandResultStatusCode.FailedModelValidation,
                    ModelValidationErrors = club.ModelValidationErrors,
                    OutputModel = null
                };
            }

            if (await IsClubAlreadyStoredAsync(club))
            {
                PresentDuplicatedResult(new AddClubOutputModel(club.GetModel()));
                return new CommandResult<AddClubOutputModel>
                {
                    Status = CommandResultStatusCode.DuplicateEntry,
                    ModelValidationErrors = null,
                    OutputModel = null
                };
            }
            else
            {
                var storedClub = await SaveToStorageAsync(club);
                var storedOutputModel = new AddClubOutputModel(storedClub.GetModel());
                PresentSuccessfulResult(storedOutputModel);
                return new CommandResult<AddClubOutputModel>
                {
                    Status = CommandResultStatusCode.Success,
                    ModelValidationErrors = null,
                    OutputModel = storedOutputModel
                };
            }
        }

        protected virtual void PresentValidationErrors(Club club)
        {
            _presenter.PresentValidationErrors(club.ModelValidationErrors);
        }

        protected virtual async Task<bool> IsClubAlreadyStoredAsync(Club club)
        {
            var existingClub = await _entityGateway.GetClubByNameAsync(club.Name);
            return existingClub != null;
        }

        protected virtual void PresentDuplicatedResult(AddClubOutputModel outputModel)
        {
            _presenter.PresentDuplicatedResult(outputModel);
        }

        protected virtual async Task<Club> SaveToStorageAsync(Club club)
        {
            return await _entityGateway.AddClubAsync(club);
        }

        protected virtual void PresentSuccessfulResult(AddClubOutputModel outputModel)
        {
            _presenter.PresentSuccessfulResult(outputModel);
        }
    }
}

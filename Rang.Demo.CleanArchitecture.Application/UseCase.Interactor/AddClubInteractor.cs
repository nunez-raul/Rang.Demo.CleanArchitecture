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
        protected AddClubInputModel _inputModel;
        protected Club _clubToAdd;

        // constructor
        public AddClubInteractor(IAddClubPresenter presenter, IEntityGateway entityGateway)
            : base(entityGateway)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        // methods
        public async Task<CommandResult<AddClubOutputModel>> AddClubAsync(AddClubInputModel inputModel)
        {
            _inputModel = inputModel ?? throw new ArgumentNullException(nameof(inputModel));
            _clubToAdd = new Club(inputModel.ToClubModel());

            if (!_clubToAdd.IsValid)
                return PresentValidationErrors();
                
            if (await IsClubAlreadyStoredAsync())            
                return PresentDuplicatedResult();
                
            _clubToAdd = await SaveToStorageAsync();
            return PresentSuccessfulResult();
        }

        protected virtual CommandResult<AddClubOutputModel> PresentValidationErrors()
        {
            _presenter.PresentValidationErrors(_clubToAdd.ModelValidationErrors);

            return new CommandResult<AddClubOutputModel>
            {
                Status = CommandResultStatusCode.FailedModelValidation,
                ModelValidationErrors = _clubToAdd.ModelValidationErrors,
                OutputModel = null
            };
        }

        protected virtual async Task<bool> IsClubAlreadyStoredAsync()
        {
            var existingClub = await _entityGateway.GetClubByNameAsync(_clubToAdd.Name);
            return existingClub != null;
        }

        protected virtual CommandResult<AddClubOutputModel> PresentDuplicatedResult()
        {
            var outputModel = new AddClubOutputModel(_clubToAdd.GetModel());

            _presenter.PresentDuplicatedResult(outputModel);

            return new CommandResult<AddClubOutputModel>
            {
                Status = CommandResultStatusCode.DuplicateEntry,
                ModelValidationErrors = null,
                OutputModel = null
            };
        }

        protected virtual async Task<Club> SaveToStorageAsync()
        {
            return await _entityGateway.AddClubAsync(_clubToAdd);
        }

        protected virtual CommandResult<AddClubOutputModel> PresentSuccessfulResult()
        {
            var outputModel = new AddClubOutputModel(_clubToAdd.GetModel());

            _presenter.PresentSuccessfulResult(outputModel);

            return new CommandResult<AddClubOutputModel>
            {
                Status = CommandResultStatusCode.Success,
                ModelValidationErrors = null,
                OutputModel = outputModel
            };
        }
    }
}

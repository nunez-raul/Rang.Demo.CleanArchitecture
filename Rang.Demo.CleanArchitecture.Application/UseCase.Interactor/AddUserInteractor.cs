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
    public class AddUserInteractor :
        BaseEntityGatewayInteractor, IAddUserInteractor
    {
        //fields
        protected IAddUserPresenter _presenter;
        protected AddUserInputModel _inputModel;
        protected User _userToAdd;

        //Constructors
        public AddUserInteractor(IAddUserPresenter presenter, IEntityGateway entityGateway)
            : base(entityGateway)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        //methods
        public async Task<CommandResult<AddUserOutputModel>> AddUserAsync(AddUserInputModel inputModel)
        {
            _inputModel = inputModel ?? throw new ArgumentNullException(nameof(inputModel));
            _userToAdd = new User(inputModel.ToUserModel());

            if (!_userToAdd.IsValid)
                return PresentValidationErrors();
   
            if (await IsUserAlreadyStoredAsync(_userToAdd))
                return PresentDuplicatedResult(new AddUserOutputModel(_userToAdd.GetModel()));

            _userToAdd = await SaveToStorageAsync();
            return PresentSuccessfulResult();
        }

        protected virtual CommandResult<AddUserOutputModel> PresentValidationErrors()
        {
            _presenter.PresentValidationErrors(_userToAdd.ModelValidationErrors);

            return new CommandResult<AddUserOutputModel>
            {
                Status = CommandResultStatusCode.FailedModelValidation,
                ModelValidationErrors = _userToAdd.ModelValidationErrors,
                OutputModel = null
            };
        }

        protected virtual async Task<bool> IsUserAlreadyStoredAsync(User user)
        {
            var existingUser = await _entityGateway.GetUserByUsernameAsync(user.Username);
            return existingUser != null;
        }

        protected virtual CommandResult<AddUserOutputModel> PresentDuplicatedResult(AddUserOutputModel outputModel)
        {
            _presenter.PresentDuplicatedResult(outputModel);

            return new CommandResult<AddUserOutputModel>
            {
                Status = CommandResultStatusCode.DuplicateEntry,
                ModelValidationErrors = null,
                OutputModel = null
            };
        }

        protected virtual async Task<User> SaveToStorageAsync()
        {
            return await _entityGateway.AddUserAsync(_userToAdd);
        }

        protected virtual CommandResult<AddUserOutputModel> PresentSuccessfulResult()
        {
            var outputModel = new AddUserOutputModel(_userToAdd.GetModel());

            _presenter.PresentSuccessfulResult(outputModel);

            return new CommandResult<AddUserOutputModel>
            {
                Status = CommandResultStatusCode.Success,
                ModelValidationErrors = null,
                OutputModel = outputModel
            };
        }
    }
}

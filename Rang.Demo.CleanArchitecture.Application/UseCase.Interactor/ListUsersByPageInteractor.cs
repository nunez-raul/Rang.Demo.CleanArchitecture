using Rang.Demo.CleanArchitecture.Application.Common;
using Rang.Demo.CleanArchitecture.Application.Infrastructure.PlugIn;
using Rang.Demo.CleanArchitecture.Application.UseCase.In;
using Rang.Demo.CleanArchitecture.Application.UseCase.In.Boundary;
using Rang.Demo.CleanArchitecture.Application.UseCase.Out;
using Rang.Demo.CleanArchitecture.Application.UseCase.Out.Boundary;
using Rang.Demo.CleanArchitecture.Domain.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.Interactor
{
    public class ListUsersByPageInteractor :
        BaseEntityGatewayInteractor, IListUsersByPageInteractor
    {
        //fields
        protected IListUserByPagePresenter _presenter;
        protected ListUsersByPageInputModel _inputModel;

        //Constructors
        public ListUsersByPageInteractor(IListUserByPagePresenter presenter, IEntityGateway entityGateway)
            : base(entityGateway)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        //methods
        public async Task<CommandResult<ListUsersByPageOutputModel>> ListUsersByPageAsync(ListUsersByPageInputModel inputModel)
        {
            _inputModel = inputModel ?? throw new ArgumentNullException(nameof(inputModel));
         
            if (_inputModel.UsersPerPage == 0)
                return PresentValidationErrors();
            
            var page = await _entityGateway.GetUsersByPageAsync(_inputModel.PageNumber, _inputModel.UsersPerPage);
            return PresentSuccessfulResult(page);
        }

        protected virtual CommandResult<ListUsersByPageOutputModel> PresentValidationErrors()
        {
            var validationErrors = new Dictionary<ModelValidationStatusCode, List<string>>
                {
                    { ModelValidationStatusCode.InvalidDataSupplied, new List<string>(new string[] { "0 users per page is not a valid page size." }) }
                };

            _presenter.PresentValidationErrors(validationErrors);

            return new CommandResult<ListUsersByPageOutputModel>
            {
                Status = CommandResultStatusCode.FailedModelValidation,
                ModelValidationErrors = validationErrors,
                OutputModel = null
            };
        }

        protected virtual CommandResult<ListUsersByPageOutputModel> PresentSuccessfulResult(Page<Domain.Entity.User> page)
        {
            var outputModel = new ListUsersByPageOutputModel
            {
                Page = page
            };

            _presenter.PresentSuccessfulResult(outputModel);

            return new CommandResult<ListUsersByPageOutputModel>
            {
                Status = CommandResultStatusCode.Success,
                ModelValidationErrors = null,
                OutputModel = outputModel
            };
        }
    }
}

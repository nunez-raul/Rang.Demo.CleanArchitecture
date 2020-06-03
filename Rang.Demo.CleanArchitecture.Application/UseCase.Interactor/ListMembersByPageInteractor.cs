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
    public class ListMembersByPageInteractor :
        BaseEntityGatewayInteractor, IListMembersByPageInteractor
    {
        //fields
        protected IListMembersByPagePresenter _presenter;

        //Constructors
        public ListMembersByPageInteractor(IListMembersByPagePresenter presenter, IEntityGateway entityGateway)
            : base(entityGateway)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        //methods
        public async Task<CommandResult<ListMembersByPageOutputModel>> ListMembersByPageAsync(ListMembersByPageInputModel inputModel)
        {
            if (inputModel == null)
                throw new ArgumentNullException(nameof(inputModel));

            if (inputModel.MembersPerPage == 0)
            {
                var validationErrors = new Dictionary<ModelValidationStatusCode, List<string>>
                {
                    { ModelValidationStatusCode.InvalidDataSupplied, new List<string>(new string[] { "0 members per page is not a valid page size." }) }
                };

                PresentValidationErrors(validationErrors);
                return new CommandResult<ListMembersByPageOutputModel>
                {
                    Status = CommandResultStatusCode.FailedModelValidation,
                    ModelValidationErrors = validationErrors,
                    OutputModel = null
                };
            }

            var page = await _entityGateway.GetMembersByPageAsync(inputModel.PageNumber, inputModel.MembersPerPage);

            var outputModel = new ListMembersByPageOutputModel
            {
                Page = page
            };

            PresentSuccessfulResult(outputModel);
            return new CommandResult<ListMembersByPageOutputModel>
            {
                Status = CommandResultStatusCode.Success,
                ModelValidationErrors = null,
                OutputModel = outputModel
            };
        }

        protected virtual void PresentValidationErrors(Dictionary<ModelValidationStatusCode, List<string>> validationErrors)
        {
            _presenter.PresentValidationErrors(validationErrors);
        }

        protected virtual void PresentSuccessfulResult(ListMembersByPageOutputModel outputModel)
        {
            _presenter.PresentSuccessfulResult(outputModel);
        }
    }
}

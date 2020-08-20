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
        protected ListMembersByPageInputModel _inputModel;

        //Constructors
        public ListMembersByPageInteractor(IListMembersByPagePresenter presenter, IEntityGateway entityGateway)
            : base(entityGateway)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        //methods
        public async Task<CommandResult<ListMembersByPageOutputModel>> ListMembersByPageAsync(ListMembersByPageInputModel inputModel)
        {
            _inputModel = inputModel ?? throw new ArgumentNullException(nameof(inputModel));
         
            if (_inputModel.MembersPerPage == 0)
                return PresentValidationErrors();
            
            var page = await _entityGateway.GetMembersByPageAsync(_inputModel.PageNumber, _inputModel.MembersPerPage);
            return PresentSuccessfulResult(page);
        }

        protected virtual CommandResult<ListMembersByPageOutputModel> PresentValidationErrors()
        {
            var validationErrors = new Dictionary<ModelValidationStatusCode, List<string>>
                {
                    { ModelValidationStatusCode.InvalidDataSupplied, new List<string>(new string[] { "0 members per page is not a valid page size." }) }
                };

            _presenter.PresentValidationErrors(validationErrors);

            return new CommandResult<ListMembersByPageOutputModel>
            {
                Status = CommandResultStatusCode.FailedModelValidation,
                ModelValidationErrors = validationErrors,
                OutputModel = null
            };
        }

        protected virtual CommandResult<ListMembersByPageOutputModel> PresentSuccessfulResult(Page<Domain.Entity.Member> page)
        {
            var outputModel = new ListMembersByPageOutputModel
            {
                Page = page
            };

            _presenter.PresentSuccessfulResult(outputModel);

            return new CommandResult<ListMembersByPageOutputModel>
            {
                Status = CommandResultStatusCode.Success,
                ModelValidationErrors = null,
                OutputModel = outputModel
            };
        }
    }
}

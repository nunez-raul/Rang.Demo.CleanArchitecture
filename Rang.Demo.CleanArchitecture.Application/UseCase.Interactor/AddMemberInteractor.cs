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
    public class AddMemberInteractor :
        BaseEntityGatewayInteractor, IAddMemberInteractor
    {
        //fields
        protected IAddMemberPresenter _presenter;
        protected AddMemberInputModel _inputModel;
        protected Member _memberToAdd;

        //Constructors
        public AddMemberInteractor(IAddMemberPresenter presenter, IEntityGateway entityGateway)
            : base(entityGateway)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        //methods
        public async Task<CommandResult<AddMemberOutputModel>> AddMemberAsync(AddMemberInputModel inputModel)
        {
            _inputModel = inputModel ?? throw new ArgumentNullException(nameof(inputModel));
            _memberToAdd = new Member(inputModel.ToMemberModel());

            if (!_memberToAdd.IsValid)
                return PresentValidationErrors();
   
            if (await IsMemberAlreadyStoredAsync(_memberToAdd))
                return PresentDuplicatedResult(new AddMemberOutputModel(_memberToAdd.GetModel()));

            _memberToAdd = await SaveToStorageAsync();
            return PresentSuccessfulResult();
        }

        protected virtual CommandResult<AddMemberOutputModel> PresentValidationErrors()
        {
            _presenter.PresentValidationErrors(_memberToAdd.ModelValidationErrors);

            return new CommandResult<AddMemberOutputModel>
            {
                Status = CommandResultStatusCode.FailedModelValidation,
                ModelValidationErrors = _memberToAdd.ModelValidationErrors,
                OutputModel = null
            };
        }

        protected virtual async Task<bool> IsMemberAlreadyStoredAsync(Member member)
        {
            var existingMember = await _entityGateway.GetMemberByUsernameAsync(member.Username);
            return existingMember != null;
        }

        protected virtual CommandResult<AddMemberOutputModel> PresentDuplicatedResult(AddMemberOutputModel outputModel)
        {
            _presenter.PresentDuplicatedResult(outputModel);

            return new CommandResult<AddMemberOutputModel>
            {
                Status = CommandResultStatusCode.DuplicateEntry,
                ModelValidationErrors = null,
                OutputModel = null
            };
        }

        protected virtual async Task<Member> SaveToStorageAsync()
        {
            return await _entityGateway.AddMemberAsync(_memberToAdd);
        }

        protected virtual CommandResult<AddMemberOutputModel> PresentSuccessfulResult()
        {
            var outputModel = new AddMemberOutputModel(_memberToAdd.GetModel());

            _presenter.PresentSuccessfulResult(outputModel);

            return new CommandResult<AddMemberOutputModel>
            {
                Status = CommandResultStatusCode.Success,
                ModelValidationErrors = null,
                OutputModel = outputModel
            };
        }
    }
}

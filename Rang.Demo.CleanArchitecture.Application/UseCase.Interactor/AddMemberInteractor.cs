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

        //Constructors
        public AddMemberInteractor(IAddMemberPresenter presenter, IEntityGateway entityGateway)
            : base(entityGateway)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        //methods
        public async Task<CommandResult<AddMemberOutputModel>> AddMemberAsync(AddMemberInputModel inputModel)
        {
            if (inputModel == null)
                throw new ArgumentNullException(nameof(inputModel));

            var member = new Member(inputModel.ToMemberModel());

            if (!member.IsValid)
            {
                PresentValidationErrors(member);
                return new CommandResult<AddMemberOutputModel>
                {
                    Status = CommandResultStatusCode.FailedModelValidation,
                    ModelValidationErrors = member.ModelValidationErrors,
                    OutputModel = null
                };
            }

            if (await IsMemberAlreadyStoredAsync(member))
            {
                PresentDuplicatedResult(new AddMemberOutputModel(member.GetModel()));
                return new CommandResult<AddMemberOutputModel>
                {
                    Status = CommandResultStatusCode.DuplicateEntry,
                    ModelValidationErrors = null,
                    OutputModel = null
                };
            }
            else
            {
                var storedMember = await SaveToStorageAsync(member);
                var storedOutputModel = new AddMemberOutputModel(storedMember.GetModel());
                PresentSuccessfulResult(storedOutputModel);
                return new CommandResult<AddMemberOutputModel>
                {
                    Status = CommandResultStatusCode.Success,
                    ModelValidationErrors = null,
                    OutputModel = storedOutputModel
                };
            }
        }
        protected virtual void PresentValidationErrors(Member member)
        {
            _presenter.PresentValidationErrors(member.ModelValidationErrors);
        }
        protected virtual async Task<bool> IsMemberAlreadyStoredAsync(Member member)
        {
            var existingMember = await _entityGateway.GetMemberByCodenameAsync(member.Codename);
            return existingMember != null;
        }
        protected virtual void PresentDuplicatedResult(AddMemberOutputModel outputModel)
        {
            _presenter.PresentDuplicatedResult(outputModel);
        }
        protected virtual async Task<Member> SaveToStorageAsync(Member member)
        {
            return await _entityGateway.AddMemberAsync(member);
        }
        protected virtual void PresentSuccessfulResult(AddMemberOutputModel outputModel)
        {
            _presenter.PresentSuccessfulResult(outputModel);
        }
    }
}

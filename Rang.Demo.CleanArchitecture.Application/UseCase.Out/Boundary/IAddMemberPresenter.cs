using Rang.Demo.CleanArchitecture.Domain.Common;
using System.Collections.Generic;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.Out.Boundary
{
    public interface IAddMemberPresenter
    {
        void PresentValidationErrors(IDictionary<ModelValidationStatusCode, List<string>> modelValidationErrors);
        void PresentDuplicatedResult(AddMemberOutputModel inputModel);
        void PresentSuccessfulResult(AddMemberOutputModel inputModel);
    }
}

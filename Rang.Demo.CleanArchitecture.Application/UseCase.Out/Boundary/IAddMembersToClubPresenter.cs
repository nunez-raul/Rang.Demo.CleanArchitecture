using Rang.Demo.CleanArchitecture.Domain.Common;
using System.Collections.Generic;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.Out.Boundary
{
    public interface IAddMembersToClubPresenter
    {
        void PresentValidationErrors(IDictionary<ModelValidationStatusCode, List<string>> modelValidationErrors);
        void PresentDuplicatedResult(AddMembersToClubOutputModel outputModel);
        void PresentSuccessfulResult(AddMembersToClubOutputModel outputModel);
        void PresentErrorMessage(string message);
        void PresentWarningMessage(string message);
        void PresentInformationMessage(string message);
    }
}

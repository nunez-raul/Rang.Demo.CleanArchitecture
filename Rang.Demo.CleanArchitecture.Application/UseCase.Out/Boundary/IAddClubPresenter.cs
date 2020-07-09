using Rang.Demo.CleanArchitecture.Domain.Common;
using System.Collections.Generic;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.Out.Boundary
{
    public interface IAddClubPresenter
    {
        void PresentValidationErrors(IDictionary<ModelValidationStatusCode, List<string>> modelValidationErrors);
        void PresentDuplicatedResult(AddClubOutputModel outputModel);
        void PresentSuccessfulResult(AddClubOutputModel outputModel);
    }
}

using Rang.Demo.CleanArchitecture.Domain.Common;
using System.Collections.Generic;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.Out.Boundary
{
    public interface IAddUserPresenter
    {
        void PresentValidationErrors(IDictionary<ModelValidationStatusCode, List<string>> modelValidationErrors);
        void PresentDuplicatedResult(AddUserOutputModel outputModel);
        void PresentSuccessfulResult(AddUserOutputModel outputModel);
    }
}

using Rang.Demo.CleanArchitecture.Domain.Common;
using System.Collections.Generic;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.Out.Boundary
{
    public interface IListUserByPagePresenter
    {
        void PresentValidationErrors(IDictionary<ModelValidationStatusCode, List<string>> modelValidationErrors);
        void PresentSuccessfulResult(ListUsersByPageOutputModel inputModel);
    }
}

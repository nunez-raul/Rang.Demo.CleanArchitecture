using Rang.Demo.CleanArchitecture.Application.Common;
using Rang.Demo.CleanArchitecture.Domain.Common;
using System.Collections.Generic;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.Out
{
    public class CommandResult<TOutputModel>
    {
        public CommandResultStatusCode Status { get; set; }
        public IDictionary<ModelValidationStatusCode, List<string>> ModelValidationErrors { get; set; }
        public TOutputModel OutputModel { get; internal set; }
    }
}

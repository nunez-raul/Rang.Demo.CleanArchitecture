using Rang.Demo.CleanArchitecture.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Abstractions;

namespace Rang.Demo.CleanArchitecture.XUnitTest.TestDoubles
{
    public abstract class BaseFakePresenter
    {
        //fields
        protected readonly ITestOutputHelper _output;

        //constructors
        public BaseFakePresenter(ITestOutputHelper output)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
        }

        public virtual void PresentValidationErrors(IDictionary<ModelValidationStatusCode, List<string>> modelValidationErrors)
        {
            if (modelValidationErrors != null && modelValidationErrors.Any())
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("There are some validation errors:");
                stringBuilder.AppendJoin(Environment.NewLine, modelValidationErrors.SelectMany(kv => kv.Value));

                _output.WriteLine(stringBuilder.ToString());
                return;
            }
        }
    }
}

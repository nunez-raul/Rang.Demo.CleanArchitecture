using Rang.Demo.CleanArchitecture.Application.UseCase.Out;
using Rang.Demo.CleanArchitecture.Application.UseCase.Out.Boundary;
using Xunit.Abstractions;

namespace Rang.Demo.CleanArchitecture.XUnitTest.TestDoubles
{
    public class FakeAddUserPresenter : BaseFakePresenter, IAddUserPresenter
    {
        //constructors
        public FakeAddUserPresenter(ITestOutputHelper output)
            : base(output) { }

        //methods
        public void PresentDuplicatedResult(AddUserOutputModel outputModel)
        {
            _output.WriteLine(string.Format("Couldn't add user with username \"{0}\" because there is another user using that username.", outputModel.Username));
        }

        public void PresentSuccessfulResult(AddUserOutputModel outputModel)
        {
            _output.WriteLine(string.Format("Added new user with username \"{0}\" successfully.", outputModel.Username));
        }
    }
}

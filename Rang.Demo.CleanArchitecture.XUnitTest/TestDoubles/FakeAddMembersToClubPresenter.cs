using Rang.Demo.CleanArchitecture.Application.UseCase.Out;
using Rang.Demo.CleanArchitecture.Application.UseCase.Out.Boundary;
using Xunit.Abstractions;

namespace Rang.Demo.CleanArchitecture.XUnitTest.TestDoubles
{
    public class FakeAddMembersToClubPresenter : BaseFakePresenter, IAddMembersToClubPresenter
    {
        // constructors
        public FakeAddMembersToClubPresenter(ITestOutputHelper output)
            : base(output) { }

        // methods
        public void PresentDuplicatedResult(AddMembersToClubOutputModel outputModel)
        {
            throw new System.NotImplementedException();
        }

        public void PresentSuccessfulResult(AddMembersToClubOutputModel outputModel)
        {
            throw new System.NotImplementedException();
        }

        public void PresentErrorMessage(string message)
        {
            _output.WriteLine(message);
        }

        public void PresentWarningMessage(string message)
        {
            _output.WriteLine(message);
        }

        public void PresentInformationMessage(string message)
        {
            _output.WriteLine(message);
        }
    }
}

using Rang.Demo.CleanArchitecture.Application.UseCase.Out;
using Rang.Demo.CleanArchitecture.Application.UseCase.Out.Boundary;
using Xunit.Abstractions;

namespace Rang.Demo.CleanArchitecture.XUnitTest.TestDoubles
{
    public class FakeAddMemberPresenter : BaseFakePresenter, IAddMemberPresenter
    {
        //constructors
        public FakeAddMemberPresenter(ITestOutputHelper output)
            : base(output) { }

        //methods
        public void PresentDuplicatedResult(AddMemberOutputModel inputModel)
        {
            _output.WriteLine(string.Format("Couldn't add member with username \"{0}\" because there is another member using that username.", inputModel.Username));
        }

        public void PresentSuccessfulResult(AddMemberOutputModel inputModel)
        {
            _output.WriteLine(string.Format("Added new member with username \"{0}\" successfully.", inputModel.Username));
        }
    }
}

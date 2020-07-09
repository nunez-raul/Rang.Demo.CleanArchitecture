using Rang.Demo.CleanArchitecture.Application.UseCase.Out;
using Rang.Demo.CleanArchitecture.Application.UseCase.Out.Boundary;
using Xunit.Abstractions;

namespace Rang.Demo.CleanArchitecture.XUnitTest.TestDoubles
{
    public class FakeAddClubPresenter : BaseFakePresenter, IAddClubPresenter
    {
        //constructors
        public FakeAddClubPresenter(ITestOutputHelper output)
            : base(output) { }

        //methods
        public void PresentDuplicatedResult(AddClubOutputModel outputModel)
        {
            _output.WriteLine(string.Format("Couldn't add club with name \"{0}\" because there is another club using that name.", outputModel.Name));
        }

        public void PresentSuccessfulResult(AddClubOutputModel outputModel)
        {
            _output.WriteLine(string.Format("Added new club with name \"{0}\" successfully.", outputModel.Name));
        }
    }
}

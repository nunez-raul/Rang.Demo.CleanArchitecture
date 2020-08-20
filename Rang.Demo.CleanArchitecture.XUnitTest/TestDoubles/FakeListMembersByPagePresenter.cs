using Rang.Demo.CleanArchitecture.Application.UseCase.Out;
using Rang.Demo.CleanArchitecture.Application.UseCase.Out.Boundary;
using Xunit.Abstractions;

namespace Rang.Demo.CleanArchitecture.XUnitTest.TestDoubles
{
    public class FakeListUsersByPagePresenter : BaseFakePresenter, IListUserByPagePresenter
    {
        //constructors
        public FakeListUsersByPagePresenter(ITestOutputHelper output)
            : base(output) { }

        //methods
        public void PresentSuccessfulResult(ListUsersByPageOutputModel inputModel)
        {
            _output.WriteLine(string.Format("Page {0}/{1} retrieved successfully.", inputModel.Page.PageNumber, inputModel.Page.TotalPages));
            _output.WriteLine(string.Format("This page contains {0}/{1} users.", inputModel.Page.ItemsCount, inputModel.Page.TotalItems));
            _output.WriteLine(" ");
            _output.WriteLine("Users: ");
            foreach (var item in inputModel.Page.Items)
            {
                _output.WriteLine(item.Username);
            }
        }
    }
}

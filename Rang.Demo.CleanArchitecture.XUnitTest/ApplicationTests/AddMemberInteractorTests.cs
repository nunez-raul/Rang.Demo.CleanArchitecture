using Rang.Demo.CleanArchitecture.Application.Infrastructure.PlugIn;
using Rang.Demo.CleanArchitecture.Application.UseCase.In;
using Rang.Demo.CleanArchitecture.Application.UseCase.In.Boundary;
using Rang.Demo.CleanArchitecture.Application.UseCase.Interactor;
using Rang.Demo.CleanArchitecture.Application.UseCase.Out;
using Rang.Demo.CleanArchitecture.Application.UseCase.Out.Boundary;
using Rang.Demo.CleanArchitecture.Domain.Entity;
using Rang.Demo.CleanArchitecture.XUnitTest.TestDoubles;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Rang.Demo.CleanArchitecture.XUnitTest.ApplicationTests
{
    public class AddMemberInteractorTests
    {
        //fields
        private readonly ITestOutputHelper _output;

        //constructors
        public AddMemberInteractorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        //methods
        [Fact]
        public async Task CreateInteractor_ThrowsException_NullGateway()
        {
            //arrange
            IEntityGateway entityGateway = null;
            IAddMemberPresenter presenter = new FakeAddMemberPresenter(_output);

            //act
            async Task<AddMemberInteractor> function() => await Task<AddMemberInteractor>.Factory.StartNew(() => new AddMemberInteractor(presenter, entityGateway));

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task CreateInteractor_ThrowsException_NullPresenter()
        {
            //arrange
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddMemberPresenter presenter = null;

            //act
            async Task<AddMemberInteractor> function() => await Task<AddMemberInteractor>.Factory.StartNew(() => new AddMemberInteractor(presenter, entityGateway));

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task CreateInteractor_Success()
        {
            //arrange
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddMemberPresenter presenter = new FakeAddMemberPresenter(_output);

            //act
            var interactor = await Task<AddMemberInteractor>.Factory.StartNew(() => new AddMemberInteractor(presenter, entityGateway));

            //assert
            Assert.NotNull(interactor);
        }

        [Fact]
        public async Task AddMemberAsync_ThrowsException_NullInput()
        {
            //arrange
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddMemberPresenter presenter = new FakeAddMemberPresenter(_output);
            IAddMemberInteractor interactor = new AddMemberInteractor(presenter, entityGateway);
            AddMemberInputModel inputModel = null;

            //act
            async Task<CommandResult<Application.UseCase.Out.AddMemberOutputModel>> function() => await interactor.AddMemberAsync(inputModel);

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task AddMemberAsync_FailedModelValidation_NullCodename()
        {
            //arrange
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddMemberPresenter presenter = new FakeAddMemberPresenter(_output);
            IAddMemberInteractor interactor = new AddMemberInteractor(presenter, entityGateway);
            AddMemberInputModel inputModel = new AddMemberInputModel();

            //act
            var result = await interactor.AddMemberAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.FailedModelValidation);
            Assert.True(result.ModelValidationErrors.ContainsKey(Domain.Common.ModelValidationStatusCode.RequiredInformationMissing));
        }

        [Fact]
        public async Task AddMemberAsync_CommandResult_DuplicateMember()
        {
            //arrange
            var members = new Member[] { new Member { Codename = "blacksheep" } };
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(members);
            IAddMemberPresenter presenter = new FakeAddMemberPresenter(_output);
            IAddMemberInteractor interactor = new AddMemberInteractor(presenter, entityGateway);
            AddMemberInputModel inputModel = new AddMemberInputModel { Codename = "blacksheep" };

            //act
            var result = await interactor.AddMemberAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.DuplicateEntry);
        }

        [Fact]
        public async Task AddMemberAsync_CommandResult_Success()
        {
            //arrange
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddMemberPresenter presenter = new FakeAddMemberPresenter(_output);
            IAddMemberInteractor interactor = new AddMemberInteractor(presenter, entityGateway);
            AddMemberInputModel inputModel = new AddMemberInputModel { Codename = "blacksheep" };

            //act
            var result = await interactor.AddMemberAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.Success);
            Assert.Null(result.ModelValidationErrors);
            Assert.NotNull(result.OutputModel);
            Assert.True(result.OutputModel.Id != Guid.Empty);
        }
    }
}

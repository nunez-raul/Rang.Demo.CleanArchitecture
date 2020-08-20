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
    public class AddUserInteractorTests
    {
        //fields
        private readonly ITestOutputHelper _output;

        //constructors
        public AddUserInteractorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        //methods
        [Fact]
        public void CreateInteractor_ThrowsException_NullGateway()
        {
            //arrange
            IEntityGateway entityGateway = null;
            IAddUserPresenter presenter = new FakeAddUserPresenter(_output);

            //act
            Action action = () => new AddUserInteractor(presenter, entityGateway);

            //assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void CreateInteractor_ThrowsException_NullPresenter()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddUserPresenter presenter = null;

            //act
            Action action = () => new AddUserInteractor(presenter, entityGateway);

            //assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void CreateInteractor_Success()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddUserPresenter presenter = new FakeAddUserPresenter(_output);

            //act
            var interactor = new AddUserInteractor(presenter, entityGateway);

            //assert
            Assert.NotNull(interactor);
        }

        [Fact]
        public async Task AddUserAsync_ThrowsException_NullInput()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddUserPresenter presenter = new FakeAddUserPresenter(_output);
            IAddUserInteractor interactor = new AddUserInteractor(presenter, entityGateway);
            AddUserInputModel inputModel = null;

            //act
            async Task<CommandResult<Application.UseCase.Out.AddUserOutputModel>> function() => await interactor.AddUserAsync(inputModel);

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task AddUserAsync_FailedModelValidation_NullUsername()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddUserPresenter presenter = new FakeAddUserPresenter(_output);
            IAddUserInteractor interactor = new AddUserInteractor(presenter, entityGateway);
            AddUserInputModel inputModel = new AddUserInputModel();

            //act
            var result = await interactor.AddUserAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.FailedModelValidation);
            Assert.True(result.ModelValidationErrors.ContainsKey(Domain.Common.ModelValidationStatusCode.RequiredInformationMissing));
        }

        [Fact]
        public async Task AddUserAsyncExistingUserUsernameSameCase_CommandResult_DuplicateEntry()
        {
            //arrange
            string duplicatedUsername = "blacksheep";
            var users = new User[] { new User { Username = duplicatedUsername } };
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(users);
            IAddUserPresenter presenter = new FakeAddUserPresenter(_output);
            IAddUserInteractor interactor = new AddUserInteractor(presenter, entityGateway);
            AddUserInputModel inputModel = new AddUserInputModel { Username = duplicatedUsername };

            //act
            var result = await interactor.AddUserAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.DuplicateEntry);
        }

        [Fact]
        public async Task AddUserAsyncExistingUserUsernameDifferentCase_CommandResult_DuplicateEntry()
        {
            //arrange
            string duplicatedUsername = "blacksheep";
            var users = new User[] { new User { Username = duplicatedUsername } };
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(users);
            IAddUserPresenter presenter = new FakeAddUserPresenter(_output);
            IAddUserInteractor interactor = new AddUserInteractor(presenter, entityGateway);
            AddUserInputModel inputModel = new AddUserInputModel { Username = duplicatedUsername.ToUpperInvariant() };

            //act
            var result = await interactor.AddUserAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.DuplicateEntry);
        }

        [Fact]
        public async Task AddUserAsync_CommandResult_Success()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddUserPresenter presenter = new FakeAddUserPresenter(_output);
            IAddUserInteractor interactor = new AddUserInteractor(presenter, entityGateway);
            AddUserInputModel inputModel = new AddUserInputModel { Username = "blacksheep" };

            //act
            var result = await interactor.AddUserAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.Success);
            Assert.Null(result.ModelValidationErrors);
            Assert.NotNull(result.OutputModel);
            Assert.True(result.OutputModel.Id != Guid.Empty);
        }
    }
}

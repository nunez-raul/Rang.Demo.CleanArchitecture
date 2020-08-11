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
    public class AddClubInteractorTests
    {
        //fields
        private readonly ITestOutputHelper _output;

        //constructors
        public AddClubInteractorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        //methods
        [Fact]
        public void CreateInteractor_ThrowsException_NullGateway()
        {
            //arrange
            IEntityGateway entityGateway = null;
            IAddClubPresenter presenter = new FakeAddClubPresenter(_output);

            //act
            Action action = () => new AddClubInteractor(presenter, entityGateway);

            //assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void CreateInteractor_ThrowsException_NullPresenter()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddClubPresenter presenter = null;

            //act
            Action action = () => new AddClubInteractor(presenter, entityGateway);

            //assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void CreateInteractor_Success()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddClubPresenter presenter = new FakeAddClubPresenter(_output);

            //act
            var interactor = new AddClubInteractor(presenter, entityGateway);

            //assert
            Assert.NotNull(interactor);
        }

        [Fact]
        public async Task AddClubAsync_ThrowsException_NullInput()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddClubPresenter presenter = new FakeAddClubPresenter(_output);
            IAddClubInteractor interactor = new AddClubInteractor(presenter, entityGateway);
            AddClubInputModel inputModel = null;

            //act
            async Task<CommandResult<AddClubOutputModel>> function() => await interactor.AddClubAsync(inputModel);

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task AddClubAsync_FailedModelValidation_NullName()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddClubPresenter presenter = new FakeAddClubPresenter(_output);
            IAddClubInteractor interactor = new AddClubInteractor(presenter, entityGateway);
            AddClubInputModel inputModel = new AddClubInputModel();

            //act
            var result = await interactor.AddClubAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.FailedModelValidation);
            Assert.True(result.ModelValidationErrors.ContainsKey(Domain.Common.ModelValidationStatusCode.RequiredInformationMissing));
        }

        [Fact]
        public async Task AddClubAsyncExistingClubNameSameCase_CommandResult_DuplicateEntry()
        {
            //arrange
            string duplicatedClubName = "C# Knights";
            var clubsToPreload = new Club[] { new Club { Name = duplicatedClubName } };
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(clubsToPreload);
            IAddClubPresenter presenter = new FakeAddClubPresenter(_output);
            IAddClubInteractor interactor = new AddClubInteractor(presenter, entityGateway);
            AddClubInputModel inputModel = new AddClubInputModel { Name = duplicatedClubName };

            //act
            var result = await interactor.AddClubAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.DuplicateEntry);
        }

        [Fact]
        public async Task AddClubAsyncExistingClubNameDifferentCase_CommandResult_DuplicateEntry()
        {
            //arrange
            string duplicatedClubName = "C# Knights";
            var clubsToPreload = new Club[] { new Club { Name = duplicatedClubName } };
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(clubsToPreload);
            IAddClubPresenter presenter = new FakeAddClubPresenter(_output);
            IAddClubInteractor interactor = new AddClubInteractor(presenter, entityGateway);
            AddClubInputModel inputModel = new AddClubInputModel { Name = duplicatedClubName.ToLowerInvariant() };

            //act
            var result = await interactor.AddClubAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.DuplicateEntry);
        }

        [Fact]
        public async Task AddClubAsync_CommandResult_Success()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddClubPresenter presenter = new FakeAddClubPresenter(_output);
            IAddClubInteractor interactor = new AddClubInteractor(presenter, entityGateway);
            AddClubInputModel inputModel = new AddClubInputModel { Name = "C# Knights" };

            //act
            var result = await interactor.AddClubAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.Success);
            Assert.Null(result.ModelValidationErrors);
            Assert.NotNull(result.OutputModel);
            Assert.True(result.OutputModel.Id != Guid.Empty);
        }
    }
}

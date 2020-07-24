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
    public class AddMembersToClubInteractorTests
    {
        // fields
        private readonly ITestOutputHelper _output;

        // constructors
        public AddMembersToClubInteractorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        // methods
        [Fact]
        public async Task CreateInteractor_ThrowsException_NullGateway()
        {
            //arrange
            IEntityGateway entityGateway = null;
            IAddMembersToClubPresenter presenter = new FakeAddMembersToClubPresenter(_output);

            //act
            async Task<AddMembersToClubInteractor> function() => await Task<AddMembersToClubInteractor>.Factory.StartNew(() => new AddMembersToClubInteractor(presenter, entityGateway));

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task CreateInteractor_ThrowsException_NullPresenter()
        {
            //arrange
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddMembersToClubPresenter presenter = null;

            //act
            async Task<AddMembersToClubInteractor> function() => await Task<AddMembersToClubInteractor>.Factory.StartNew(() => new AddMembersToClubInteractor(presenter, entityGateway));

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task CreateInteractor_Success()
        {
            //arrange
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddMembersToClubPresenter presenter = new FakeAddMembersToClubPresenter(_output);

            //act
            var interactor = await Task<AddMembersToClubInteractor>.Factory.StartNew(() => new AddMembersToClubInteractor(presenter, entityGateway));

            //assert
            Assert.NotNull(interactor);
        }

        [Fact]
        public async Task AddMembersToClubAsync_ThrowsException_NullInput()
        {
            //arrange
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddMembersToClubPresenter presenter = new FakeAddMembersToClubPresenter(_output);
            IAddMembersToClubInteractor interactor = new AddMembersToClubInteractor(presenter, entityGateway);
            AddMembersToClubInputModel inputModel = null;

            //act
            async Task<CommandResult<AddMembersToClubOutputModel>> function() => await interactor.AddMembersToClubAsync(inputModel);

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task AddMembersToClubAsync_CommandResult_MissingClub()
        {
            //arrange
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddMembersToClubPresenter presenter = new FakeAddMembersToClubPresenter(_output);
            IAddMembersToClubInteractor interactor = new AddMembersToClubInteractor(presenter, entityGateway);
            AddMembersToClubInputModel inputModel = new AddMembersToClubInputModel();

            //act
            var result = await interactor.AddMembersToClubAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.MissingClub);
        }

        [Fact]
        public async Task AddMembersToClubAsync_CommandResult_MissingClub2()
        {
            //arrange
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddMembersToClubPresenter presenter = new FakeAddMembersToClubPresenter(_output);
            IAddMembersToClubInteractor interactor = new AddMembersToClubInteractor(presenter, entityGateway);
            AddMembersToClubInputModel inputModel = new AddMembersToClubInputModel { ClubModel = new Domain.Model.ClubModel()};

            //act
            var result = await interactor.AddMembersToClubAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.MissingClub);
        }

        [Fact]
        public async Task AddMembersToClubAsync_CommandResult_MissingMembersToAdd()
        {
            //arrange
            string existingClubName = "C# Knights";
            var clubsToPreload = new Club[] { new Club { Name = existingClubName } };
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(clubsToPreload);
            IAddMembersToClubPresenter presenter = new FakeAddMembersToClubPresenter(_output);
            IAddMembersToClubInteractor interactor = new AddMembersToClubInteractor(presenter, entityGateway);
            AddMembersToClubInputModel inputModel = new AddMembersToClubInputModel { ClubModel = new Domain.Model.ClubModel { Name = existingClubName } };

            //act
            var result = await interactor.AddMembersToClubAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.MissingMembersToAdd);
        }
        [Fact]
        public async Task AddMembersToClubAsync_CommandResult_MissingMembersToAdd2()
        {
            //arrange
            string existingClubName = "C# Knights";
            var clubsToPreload = new Club[] { new Club { Name = existingClubName } };
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(clubsToPreload);
            IAddMembersToClubPresenter presenter = new FakeAddMembersToClubPresenter(_output);
            IAddMembersToClubInteractor interactor = new AddMembersToClubInteractor(presenter, entityGateway);
            AddMembersToClubInputModel inputModel = new AddMembersToClubInputModel { ClubModel = new Domain.Model.ClubModel { Name = existingClubName }, MemberModelsToAdd = null };

            //act
            var result = await interactor.AddMembersToClubAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.MissingMembersToAdd);
        }
    }
}

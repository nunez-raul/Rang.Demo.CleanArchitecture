using Rang.Demo.CleanArchitecture.Application.Infrastructure.PlugIn;
using Rang.Demo.CleanArchitecture.Application.UseCase.In;
using Rang.Demo.CleanArchitecture.Application.UseCase.In.Boundary;
using Rang.Demo.CleanArchitecture.Application.UseCase.Interactor;
using Rang.Demo.CleanArchitecture.Application.UseCase.Out;
using Rang.Demo.CleanArchitecture.Application.UseCase.Out.Boundary;
using Rang.Demo.CleanArchitecture.Domain.Entity;
using Rang.Demo.CleanArchitecture.XUnitTest.TestDoubles;
using System;
using System.Linq;
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
        public void CreateInteractor_ThrowsException_NullGateway()
        {
            //arrange
            IEntityGateway entityGateway = null;
            IAddMembersToClubPresenter presenter = new FakeAddMembersToClubPresenter(_output);

            //act
            Action action = () => new AddMembersToClubInteractor(presenter, entityGateway);

            //assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void CreateInteractor_ThrowsException_NullPresenter()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddMembersToClubPresenter presenter = null;

            //act
            Action action = () => new AddMembersToClubInteractor(presenter, entityGateway);

            //assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void CreateInteractor_Success()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddMembersToClubPresenter presenter = new FakeAddMembersToClubPresenter(_output);

            //act
            var interactor = new AddMembersToClubInteractor(presenter, entityGateway);

            //assert
            Assert.NotNull(interactor);
        }

        [Fact]
        public async Task AddMembersToClubAsync_ThrowsException_NullInput()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
            IAddMembersToClubPresenter presenter = new FakeAddMembersToClubPresenter(_output);
            IAddMembersToClubInteractor interactor = new AddMembersToClubInteractor(presenter, entityGateway);
            AddMembersToClubInputModel inputModel = null;

            //act
            async Task<CommandResult<AddMembersToClubOutputModel>> function() => await interactor.AddMembersToClubAsync(inputModel);

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task AddMembersToClubAsync_CommandResult_MissingClubFromInput1()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
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
        public async Task AddMembersToClubAsync_CommandResult_MissingClubFromInput2()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
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
        public async Task AddMembersToClubAsync_CommandResult_ClubNotFoundById()
        {
            //arrange
            var nonExistingClubId = Guid.NewGuid();
            string existingMemberUsername = "blacksheep";
            var members = new User[] { new User { Username = existingMemberUsername } };
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(members);
            IAddMembersToClubPresenter presenter = new FakeAddMembersToClubPresenter(_output);
            IAddMembersToClubInteractor interactor = new AddMembersToClubInteractor(presenter, entityGateway);
            AddMembersToClubInputModel inputModel = new AddMembersToClubInputModel { ClubModel = new Domain.Model.ClubModel { Id = nonExistingClubId }, UserModelsToAdd = members.Select(member => member.GetModel()).ToList() };

            //act
            var result = await interactor.AddMembersToClubAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.ClubNotFound);
        }

        [Fact]
        public async Task AddMembersToClubAsync_CommandResult_ClubNotFoundByName()
        {
            //arrange
            string nonExistingClubName = "C# Knights";
            string existingMemberUsername = "blacksheep";
            var members = new User[] { new User { Username = existingMemberUsername } };
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(members);
            IAddMembersToClubPresenter presenter = new FakeAddMembersToClubPresenter(_output);
            IAddMembersToClubInteractor interactor = new AddMembersToClubInteractor(presenter, entityGateway);
            AddMembersToClubInputModel inputModel = new AddMembersToClubInputModel { ClubModel = new Domain.Model.ClubModel { Name = nonExistingClubName }, UserModelsToAdd = members.Select(member => member.GetModel()).ToList() };

            //act
            var result = await interactor.AddMembersToClubAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.ClubNotFound);
        }

        [Fact]
        public async Task AddMembersToClubAsync_CommandResult_MissingUsersToAddFromInput1()
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
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.MissingUsersToAdd);
        }

        [Fact]
        public async Task AddMembersToClubAsync_CommandResult_MissingUsersToAddFromInput2()
        {
            //arrange
            string existingClubName = "C# Knights";
            var clubsToPreload = new Club[] { new Club { Name = existingClubName } };
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(clubsToPreload);
            IAddMembersToClubPresenter presenter = new FakeAddMembersToClubPresenter(_output);
            IAddMembersToClubInteractor interactor = new AddMembersToClubInteractor(presenter, entityGateway);
            AddMembersToClubInputModel inputModel = new AddMembersToClubInputModel { ClubModel = new Domain.Model.ClubModel { Name = existingClubName }, UserModelsToAdd = null };

            //act
            var result = await interactor.AddMembersToClubAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.MissingUsersToAdd);
        }

        [Fact]
        public async Task AddMembersToClubAsync_CommandResult_UsersToAddNotFoundById()
        {
            //arrange
            string existingClubName = "C# Knights";
            var clubsToPreload = new Club[] { new Club { Name = existingClubName } };
            var members = new User[] { new User( new Domain.Model.UserModel{ Id = Guid.NewGuid() }) };
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(clubsToPreload);
            IAddMembersToClubPresenter presenter = new FakeAddMembersToClubPresenter(_output);
            IAddMembersToClubInteractor interactor = new AddMembersToClubInteractor(presenter, entityGateway);
            AddMembersToClubInputModel inputModel = new AddMembersToClubInputModel { ClubModel = new Domain.Model.ClubModel { Name = existingClubName }, UserModelsToAdd = members.Select(member => member.GetModel()).ToList() };

            //act
            var result = await interactor.AddMembersToClubAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.UsersInListNotFound);
        }

        [Fact]
        public async Task AddMembersToClubAsync_CommandResult_UsersToAddNotFoundByUsername()
        {
            //arrange
            string existingClubName = "C# Knights";
            var clubsToPreload = new Club[] { new Club { Name = existingClubName } };
            var members = new User[] { new User { Username = "whitesheep" } };
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(clubsToPreload);
            IAddMembersToClubPresenter presenter = new FakeAddMembersToClubPresenter(_output);
            IAddMembersToClubInteractor interactor = new AddMembersToClubInteractor(presenter, entityGateway);
            AddMembersToClubInputModel inputModel = new AddMembersToClubInputModel { ClubModel = new Domain.Model.ClubModel { Name = existingClubName }, UserModelsToAdd = members.Select(member => member.GetModel()).ToList() };

            //act
            var result = await interactor.AddMembersToClubAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.UsersInListNotFound);
        }
    }
}

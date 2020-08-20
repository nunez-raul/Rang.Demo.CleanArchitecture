using Rang.Demo.CleanArchitecture.Application.Infrastructure.PlugIn;
using Rang.Demo.CleanArchitecture.Application.UseCase.In;
using Rang.Demo.CleanArchitecture.Application.UseCase.In.Boundary;
using Rang.Demo.CleanArchitecture.Application.UseCase.Interactor;
using Rang.Demo.CleanArchitecture.Application.UseCase.Out;
using Rang.Demo.CleanArchitecture.Application.UseCase.Out.Boundary;
using Rang.Demo.CleanArchitecture.Domain.Entity;
using Rang.Demo.CleanArchitecture.XUnitTest.TestDoubles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Rang.Demo.CleanArchitecture.XUnitTest.ApplicationTests
{
    public class ListUsersByPageInteractorTests
    {
        //fields
        private readonly ITestOutputHelper _output;

        //constructors
        public ListUsersByPageInteractorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        //methods
        [Fact]
        public void CreateInteractor_ThrowsException_NullGateway()
        {
            //arrange
            IEntityGateway entityGateway = null;
            IListUserByPagePresenter presenter = new FakeListUsersByPagePresenter(_output);

            //act
            Action action = () => new ListUsersByPageInteractor(presenter, entityGateway);

            //assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void CreateInteractor_ThrowsException_NullPresenter()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
            IListUserByPagePresenter presenter = null;

            //act
            Action action = () => new ListUsersByPageInteractor(presenter, entityGateway);

            //assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void CreateInteractor_Success()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
            IListUserByPagePresenter presenter = new FakeListUsersByPagePresenter(_output);

            //act
            IListUsersByPageInteractor interactor = new ListUsersByPageInteractor(presenter, entityGateway);

            //assert
            Assert.NotNull(interactor);
        }

        [Fact]
        public async Task ListUsersByPageAsync_ThrowsException_NullInput()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
            IListUserByPagePresenter presenter = new FakeListUsersByPagePresenter(_output);
            IListUsersByPageInteractor interactor = new ListUsersByPageInteractor(presenter, entityGateway);
            ListUsersByPageInputModel inputModel = null;

            //act
            async Task<CommandResult<ListUsersByPageOutputModel>> function() => await interactor.ListUsersByPageAsync(inputModel);

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task ListUsersByPageAsync_FailedModelValidation_ZeroUsersPerPage()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
            IListUserByPagePresenter presenter = new FakeListUsersByPagePresenter(_output);
            IListUsersByPageInteractor interactor = new ListUsersByPageInteractor(presenter, entityGateway);
            ListUsersByPageInputModel inputModel = new ListUsersByPageInputModel { PageNumber = 1, UsersPerPage = 0 };

            //act
            var result = await interactor.ListUsersByPageAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.FailedModelValidation);
            Assert.True(result.ModelValidationErrors.ContainsKey(Domain.Common.ModelValidationStatusCode.InvalidDataSupplied));
        }

        [Fact]
        public async Task ListUsersByPageAsync_Success_EmptyPage()
        {
            //arrange
            IEntityGateway entityGateway = InMemoryEntityGatewayFactory.CreateEntityGateway();
            IListUserByPagePresenter presenter = new FakeListUsersByPagePresenter(_output);
            IListUsersByPageInteractor interactor = new ListUsersByPageInteractor(presenter, entityGateway);
            ListUsersByPageInputModel inputModel = new ListUsersByPageInputModel { PageNumber = 1, UsersPerPage = 50 };

            //act
            var result = await interactor.ListUsersByPageAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.Success);
            Assert.Null(result.ModelValidationErrors);
            Assert.NotNull(result.OutputModel);
            Assert.NotNull(result.OutputModel.Page);
            Assert.True(result.OutputModel.Page.ItemsCount == 0);
        }

        [Fact]
        public async Task ListUsersByPageAsync_Success_GetPage0()
        {
            //arrange
            var users = new List<User>();
            for (int i = 1; i < 100; i++)
            {
                users.Add(new User { Username = string.Format("Agent{0}", i) });
            }
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(users.ToArray());
            IListUserByPagePresenter presenter = new FakeListUsersByPagePresenter(_output);
            IListUsersByPageInteractor interactor = new ListUsersByPageInteractor(presenter, entityGateway);
            ListUsersByPageInputModel inputModel = new ListUsersByPageInputModel { PageNumber = 0, UsersPerPage = 25 };

            //act
            var result = await interactor.ListUsersByPageAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.Success);
            Assert.Null(result.ModelValidationErrors);
            Assert.NotNull(result.OutputModel);
            Assert.NotNull(result.OutputModel.Page);
            Assert.True(result.OutputModel.Page.ItemsCount == 0, "Item Count");
            Assert.True(result.OutputModel.Page.TotalItems == 99, "Total Items");
            Assert.True(result.OutputModel.Page.TotalPages == 4, "Total Pages");
        }

        [Fact]
        public async Task ListUsersByPageAsync_Success_GetFirstPage()
        {
            //arrange
            var users = new List<User>();
            for (int i = 1; i < 100; i++)
            {
                users.Add(new User { Username = string.Format("Agent{0}", i) });
            }
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(users.ToArray());
            IListUserByPagePresenter presenter = new FakeListUsersByPagePresenter(_output);
            IListUsersByPageInteractor interactor = new ListUsersByPageInteractor(presenter, entityGateway);
            ListUsersByPageInputModel inputModel = new ListUsersByPageInputModel { PageNumber = 1, UsersPerPage = 25 };

            //act
            var result = await interactor.ListUsersByPageAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.Success);
            Assert.Null(result.ModelValidationErrors);
            Assert.NotNull(result.OutputModel);
            Assert.NotNull(result.OutputModel.Page);
            Assert.True(result.OutputModel.Page.ItemsCount == 25, "Item Count");
            Assert.True(result.OutputModel.Page.TotalItems == 99, "Total Items");
            Assert.True(result.OutputModel.Page.TotalPages == 4, "Total Pages");
        }

        [Fact]
        public async Task ListUsersByPageAsync_Success_GetMiddlePage()
        {
            //arrange
            var users = new List<User>();
            for (int i = 1; i < 100; i++)
            {
                users.Add(new User { Username = string.Format("Agent{0}", i) });
            }
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(users.ToArray());
            IListUserByPagePresenter presenter = new FakeListUsersByPagePresenter(_output);
            IListUsersByPageInteractor interactor = new ListUsersByPageInteractor(presenter, entityGateway);
            ListUsersByPageInputModel inputModel = new ListUsersByPageInputModel { PageNumber = 2, UsersPerPage = 25 };

            //act
            var result = await interactor.ListUsersByPageAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.Success);
            Assert.Null(result.ModelValidationErrors);
            Assert.NotNull(result.OutputModel);
            Assert.NotNull(result.OutputModel.Page);
            Assert.True(result.OutputModel.Page.ItemsCount == 25, "Item Count");
            Assert.True(result.OutputModel.Page.TotalItems == 99, "Total Items");
            Assert.True(result.OutputModel.Page.TotalPages == 4, "Total Pages");
        }

        [Fact]
        public async Task ListUsersByPageAsync_Success_GetLastPage()
        {
            //arrange
            var Users = new List<User>();
            for (int i = 1; i < 100; i++)
            {
                Users.Add(new User { Username = string.Format("Agent{0}", i) });
            }
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(Users.ToArray());
            IListUserByPagePresenter presenter = new FakeListUsersByPagePresenter(_output);
            IListUsersByPageInteractor interactor = new ListUsersByPageInteractor(presenter, entityGateway);
            ListUsersByPageInputModel inputModel = new ListUsersByPageInputModel { PageNumber = 4, UsersPerPage = 25 };

            //act
            var result = await interactor.ListUsersByPageAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.Success);
            Assert.Null(result.ModelValidationErrors);
            Assert.NotNull(result.OutputModel);
            Assert.NotNull(result.OutputModel.Page);
            Assert.True(result.OutputModel.Page.ItemsCount == 24, "Item Count");
            Assert.True(result.OutputModel.Page.TotalItems == 99, "Total Items");
            Assert.True(result.OutputModel.Page.TotalPages == 4, "Total Pages");
        }

        [Fact]
        public async Task ListUsersByPageAsync_Success_GetLastPageNextPage()
        {
            //arrange
            var Users = new List<User>();
            for (int i = 1; i < 100; i++)
            {
                Users.Add(new User { Username = string.Format("Agent{0}", i) });
            }
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(Users.ToArray());
            IListUserByPagePresenter presenter = new FakeListUsersByPagePresenter(_output);
            IListUsersByPageInteractor interactor = new ListUsersByPageInteractor(presenter, entityGateway);
            ListUsersByPageInputModel inputModel = new ListUsersByPageInputModel { PageNumber = 5, UsersPerPage = 25 };

            //act
            var result = await interactor.ListUsersByPageAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.Success);
            Assert.Null(result.ModelValidationErrors);
            Assert.NotNull(result.OutputModel);
            Assert.NotNull(result.OutputModel.Page);
            Assert.True(result.OutputModel.Page.ItemsCount == 0, "Item Count");
            Assert.True(result.OutputModel.Page.TotalItems == 99, "Total Items");
            Assert.True(result.OutputModel.Page.TotalPages == 4, "Total Pages");
        }
    }
}

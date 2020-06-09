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
    public class ListMembersByPageInteractorTests
    {
        //fields
        private readonly ITestOutputHelper _output;

        //constructors
        public ListMembersByPageInteractorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        //methods
        [Fact]
        public async Task CreateInteractor_ThrowsException_NullGateway()
        {
            //arrange
            IEntityGateway entityGateway = null;
            IListMembersByPagePresenter presenter = new FakeListMembersByPagePresenter(_output);

            //act
            async Task<ListMembersByPageInteractor> function() => await Task<ListMembersByPageInteractor>.Factory.StartNew(() => new ListMembersByPageInteractor(presenter, entityGateway));

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task CreateInteractor_ThrowsException_NullPresenter()
        {
            //arrange
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGateway();
            IListMembersByPagePresenter presenter = null;

            //act
            async Task<ListMembersByPageInteractor> function() => await Task<ListMembersByPageInteractor>.Factory.StartNew(() => new ListMembersByPageInteractor(presenter, entityGateway));

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task CreateInteractor_Success()
        {
            //arrange
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGateway();
            IListMembersByPagePresenter presenter = new FakeListMembersByPagePresenter(_output);

            //act
            IListMembersByPageInteractor interactor = await Task<ListMembersByPageInteractor>.Factory.StartNew(() => new ListMembersByPageInteractor(presenter, entityGateway));

            //assert
            Assert.NotNull(interactor);
        }

        [Fact]
        public async Task ListMembersByPageAsync_ThrowsException_NullInput()
        {
            //arrange
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGateway();
            IListMembersByPagePresenter presenter = new FakeListMembersByPagePresenter(_output);
            IListMembersByPageInteractor interactor = new ListMembersByPageInteractor(presenter, entityGateway);
            ListMembersByPageInputModel inputModel = null;

            //act
            async Task<CommandResult<ListMembersByPageOutputModel>> function() => await interactor.ListMembersByPageAsync(inputModel);

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task ListMembersByPageAsync_FailedModelValidation_ZeroMembersPerPage()
        {
            //arrange
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGateway();
            IListMembersByPagePresenter presenter = new FakeListMembersByPagePresenter(_output);
            IListMembersByPageInteractor interactor = new ListMembersByPageInteractor(presenter, entityGateway);
            ListMembersByPageInputModel inputModel = new ListMembersByPageInputModel { PageNumber = 1, MembersPerPage = 0 };

            //act
            var result = await interactor.ListMembersByPageAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.FailedModelValidation);
            Assert.True(result.ModelValidationErrors.ContainsKey(Domain.Common.ModelValidationStatusCode.InvalidDataSupplied));
        }

        [Fact]
        public async Task ListMembersByPageAsync_Success_EmptyPage()
        {
            //arrange
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGateway();
            IListMembersByPagePresenter presenter = new FakeListMembersByPagePresenter(_output);
            IListMembersByPageInteractor interactor = new ListMembersByPageInteractor(presenter, entityGateway);
            ListMembersByPageInputModel inputModel = new ListMembersByPageInputModel { PageNumber = 1, MembersPerPage = 50 };

            //act
            var result = await interactor.ListMembersByPageAsync(inputModel);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Status == Application.Common.CommandResultStatusCode.Success);
            Assert.Null(result.ModelValidationErrors);
            Assert.NotNull(result.OutputModel);
            Assert.NotNull(result.OutputModel.Page);
            Assert.True(result.OutputModel.Page.ItemsCount == 0);
        }

        [Fact]
        public async Task ListMembersByPageAsync_Success_GetPage0()
        {
            //arrange
            var members = new List<Member>();
            for (int i = 1; i < 100; i++)
            {
                members.Add(new Member { Username = string.Format("Agent{0}", i) });
            }
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(members.ToArray());
            IListMembersByPagePresenter presenter = new FakeListMembersByPagePresenter(_output);
            IListMembersByPageInteractor interactor = new ListMembersByPageInteractor(presenter, entityGateway);
            ListMembersByPageInputModel inputModel = new ListMembersByPageInputModel { PageNumber = 0, MembersPerPage = 25 };

            //act
            var result = await interactor.ListMembersByPageAsync(inputModel);

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
        public async Task ListMembersByPageAsync_Success_GetFirstPage()
        {
            //arrange
            var members = new List<Member>();
            for (int i = 1; i < 100; i++)
            {
                members.Add(new Member { Username = string.Format("Agent{0}", i) });
            }
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(members.ToArray());
            IListMembersByPagePresenter presenter = new FakeListMembersByPagePresenter(_output);
            IListMembersByPageInteractor interactor = new ListMembersByPageInteractor(presenter, entityGateway);
            ListMembersByPageInputModel inputModel = new ListMembersByPageInputModel { PageNumber = 1, MembersPerPage = 25 };

            //act
            var result = await interactor.ListMembersByPageAsync(inputModel);

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
        public async Task ListMembersByPageAsync_Success_GetMiddlePage()
        {
            //arrange
            var members = new List<Member>();
            for (int i = 1; i < 100; i++)
            {
                members.Add(new Member { Username = string.Format("Agent{0}", i) });
            }
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(members.ToArray());
            IListMembersByPagePresenter presenter = new FakeListMembersByPagePresenter(_output);
            IListMembersByPageInteractor interactor = new ListMembersByPageInteractor(presenter, entityGateway);
            ListMembersByPageInputModel inputModel = new ListMembersByPageInputModel { PageNumber = 2, MembersPerPage = 25 };

            //act
            var result = await interactor.ListMembersByPageAsync(inputModel);

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
        public async Task ListMembersByPageAsync_Success_GetLastPage()
        {
            //arrange
            var members = new List<Member>();
            for (int i = 1; i < 100; i++)
            {
                members.Add(new Member { Username = string.Format("Agent{0}", i) });
            }
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(members.ToArray());
            IListMembersByPagePresenter presenter = new FakeListMembersByPagePresenter(_output);
            IListMembersByPageInteractor interactor = new ListMembersByPageInteractor(presenter, entityGateway);
            ListMembersByPageInputModel inputModel = new ListMembersByPageInputModel { PageNumber = 4, MembersPerPage = 25 };

            //act
            var result = await interactor.ListMembersByPageAsync(inputModel);

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
        public async Task ListMembersByPageAsync_Success_GetLastPageNextPage()
        {
            //arrange
            var members = new List<Member>();
            for (int i = 1; i < 100; i++)
            {
                members.Add(new Member { Username = string.Format("Agent{0}", i) });
            }
            IEntityGateway entityGateway = await InMemoryEntityGatewayFactory.CreateEntityGatewayAsync(members.ToArray());
            IListMembersByPagePresenter presenter = new FakeListMembersByPagePresenter(_output);
            IListMembersByPageInteractor interactor = new ListMembersByPageInteractor(presenter, entityGateway);
            ListMembersByPageInputModel inputModel = new ListMembersByPageInputModel { PageNumber = 5, MembersPerPage = 25 };

            //act
            var result = await interactor.ListMembersByPageAsync(inputModel);

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

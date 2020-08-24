using Rang.Demo.CleanArchitecture.Domain.Common;
using Rang.Demo.CleanArchitecture.Domain.Entity;
using Rang.Demo.CleanArchitecture.Domain.Model;
using System;
using Xunit;

namespace Rang.Demo.CleanArchitecture.XUnitTest.DomainTests
{
    public class ClubTests
    {
        [Fact]
        public void CreateClub_Success_DefaultConstructor()
        {
            // arrange

            // act
            var entity = new Club { Name = "C# Coding Club" };

            // assert
            Assert.NotNull(entity);
            Assert.True(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.Count == 0);
            Assert.True(entity.Id != null && entity.Id != Guid.Empty);
        }

        [Fact]
        public void CreateClub_Success_Constructor1()
        {
            // arrange
            var johnDoe = new User { Username = "john.doe" };
            var williamDoe = new User { Username = "william.doe" };
            var clubMemberModels = new ClubMemberModel[] {
                new ClubMemberModel { UserId = johnDoe.Id },
                new ClubMemberModel { UserId = williamDoe.Id } };
            var model = new ClubModel { Name = "C# Coding Club", ClubMemberModels = clubMemberModels };

            // act
            var entity = new Club(model);

            // assert
            Assert.NotNull(entity);
            Assert.True(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.Count == 0);
            Assert.True(entity.Id != null && entity.Id != Guid.Empty);
        }

        [Fact]
        public void CreateClub_ThrowsException_NullInput()
        {
            // arrange  
            ClubModel model = null;

            // act
            Action action = () => new Club(model);

            // assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void CreateClub_ThrowsException_DuplicateClubMembersInput()
        {
            // arrange  
            var johnDoe = new User { Username = "john.doe" };
            var clubMemberModel = new ClubMemberModel { UserId = johnDoe.Id };
            var clubMemberModels = new ClubMemberModel[] {
                clubMemberModel,
                clubMemberModel }; //<-- Added twice the same clubMemberModel to the array of models
            var model = new ClubModel { Name = "C# Coding Club", ClubMemberModels = clubMemberModels };

            // act
            var entity = new Club(model);

            // assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.InvalidDataSupplied));
            Assert.True(entity.Id != null && entity.Id != Guid.Empty);
        }

        [Fact]
        public void CreateClub_ThrowsException_DuplicateMemberInput()
        {
            // arrange  
            var johnDoe = new User { Username = "john.doe" };
            var clubMemberModels = new ClubMemberModel[] {
                new ClubMemberModel { UserId = johnDoe.Id },
                new ClubMemberModel { UserId = johnDoe.Id } }; //<-- added the same memberId within 2 different ClubMemberModel to the array of models
            var model = new ClubModel { Name = "C# Coding Club", ClubMemberModels = clubMemberModels };

            // act
            var entity = new Club(model);

            // assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.InvalidDataSupplied));
            Assert.True(entity.Id != null && entity.Id != Guid.Empty);
        }

        [Fact]
        public void CreateClub_FailedModelValidation_NullName()
        {
            // arrange
            var model = new ClubModel { Name = null };

            // act
            var entity = new Club(model);

            // assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.RequiredInformationMissing));
        }

        [Fact]
        public void CreateClub_FailedModelValidation_EmptyName()
        {
            // arrange
            var model = new ClubModel { Name = string.Empty };

            // act
            var entity = new Club(model);

            // assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.RequiredInformationMissing));
        }

        [Fact]
        public void CreateClub_FailedModelValidation_BlankName()
        {
            // arrange
            var model = new ClubModel { Name = " " };

            // act
            var entity = new Club(model);

            // assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.RequiredInformationMissing));
        }

        [Fact]
        public void CreateClub_FailedModelValidation_ExceedingLengthName()
        {
            // arrange
            var model = new ClubModel { Name = new string('*', Club.NAME_MAX_LENGTH + 1) };

            // act
            var entity = new Club(model);

            // assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.CapacityExceeded));
        }

        [Fact]
        public void CreateClub_FailedModelValidation_InvalidMember()
        {
            // arrange
            var johnDoe = new User { Username = "john.doe" };
            var clubMemberModels = new ClubMemberModel[] {
                new ClubMemberModel { UserId = johnDoe.Id },
                new ClubMemberModel { UserId = Guid.Empty } };
            var model = new ClubModel { Name = "C# Coding Club", ClubMemberModels = clubMemberModels };

            // act
            var entity = new Club(model);

            // assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.InternalMemberFailedValidation));
        }

        [Fact]
        public void CreateClub_FailedModelValidation_NullMember()
        {
            // arrange
            var johnDoe = new User { Username = "john.doe" };
            var clubMemberModels = new ClubMemberModel[] {
                new ClubMemberModel { UserId = johnDoe.Id },
                null };
            var model = new ClubModel { Name = "C# Coding Club", ClubMemberModels = clubMemberModels };

            // act
            Action action = () => new Club(model);

            // assert
            Assert.Throws<ArgumentNullException>(action);
        }
    }
}

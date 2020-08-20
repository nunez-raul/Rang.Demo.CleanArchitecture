using Rang.Demo.CleanArchitecture.Domain.Common;
using Rang.Demo.CleanArchitecture.Domain.Entity;
using Rang.Demo.CleanArchitecture.Domain.Model;
using System;
using Xunit;

namespace Rang.Demo.CleanArchitecture.XUnitTest.DomainTests
{
    public class UserTests
    {
        [Fact]
        public void CreateUser_Success_DefaultConstructor()
        {
            // arrange

            // act
            var entity = new User
            {
                Username = "Black Sheep"
            };

            // assert
            Assert.NotNull(entity);
            Assert.True(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.Count == 0);
            Assert.True(entity.Id != null && entity.Id != Guid.Empty);
        }

        [Fact]
        public void CreateUser_Success_Constructor1()
        {
            // arrange
            var model = new UserModel { Username = "Balck Sheep" };

            // act
            var entity = new User(model);

            // assert
            Assert.NotNull(entity);
            Assert.True(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.Count == 0);
            Assert.True(entity.Id != null && entity.Id != Guid.Empty);
        }

        [Fact]
        public void CreateUser_ThrowsException_NullInput()
        {
            // arrange  
            UserModel model = null;

            // act
            Action action = () => new User(model);

            // assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void CreateUser_FailedModelValidation_NullUsername()
        {
            // arrange
            var model = new UserModel { Username = null };

            // act
            var entity = new User(model);

            // assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.RequiredInformationMissing));
            Assert.True(entity.Id != null && entity.Id != Guid.Empty);
        }

        [Fact]
        public void CreateUser_FailedModelValidation_EmptyUsername()
        {
            // arrange
            var model = new UserModel { Username = string.Empty };

            // act
            var entity = new User(model);

            // assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.RequiredInformationMissing));
        }

        [Fact]
        public void CreateUser_FailedModelValidation_BlankUsername()
        {
            // arrange
            var model = new UserModel { Username = " " };

            // act
            var entity = new User(model);

            // assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.RequiredInformationMissing));
        }

        [Fact]
        public void CreateUser_FailedModelValidation_ExeedingLengthUsername()
        {
            // arrange
            var model = new UserModel { Username = new string('*', User.USERNAME_MAX_LENGTH + 1) };

            // act
            var entity = new User(model);

            // assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.CapacityExceeded));
        }
    }
}

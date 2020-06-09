using Rang.Demo.CleanArchitecture.Domain.Common;
using Rang.Demo.CleanArchitecture.Domain.Entity;
using Rang.Demo.CleanArchitecture.Domain.Model;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Rang.Demo.CleanArchitecture.XUnitTest.DomainTests
{
    public class MemberTests
    {
        [Fact]
        public async Task CreateMember_Success_DefaultConstructor()
        {
            //arrange

            //act
            var entity = await Task<Member>.Factory.StartNew(() => new Member());
            entity.Username = "Black Sheep";

            //assert
            Assert.NotNull(entity);
            Assert.True(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.Count == 0);
            Assert.True(entity.Id != null);
        }

        [Fact]
        public async Task CreateMember_Success_Constructor1()
        {
            //arrange
            var model = new MemberModel { Username = "Balck Sheep" };

            //act
            var entity = await Task<Member>.Factory.StartNew(() => new Member(model));

            //assert
            Assert.NotNull(entity);
            Assert.True(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.Count == 0);
            Assert.True(entity.Id != null);
        }

        [Fact]
        public async Task CreateMember_ThrowsException_NullInput()
        {
            //arrange  

            //act
            async Task<Member> function() => await Task<Member>.Factory.StartNew(() => new Member(null));

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task CreateMember_FailedModelValidation_NullUsername()
        {
            //arrange
            var model = new MemberModel { Username = null };

            //act
            var entity = await Task<Member>.Factory.StartNew(() => new Member(model));

            //assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.RequiredInformationMissing));
        }

        [Fact]
        public async Task CreateMember_FailedModelValidation_EmptyUsername()
        {
            //arrange
            var model = new MemberModel { Username = string.Empty };

            //act
            var entity = await Task<Member>.Factory.StartNew(() => new Member(model));

            //assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.RequiredInformationMissing));
        }

        [Fact]
        public async Task CreateMember_FailedModelValidation_BlankUsername()
        {
            //arrange
            var model = new MemberModel { Username = " " };

            //act
            var entity = await Task<Member>.Factory.StartNew(() => new Member(model));

            //assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.RequiredInformationMissing));
        }

        [Fact]
        public async Task CreateMember_FailedModelValidation_ExeedingLengthUsername()
        {
            //arrange
            var model = new MemberModel { Username = new string('*', Member.USERNAME_MAX_LENGTH + 1) };

            //act
            var entity = await Task<Member>.Factory.StartNew(() => new Member(model));

            //assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.CapacityExceeded));
        }
    }
}

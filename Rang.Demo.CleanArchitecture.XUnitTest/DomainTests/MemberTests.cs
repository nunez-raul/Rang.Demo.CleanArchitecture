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
            entity.Codename = "Black Sheep";

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
            var model = new MemberModel { Codename = "Balck Sheep" };

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
        public async Task CreateMember_FailedModelValidation_NullCodename()
        {
            //arrange
            var model = new MemberModel { Codename = null };

            //act
            var entity = await Task<Member>.Factory.StartNew(() => new Member(model));

            //assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.RequiredInformationMissing));
        }

        [Fact]
        public async Task CreateMember_FailedModelValidation_EmptyCodename()
        {
            //arrange
            var model = new MemberModel { Codename = string.Empty };

            //act
            var entity = await Task<Member>.Factory.StartNew(() => new Member(model));

            //assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.RequiredInformationMissing));
        }

        [Fact]
        public async Task CreateMember_FailedModelValidation_BlankCodename()
        {
            //arrange
            var model = new MemberModel { Codename = " " };

            //act
            var entity = await Task<Member>.Factory.StartNew(() => new Member(model));

            //assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.RequiredInformationMissing));
        }

        [Fact]
        public async Task CreateMember_FailedModelValidation_ExeedingLengthCodename()
        {
            //arrange
            var model = new MemberModel { Codename = new string('*', Member.CODENAME_MAX_LENGTH + 1) };

            //act
            var entity = await Task<Member>.Factory.StartNew(() => new Member(model));

            //assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.CapacityExceeded));
        }
    }
}

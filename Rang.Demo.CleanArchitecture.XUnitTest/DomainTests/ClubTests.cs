using Rang.Demo.CleanArchitecture.Domain.Common;
using Rang.Demo.CleanArchitecture.Domain.Entity;
using Rang.Demo.CleanArchitecture.Domain.Model;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Rang.Demo.CleanArchitecture.XUnitTest.DomainTests
{
    public class ClubTests
    {
        [Fact]
        public async Task CreateClub_Success_DefaultConstructor()
        {
            //arrange

            //act
            var entity = await Task<Club>.Factory.StartNew(() => new Club());
            entity.Name = "C# Coding Club";

            //assert
            Assert.NotNull(entity);
            Assert.True(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.Count == 0);
            Assert.True(entity.Id != null);
        }

        [Fact]
        public async Task CreateClub_Success_Constructor1()
        {
            //arrange
            var members = new MemberModel[] { new MemberModel { Username = "john.doe" }, new MemberModel { Username = "william.doe" } };
            var model = new ClubModel { Name = "C# Coding Club", Members = members };

            //act
            var entity = await Task<Club>.Factory.StartNew(() => new Club(model));

            //assert
            Assert.NotNull(entity);
            Assert.True(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.Count == 0);
            Assert.True(entity.Id != null);
        }

        [Fact]
        public async Task CreateClub_ThrowsException_NullInput()
        {
            //arrange  

            //act
            async Task<Club> function() => await Task<Club>.Factory.StartNew(() => new Club(null));

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task CreateClub_ThrowsException_DuplicateMembersInput()
        {
            //arrange  
            var validMember = new MemberModel { Username = "john.doe" };
            var members = new MemberModel[] { validMember, validMember };
            var model = new ClubModel { Name = "C# Coding Club", Members = members };

            //act
            async Task<Club> function() => await Task<Club>.Factory.StartNew(() => new Club(model));

            //assert
            await Assert.ThrowsAsync<ArgumentException>(function);
        }

        [Fact]
        public async Task CreateClub_FailedModelValidation_NullName()
        {
            //arrange
            var model = new ClubModel { Name = null };

            //act
            var entity = await Task<Club>.Factory.StartNew(() => new Club(model));

            //assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.RequiredInformationMissing));
        }

        [Fact]
        public async Task CreateClub_FailedModelValidation_EmptyName()
        {
            //arrange
            var model = new ClubModel { Name = string.Empty };

            //act
            var entity = await Task<Club>.Factory.StartNew(() => new Club(model));

            //assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.RequiredInformationMissing));
        }

        [Fact]
        public async Task CreateClub_FailedModelValidation_BlankName()
        {
            //arrange
            var model = new ClubModel { Name = " " };

            //act
            var entity = await Task<Club>.Factory.StartNew(() => new Club(model));

            //assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.RequiredInformationMissing));
        }

        [Fact]
        public async Task CreateClub_FailedModelValidation_ExceedingLengthName()
        {
            //arrange
            var model = new ClubModel { Name = new string('*', Club.NAME_MAX_LENGTH + 1) };

            //act
            var entity = await Task<Club>.Factory.StartNew(() => new Club(model));

            //assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.CapacityExceeded));
        }

        [Fact]
        public async Task CreateClub_FailedModelValidation_InvalidMember()
        {
            //arrange
            var invalidMember = new MemberModel { Username = null };
            var validMember = new MemberModel { Username = "john.doe" };
            var members = new MemberModel[] { validMember, invalidMember };
            var model = new ClubModel { Name = "C# Coding Club", Members = members };

            //act
            var entity = await Task<Club>.Factory.StartNew(() => new Club(model));

            //assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.InternalMemberFailedValidation));
        }
    }
}

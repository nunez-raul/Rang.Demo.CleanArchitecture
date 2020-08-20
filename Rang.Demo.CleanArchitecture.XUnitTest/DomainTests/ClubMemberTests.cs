using Rang.Demo.CleanArchitecture.Domain.Common;
using Rang.Demo.CleanArchitecture.Domain.Entity;
using Rang.Demo.CleanArchitecture.Domain.Model;
using System;
using Xunit;

namespace Rang.Demo.CleanArchitecture.XUnitTest.DomainTests
{
    public class ClubMemberTests
    {
        [Fact]
        public void CreateClubMember_Success_DefaultConstructor()
        {
            // arrange
            var member = new User();

            // act
            var entity = new ClubMember{ UserId = member.Id };

            // assert
            Assert.NotNull(entity);
            Assert.True(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.Count == 0);
            Assert.True(entity.Id != null && entity.Id != Guid.Empty);
        }

        [Fact]
        public void CreateClubMember_Success_Constructor1()
        {
            // arrange
            var member = new User();
            var model = new ClubMemberModel { UserId = member.Id };

            // act
            var entity = new ClubMember(model);

            // assert
            Assert.NotNull(entity);
            Assert.True(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.Count == 0);
            Assert.True(entity.Id != null && entity.Id != Guid.Empty);
        }

        [Fact]
        public void CreateClubMember_ThrowsException_NullInput()
        {
            // arrange  
            ClubMemberModel model = null;
            
            // act
            Action action = () => new ClubMember(model);

            // assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void CreateClubMember_FailedModelValidation_NullMemberId()
        {
            //arrange
            var model = new ClubMemberModel();

            //act
            var entity = new ClubMember(model);

            //assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.RequiredInformationMissing));
            Assert.True(entity.Id != null && entity.Id != Guid.Empty);
        }

        [Fact]
        public void CreateClubMember_FailedModelValidation_EmptyMemberId()
        {
            //arrange
            var model = new ClubMemberModel { UserId = Guid.Empty };

            //act
            var entity = new ClubMember(model);

            //assert
            Assert.NotNull(entity);
            Assert.False(entity.IsValid);
            Assert.True(entity.ModelValidationErrors.ContainsKey(ModelValidationStatusCode.RequiredInformationMissing));
            Assert.True(entity.Id != null && entity.Id != Guid.Empty);
        }
    }
}

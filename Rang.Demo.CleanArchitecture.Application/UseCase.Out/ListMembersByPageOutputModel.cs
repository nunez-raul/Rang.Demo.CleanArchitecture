using Rang.Demo.CleanArchitecture.Application.Common;
using Rang.Demo.CleanArchitecture.Domain.Entity;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.Out
{
    public class ListMembersByPageOutputModel
    {
        //properties
        public Page<Member> Page { get; set; }
    }
}

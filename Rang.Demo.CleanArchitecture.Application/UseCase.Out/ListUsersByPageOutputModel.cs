using Rang.Demo.CleanArchitecture.Application.Common;
using Rang.Demo.CleanArchitecture.Domain.Entity;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.Out
{
    public class ListUsersByPageOutputModel
    {
        //properties
        public Page<User> Page { get; set; }
    }
}

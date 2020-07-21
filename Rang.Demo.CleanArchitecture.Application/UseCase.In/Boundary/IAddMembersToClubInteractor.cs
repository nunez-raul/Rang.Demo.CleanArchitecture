using Rang.Demo.CleanArchitecture.Application.UseCase.Out;
using System.Threading.Tasks;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.In.Boundary
{
    public interface IAddMembersToClubInteractor
    {
        Task<CommandResult<AddMembersToClubOutputModel>> AddMembersToClubAsync(AddMembersToClubInputModel inputModel);
    }
}

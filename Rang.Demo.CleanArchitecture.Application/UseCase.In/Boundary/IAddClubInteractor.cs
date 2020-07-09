using Rang.Demo.CleanArchitecture.Application.UseCase.Out;
using System.Threading.Tasks;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.In.Boundary
{
    public interface IAddClubInteractor
    {
        Task<CommandResult<AddClubOutputModel>> AddClubAsync(AddClubInputModel inputModel);
    }
}

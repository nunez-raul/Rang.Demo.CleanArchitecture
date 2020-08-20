using Rang.Demo.CleanArchitecture.Application.UseCase.Out;
using System.Threading.Tasks;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.In.Boundary
{
    public interface IListUsersByPageInteractor
    {
        Task<CommandResult<ListUsersByPageOutputModel>> ListUsersByPageAsync(ListUsersByPageInputModel inputModel);
    }
}

using Rang.Demo.CleanArchitecture.Application.UseCase.Out;
using System.Threading.Tasks;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.In.Boundary
{
    public interface IListMembersByPageInteractor
    {
        Task<CommandResult<ListMembersByPageOutputModel>> ListMembersByPageAsync(ListMembersByPageInputModel inputModel);
    }
}

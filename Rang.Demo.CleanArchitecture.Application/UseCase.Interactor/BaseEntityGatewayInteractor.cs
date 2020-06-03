using Rang.Demo.CleanArchitecture.Application.Infrastructure.PlugIn;
using System;

namespace Rang.Demo.CleanArchitecture.Application.UseCase.Interactor
{
    public abstract class BaseEntityGatewayInteractor
    {
        protected IEntityGateway _entityGateway;

        protected BaseEntityGatewayInteractor(IEntityGateway entityGateway)
        {
            _entityGateway = entityGateway ?? throw new ArgumentNullException(nameof(entityGateway));
        }
    }
}

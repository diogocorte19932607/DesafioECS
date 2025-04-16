using DesafioTecnicoECS.Infra.Context;
using DesafioTecnicoECS.Infra.Entity;
using DesafioTecnicoECS.Infra.Repositories.Interfaces;

namespace DesafioTecnicoECS.Infra.Repositories
{
        public class ClienteRepository : RepositoryGeneric<Cliente>, IClienteRepository
        {
            private ClientContext _appContext => (ClientContext)_context;

            public ClienteRepository(ClientContext context) : base(context)
            { }



        }
    }

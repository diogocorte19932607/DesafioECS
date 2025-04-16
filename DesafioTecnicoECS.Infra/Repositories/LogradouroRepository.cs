using DesafioTecnicoECS.Infra.Context;
using DesafioTecnicoECS.Infra.Entity;
using DesafioTecnicoECS.Infra.Repositories.Interfaces;

namespace DesafioTecnicoECS.Infra.Repositories
{
    public class LogradouroRepository : RepositoryGeneric<Logradouro>, ILogradouroRepository
    {
        private ClientContext _appContext => (ClientContext)_context;

        public LogradouroRepository(ClientContext context) : base(context)
        { }



    }
}

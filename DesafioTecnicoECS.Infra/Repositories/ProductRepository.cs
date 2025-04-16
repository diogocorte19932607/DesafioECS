using DesafioTecnicoECS.Infra.Context;
using DesafioTecnicoECS.Infra.Entity;
using DesafioTecnicoECS.Infra.Services.Interfaces;

namespace DesafioTecnicoECS.Infra.Repositories
{
    public class ProductRepository : RepositoryGeneric<Product>, IProductRepository
    {
        private ClientContext _appContext => (ClientContext)_context;

        public ProductRepository(ClientContext context) : base(context)
        { }
    }
}
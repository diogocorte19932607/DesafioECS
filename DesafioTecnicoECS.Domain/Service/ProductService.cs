using AutoMapper;

using Microsoft.Extensions.Options;
using DesafioTecnicoECS.Domain.Configuration;
using DesafioTecnicoECS.Domain.Domain;
using DesafioTecnicoECS.Domain.Service.Generic;
using DesafioTecnicoECS.Infra.Entity;
using DesafioTecnicoECS.Infra.UnitofWork;
using DesafioTecnicoECS.Infra.Services.Interfaces;

namespace DesafioTecnicoECS.Domain.Service
{
    public class ProductService<Tv, Te> : GenericServiceAsync<Tv, Te>
                                               where Tv : ProductModel
                                               where Te : Product
    {
        IProductRepository _productRepository;
        private readonly AppSettings _appSettings;
        public ProductService(IUnitofWork unitOfWork, IMapper mapper,
                             IProductRepository clienteRepository, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productRepository = clienteRepository;
        }

        public async Task<Product> ModelarProduct(ProductCreateModel product)
        {
            Product result = new Product()
            {
                Id = Guid.NewGuid(),
                Code = product.Code,
                Name = product.Name,
                Price = product.Price,


            };

            return result;
        }

        public async Task<RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>> AdicionarProduct(ProductCreateModel product)
        {

            RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid> retornoController = new RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>();
            try
            {
                var entityProduct = await ModelarProduct(product);

                _productRepository.Add(entityProduct);
                _productRepository.Save();


                return retornoController;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<List<ProductModel>> ListarProducts()
        {
            var Products = _productRepository.GetAll();

            List<ProductModel> products = new List<ProductModel>();
            foreach (var elem in Products)
            {
                var lista = new ProductModel();
                lista.Id = elem.Id;
                lista.Code = elem.Code;
                lista.Name = elem.Name;
                lista.Price = elem.Price;
                products.Add(lista);
            }
            return products.ToList();
        }
    }
}

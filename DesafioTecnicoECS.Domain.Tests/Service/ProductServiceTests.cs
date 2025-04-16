using AutoMapper;
using DesafioTecnicoECS.Domain.Configuration;
using DesafioTecnicoECS.Domain.Domain;
using DesafioTecnicoECS.Domain.Service;
using DesafioTecnicoECS.Infra.Entity;
using DesafioTecnicoECS.Infra.Services.Interfaces;
using DesafioTecnicoECS.Infra.UnitofWork;
using Microsoft.Extensions.Options;
using Moq;

namespace DesafioTecnicoECS.Test
{
    public class ProductServiceTests
    {
        private readonly ProductService<ProductModel, Product> _service;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IUnitofWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IOptions<AppSettings>> _optionsMock;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _unitOfWorkMock = new Mock<IUnitofWork>();
            _mapperMock = new Mock<IMapper>();
            _optionsMock = new Mock<IOptions<AppSettings>>();
            _optionsMock.Setup(o => o.Value).Returns(new AppSettings());

            _service = new ProductService<ProductModel, Product>(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _productRepositoryMock.Object,
                _optionsMock.Object
            );
        }

        [Fact]
        public async Task ModelarProduct_DeveRetornarProductModelado()
        {
            // Arrange
            var input = new ProductCreateModel
            {
                Code = "123",
                Name = "Produto Teste",
                Price = 99.99m
            };

            // Act
            var result = await _service.ModelarProduct(input);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(input.Code, result.Code);
            Assert.Equal(input.Name, result.Name);
            Assert.Equal(input.Price, result.Price);
            Assert.NotEqual(Guid.Empty, result.Id);
        }

        [Fact]
        public async Task AdicionarProduct_DeveAdicionarProdutoComSucesso()
        {
            // Arrange
            var input = new ProductCreateModel
            {
                Code = "456",
                Name = "Produto Add",
                Price = 150
            };

            _productRepositoryMock.Setup(p => p.Add(It.IsAny<Product>()));
            _productRepositoryMock.Setup(p => p.Save());

            // Act
            var result = await _service.AdicionarProduct(input);

            // Assert
            Assert.NotNull(result);
            _productRepositoryMock.Verify(p => p.Add(It.IsAny<Product>()), Times.Once);
            _productRepositoryMock.Verify(p => p.Save(), Times.Once);
        }

        [Fact]
        public async Task ListarProducts_DeveRetornarListaDeProducts()
        {
            // Arrange
            var list = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Code = "P01", Name = "Produto 01", Price = 10 },
                new Product { Id = Guid.NewGuid(), Code = "P02", Name = "Produto 02", Price = 20 }
            };

            _productRepositoryMock.Setup(p => p.GetAll()).Returns(list);

            // Act
            var result = await _service.ListarProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("P01", result[0].Code);
            Assert.Equal("P02", result[1].Code);
        }
    }
}

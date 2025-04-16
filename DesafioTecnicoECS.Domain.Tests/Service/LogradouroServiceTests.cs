using AutoMapper;
using DesafioTecnicoECS.Domain.Configuration;
using DesafioTecnicoECS.Domain.Domain;
using DesafioTecnicoECS.Domain.Service;
using DesafioTecnicoECS.Infra.Context;
using DesafioTecnicoECS.Infra.Entity;
using DesafioTecnicoECS.Infra.Repositories.Interfaces;
using DesafioTecnicoECS.Infra.UnitofWork;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DesafioTecnicoECS.Domain.Tests.Service
{
    public class LogradouroServiceTests
    {
        private readonly LogradouroService<LogradouroModel, Logradouro> _service;
        private readonly Mock<ILogradouroRepository> _logradouroRepositoryMock;
        private readonly Mock<IUnitofWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ClientContext> _clientContextMock;

        public LogradouroServiceTests()
        {
            _logradouroRepositoryMock = new Mock<ILogradouroRepository>();
            _unitOfWorkMock = new Mock<IUnitofWork>();
            _mapperMock = new Mock<IMapper>();
            _clientContextMock = new Mock<ClientContext>(new DbContextOptions<ClientContext>());

            _service = new LogradouroService<LogradouroModel, Logradouro>(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _logradouroRepositoryMock.Object,
                _clientContextMock.Object
            );
        }

        [Fact]
        public void ModelarLogradouro_DeveRetornarLogradouroModelado()
        {
            var input = new LogradouroCreateModel
            {
                IdCliente = Guid.NewGuid(),
                CEP = "12345678",
                Rua = "Rua Teste",
                Cidade = "Cidade Teste",
                Bairro = "Bairro Teste",
                Estado = "Estado Teste",
                Numero = "123",
                Complemento = "Complemento Teste"
            };

            var result = _service.ModelarLogradouro(input);

            Assert.NotNull(result);
            Assert.Equal(input.CEP, result.CEP);
            Assert.Equal(input.Rua, result.Rua);
            Assert.Equal(input.Cidade, result.Cidade);
            Assert.Equal(input.Bairro, result.Bairro);
            Assert.Equal(input.Estado, result.Estado);
            Assert.Equal(input.Numero, result.Numero);
            Assert.Equal(input.Complemento, result.Complemento);
        }

        [Fact]
        public async Task DeletarLogradouro_DeveRemoverLogradouro()
        {
            var logradouroId = Guid.NewGuid();
            var existingLogradouro = new Logradouro
            {
                Id = logradouroId,
                Rua = "Logradouro Teste"
            };

            _logradouroRepositoryMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Logradouro, bool>>>()))
                .Returns(existingLogradouro);

            var result = await _service.DeletarLogradouro(logradouroId.ToString());

            Assert.Equal(logradouroId.ToString(), result);
            _logradouroRepositoryMock.Verify(r => r.Remove(It.IsAny<Logradouro>()), Times.Once);
            _logradouroRepositoryMock.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public async Task ListarLougradouros_DeveRetornarListaDeLogradouros()
        {
            var logradouros = new List<Logradouro>
            {
                new Logradouro { Id = Guid.NewGuid(), Rua = "Rua A", CEP = "00000-000", Cidade = "Cidade A", Bairro = "Bairro A", Estado = "SP", Numero = "10", Complemento = "Apto 1", IdCliente = Guid.NewGuid() },
                new Logradouro { Id = Guid.NewGuid(), Rua = "Rua B", CEP = "11111-111", Cidade = "Cidade B", Bairro = "Bairro B", Estado = "RJ", Numero = "20", Complemento = "Casa", IdCliente = Guid.NewGuid() }
            };

            _logradouroRepositoryMock.Setup(r => r.GetAll()).Returns(logradouros);

            var result = await _service.ListarLougradouros();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Rua A", result[0].Rua);
            Assert.Equal("Rua B", result[1].Rua);
        }
    }
}

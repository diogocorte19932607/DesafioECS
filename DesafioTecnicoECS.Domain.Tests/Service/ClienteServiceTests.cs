
using AutoMapper;
using DesafioTecnicoECS.Domain.Configuration;
using DesafioTecnicoECS.Domain.Domain;
using DesafioTecnicoECS.Domain.Service;
using DesafioTecnicoECS.Infra.Entity;
using DesafioTecnicoECS.Infra.Repositories.Interfaces;
using DesafioTecnicoECS.Infra.UnitofWork;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DesafioTecnicoECS.Domain.Tests.Service
{

    namespace DesafioTecnicoECS.Test
    {
        public class ClienteServiceTests
        {
            private readonly ClienteService<ClienteModel, Cliente> _service;
            private readonly Mock<IClienteRepository> _clienteRepositoryMock;
            private readonly Mock<IUnitofWork> _unitOfWorkMock;
            private readonly Mock<IMapper> _mapperMock;
            private readonly Mock<IOptions<AppSettings>> _optionsMock;
            private readonly AppSettings _appSettings;

            public ClienteServiceTests()
            {
                _clienteRepositoryMock = new Mock<IClienteRepository>();
                _unitOfWorkMock = new Mock<IUnitofWork>();
                _mapperMock = new Mock<IMapper>();
                _appSettings = new AppSettings { PastaImagens = "C:\\Imagens" };
                _optionsMock = new Mock<IOptions<AppSettings>>();
                _optionsMock.Setup(o => o.Value).Returns(_appSettings);

                _service = new ClienteService<ClienteModel, Cliente>(
                    _unitOfWorkMock.Object,
                    _mapperMock.Object,
                    _clienteRepositoryMock.Object,
                    _optionsMock.Object
                );
            }

            [Fact]
            public async Task ModelarCliente_DeveRetornarClienteModelado()
            {
                var input = new ClienteCreateModel
                {
                    Nome = "Cliente Teste",
                    Email = "cliente@teste.com",
                    Logotipo = new Logotipo
                    {
                        Base64 = Convert.ToBase64String(new byte[] { 1, 2, 3 }),
                        Extensao = "png"
                    }
                };

                var result = await _service.ModelarCliente(input);

                Assert.NotNull(result);
                Assert.Equal(input.Nome, result.Nome);
                Assert.Equal(input.Email, result.Email);
                Assert.Contains(result.Id.ToString(), result.Logotipo);
            }

            [Fact]
            public async Task AdicionarCliente_DeveRetornarErroSeEmailExistir()
            {
                var input = new ClienteCreateModel
                {
                    Nome = "Cliente Existente",
                    Email = "existente@teste.com",
                    Logotipo = new Logotipo
                    {
                        Base64 = Convert.ToBase64String(new byte[] { 1, 2, 3 }),
                        Extensao = "jpg"
                    }
                };

                _clienteRepositoryMock.Setup(r => r.Find(It.IsAny<Expression<Func<Cliente, bool>>>()))
                    .Returns(new List<Cliente> { new Cliente { Email = input.Email } }.AsQueryable());

                var result = await _service.AdicionarCliente(input);

                Assert.NotNull(result);
                Assert.Equal(400, result.ExibicaoMensagem.StatusCode);
                Assert.Equal("E-mail já cadastrado!", result.ExibicaoMensagem.MensagemCurta);
            }

            [Fact]
            public async Task AtualizarCliente_DeveAtualizarComSucesso()
            {
                var clienteId = Guid.NewGuid();
                var existingCliente = new Cliente
                {
                    Id = clienteId,
                    Nome = "Cliente Antigo",
                    Email = "antigo@teste.com",
                    Logotipo = "caminho/antigo/logo.png"
                };

                var input = new ClienteEditModel
                {
                    Id = clienteId,
                    Nome = "Cliente Atualizado",
                    Email = "atualizado@teste.com",
                    Logotipo = new Logotipo
                    {
                        Base64 = Convert.ToBase64String(new byte[] { 4, 5, 6 }),
                        Extensao = "png"
                    }
                };

                _clienteRepositoryMock.Setup(r => r.Find(It.IsAny<Expression<Func<Cliente, bool>>>()))
                    .Returns(new List<Cliente> { existingCliente }.AsQueryable());

                var result = await _service.AtualizarCliente(input);

                Assert.NotNull(result);
                _clienteRepositoryMock.Verify(r => r.Update(It.IsAny<Cliente>()), Times.Once);
                _clienteRepositoryMock.Verify(r => r.Save(), Times.Once);
            }

            [Fact]
            public async Task DeletarCliente_DeveRemoverCliente()
            {
                var clienteId = Guid.NewGuid();
                var existingCliente = new Cliente
                {
                    Id = clienteId,
                    Nome = "Cliente Para Deletar",
                    Email = "deletar@teste.com"
                };

                _clienteRepositoryMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Cliente, bool>>>()))
                    .Returns(existingCliente);

                var result = await _service.DeletarCliente(clienteId.ToString());

                Assert.Equal(clienteId.ToString(), result);
                _clienteRepositoryMock.Verify(r => r.Remove(It.IsAny<Cliente>()), Times.Once);
                _clienteRepositoryMock.Verify(r => r.Save(), Times.Once);
            }

            [Fact]
            public async Task ListarClientes_DeveRetornarListaDeClientes()
            {
                var clientes = new List<Cliente>
            {
                new Cliente
                {
                    Id = Guid.NewGuid(),
                    Nome = "Cliente 1",
                    Email = "cliente1@teste.com",
                    Logotipo = "caminho/logo1.png"
                },
                new Cliente
                {
                    Id = Guid.NewGuid(),
                    Nome = "Cliente 2",
                    Email = "cliente2@teste.com",
                    Logotipo = "caminho/logo2.png"
                }
            };

                _clienteRepositoryMock.Setup(r => r.GetAll()).Returns(clientes);

                var result = await _service.ListarClientes();

                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.Equal("Cliente 1", result[0].Nome);
                Assert.Equal("Cliente 2", result[1].Nome);
            }

            [Fact]
            public async Task BuscarClienteEmail_DeveRetornarCliente()
            {
                var email = "buscar@teste.com";
                var cliente = new Cliente
                {
                    Id = Guid.NewGuid(),
                    Nome = "Cliente Buscar",
                    Email = email
                };

                _clienteRepositoryMock.Setup(r => r.Find(It.IsAny<Expression<Func<Cliente, bool>>>()))
                    .Returns(new List<Cliente> { cliente }.AsQueryable());

                var result = await _service.BuscarClienteEmail(email);

                Assert.NotNull(result);
                Assert.Equal(email, result.Email);
            }
        }
    }
}
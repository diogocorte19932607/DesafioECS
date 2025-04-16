using AutoMapper;
using Microsoft.Extensions.Options;

using DesafioTecnicoECS.Domain.Configuration;
using DesafioTecnicoECS.Domain.Domain;
using DesafioTecnicoECS.Domain.Service.Generic;
using DesafioTecnicoECS.Infra.Entity;
using DesafioTecnicoECS.Infra.Repositories.Interfaces;
using DesafioTecnicoECS.Infra.UnitofWork;

namespace DesafioTecnicoECS.Domain.Service
{
    public class ClienteService<Tv, Te> : GenericServiceAsync<Tv, Te>
                                               where Tv : ClienteModel
                                               where Te : Cliente
    {
        IClienteRepository _clienteRepository;
        private readonly AppSettings _appSettings;
        public ClienteService(IUnitofWork unitOfWork, IMapper mapper,
                             IClienteRepository clienteRepository, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _clienteRepository = clienteRepository;
        }

        public async Task<Cliente> ModelarCliente(ClienteCreateModel cliente)
        {
            Cliente result = new Cliente()
            {
                Id = Guid.NewGuid(),
                Nome = cliente.Nome,
                Email = cliente.Email,
            };
            var caminhoArq = _appSettings.PastaImagens + @$"\{result.Id}\logo.{cliente.Logotipo.Extensao}";
            result.Logotipo = caminhoArq;
            return result;
        }

        public async Task<RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>> AdicionarCliente(ClienteCreateModel cliente)
        {
            var clienteExistente = await BuscarClienteEmail(cliente.Email);

            if (clienteExistente != null)
            {
                var retorno = new RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>
                {
                    ExibicaoMensagem = new ExibicaoMensagemViewModel
                    {
                        Cabecalho = "E-mail",
                        Detalhes = "E-mail já cadastrado!",
                        MensagemCurta = "E-mail já cadastrado!",
                        StatusCode = 400
                    },
                    Objeto = new Guid()
                };
                return retorno;
            }



            RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid> retornoController = new RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>();
            try
            {
                var entityCliente = await ModelarCliente(cliente);
                byte[] bytes = Convert.FromBase64String(cliente.Logotipo.Base64);

                var caminho = Path.GetDirectoryName(entityCliente.Logotipo);
                if (!Directory.Exists(caminho))
                {
                    Directory.CreateDirectory(caminho);
                }
                using (FileStream fs = new FileStream(entityCliente.Logotipo, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }

                _clienteRepository.Add(entityCliente);
                _clienteRepository.Save();


                return retornoController;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public async Task<RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>> AtualizarCliente(ClienteEditModel cliente)
        {
            var clienteAtualizar = await BuscarClienteId(cliente.Id);

            if (clienteAtualizar == null)
            {
                throw new ArgumentNullException(nameof(clienteAtualizar), "Ciente não existe.");
            }

            var retornoController = new RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>();

            try
            {
                clienteAtualizar.Email = cliente.Email;
                clienteAtualizar.Nome = cliente.Nome;
                if (!string.IsNullOrWhiteSpace(cliente.Logotipo.Base64))
                {
                    var caminhoArq = _appSettings.PastaImagens + @$"\{clienteAtualizar.Id}\logo.{cliente.Logotipo.Extensao}";
                    clienteAtualizar.Logotipo = caminhoArq;
                    byte[] bytes = Convert.FromBase64String(cliente.Logotipo.Base64);

                    var caminho = Path.GetDirectoryName(clienteAtualizar.Logotipo);
                    if (!Directory.Exists(caminho))
                    {
                        Directory.CreateDirectory(caminho);
                    }
                    using (FileStream fs = new FileStream(clienteAtualizar.Logotipo, FileMode.Create))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }

                _clienteRepository.Update(clienteAtualizar);
                _clienteRepository.Save();

                return retornoController;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<string> DeletarCliente(string idCliente)
        {

            var result = _clienteRepository.GetSingleOrDefault(x => x.Id.ToString() == idCliente);

            if (result == null)
                throw new Exception("Cliente não encontrado.");

            _clienteRepository.Remove(result);
            _clienteRepository.Save();

            return idCliente;
        }

        static string ConvertParaBase64(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            string base64String = Convert.ToBase64String(fileBytes);
            return base64String;
        }
        public async Task<List<ClienteModel>> ListarClientes()
        {
            var ClienteAtivos = _clienteRepository.GetAll();

            List<ClienteModel> clientes = new List<ClienteModel>();
            foreach (var elem in ClienteAtivos)
            {
                var lista = new ClienteModel();
                lista.Id = elem.Id;
                lista.Nome = elem.Nome;
                lista.Email = elem.Email;
                lista.Logotipo = elem.Logotipo;
                if (File.Exists(elem.Logotipo))
                {
                    lista.Base64 = ConvertParaBase64(elem.Logotipo);
                    lista.Formato = Path.GetExtension(elem.Logotipo);
                }
                clientes.Add(lista);
            }
            return clientes.ToList();
        }
        public async Task<Cliente> BuscarClienteEmail(string email)
        {
            var cliente = _clienteRepository.Find(c => c.Email == email).FirstOrDefault();

            return cliente;
        }

        public async Task<Cliente> BuscarClienteId(Guid id)
        {
            var cliente = _clienteRepository.Find(c => c.Id == id).FirstOrDefault();

            return cliente;
        }
    }
}

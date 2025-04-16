using AutoMapper;
using DesafioTecnicoECS.Domain.Domain;
using DesafioTecnicoECS.Domain.Service.Generic;
using DesafioTecnicoECS.Infra.Context;
using DesafioTecnicoECS.Infra.UnitofWork;
using Microsoft.EntityFrameworkCore;
using DesafioTecnicoECS.Infra.Entity;
using DesafioTecnicoECS.Infra.Repositories.Interfaces;
using System.Data.SqlClient;

namespace DesafioTecnicoECS.Domain.Service
{
    public class LogradouroService<Tv, Te> : GenericServiceAsync<Tv, Te>
                                                where Tv : LogradouroModel
                                                where Te : Logradouro
    {
        ILogradouroRepository _logradouroRepository;
        private readonly ClientContext _clientContext;

        public LogradouroService(IUnitofWork unitOfWork, IMapper mapper,
                             ILogradouroRepository logradouroRepository, ClientContext clientContext)
        {
            if (_unitOfWork == null)
                _unitOfWork = unitOfWork;
            if (_mapper == null)
                _mapper = mapper;

            if (_logradouroRepository == null)
                _logradouroRepository = logradouroRepository;
            _clientContext = clientContext; ;
        }

        public Logradouro ModelarLogradouro(LogradouroCreateModel logradouro)
        {
            Logradouro result = new Logradouro()
            {
                Id = Guid.NewGuid(),
                IdCliente = logradouro.IdCliente,
                CEP = logradouro.CEP,
                Rua = logradouro.Rua,
                Cidade = logradouro.Cidade,
                Bairro = logradouro.Bairro,
                Estado = logradouro.Estado,
                Numero = logradouro.Numero,
                Complemento = logradouro.Complemento
            };

            return result;
        }

        public async Task<RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>> AdicionarLogradouro(LogradouroModel logradouro)
        {
            RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid> retornoController = new RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>();
            try
            {
                var id = new SqlParameter("@ID", Guid.NewGuid());
                var numero = new SqlParameter("@Numero", logradouro.Numero);
                var rua = new SqlParameter("@Rua", logradouro.Rua);
                var bairro = new SqlParameter("@Bairro", logradouro.Bairro);
                var cidade = new SqlParameter("@Cidade", logradouro.Cidade);
                var estado = new SqlParameter("@Estado", logradouro.Estado);
                var cep = new SqlParameter("@CEP", logradouro.CEP);
                var complemento = new SqlParameter("@Complemento", logradouro.Complemento);
                var idCliente = new SqlParameter("@IdCliente", logradouro.IdCliente);

                _clientContext.Database.ExecuteSqlRaw("EXEC InserirLogradouro @ID, @Numero, @Rua, @Bairro, @Cidade, @Estado, @CEP, @Complemento, @IdCliente", id, numero, rua, bairro, cidade, estado, cep, complemento, idCliente);
                _clientContext.SaveChanges();

                return retornoController;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<string> DeletarLogradouro(string idLogradouro)
        {

            var result = _logradouroRepository.GetSingleOrDefault(x => x.Id.ToString() == idLogradouro);

            if (result == null)
                throw new Exception("Logradouro não encontrado.");

            _logradouroRepository.Remove(result);
            _logradouroRepository.Save();

            return idLogradouro;
        }


        public async Task<List<LogradouroModel>> ListarLougradouros()
        {
            var LougradourosAtivos = _logradouroRepository.GetAll();

            List<LogradouroModel> lougradouros = new List<LogradouroModel>();
            foreach (var elem in LougradourosAtivos)
            {
                var lista = new LogradouroModel
                {
                    Id = elem.Id,
                    CEP = elem.CEP,
                    Rua = elem.Rua,
                    Cidade = elem.Cidade,
                    Bairro = elem.Bairro,
                    Estado = elem.Estado,
                    Numero = elem.Numero,
                    Complemento = elem.Complemento,
                    IdCliente = elem.IdCliente
                };
                lougradouros.Add(lista);
            }
            return lougradouros.ToList();
        }
    }
}

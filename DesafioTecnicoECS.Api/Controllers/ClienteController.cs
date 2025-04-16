using DesafioTecnicoECS.Domain.Domain;
using DesafioTecnicoECS.Domain.Service;
using DesafioTecnicoECS.Infra.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioTecnicoECS.Api.Controllers
{
    [Route("api/Cliente")]
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly ClienteService<ClienteModel, Cliente> _cliente;
        private readonly LogradouroService<LogradouroModel, Logradouro> _logradouroService;

        public ClienteController(ClienteService<ClienteModel, Cliente> clienteService, LogradouroService<LogradouroModel, Logradouro> logradouroService)
        {
            _cliente = clienteService;
            _logradouroService = logradouroService;
        }

        [HttpPost("Adicionar")]
        public async Task<IActionResult> Adicionar([FromBody] ClienteCreateModel cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (cliente == null)
                return BadRequest();

            var clienteResposta = await _cliente.AdicionarCliente(cliente);

            if (clienteResposta == null)
            {
                return StatusCode(500, "Erro ao adicionar Candidato!");
            }
            if (clienteResposta.ExibicaoMensagem != null)
            {
                return StatusCode(clienteResposta.ExibicaoMensagem.StatusCode, clienteResposta);
            }

            return Ok(clienteResposta);

        }

        [HttpPost("AdicionarLogradouro")]
        public async Task<IActionResult> AdicionarLogradouro([FromBody] LogradouroModel logradouro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (logradouro == null)
                return BadRequest();
            logradouro.Id = Guid.NewGuid();
            var resp = await _logradouroService.AdicionarLogradouro(logradouro);

            if (resp == null)
            {
                return StatusCode(500, "Erro ao adicionar Candidato!");
            }

            return Ok(resp);
        }

        [HttpPost("Atualizar")]
        public async Task<IActionResult> Atualizar([FromBody] ClienteEditModel cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (cliente == null)
                return BadRequest();

            var clienteResposta = await _cliente.AtualizarCliente(cliente);

            if (clienteResposta == null)
            {
                return StatusCode(500, "Erro ao atualizar Candidato!");
            }
            if (clienteResposta.ExibicaoMensagem != null)
            {
                return StatusCode(clienteResposta.ExibicaoMensagem.StatusCode, clienteResposta);
            }

            return Ok(clienteResposta);
        }

        [HttpDelete("Deletar/{id}")]
        public async Task<IActionResult> DeletarCliente(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var resultado = await _cliente.DeletarCliente(id);
            return Ok();
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> ListarTodas()
        {


            var clientes = await _cliente.ListarClientes();

            return Ok(clientes);
        }

        [HttpGet("RetornaPorId/{id}")]
        public async Task<IActionResult> RetornaPorId(Guid id)
        {

            var clientes = await _cliente.Get(x => x.Id == id);

            return Ok(clientes.FirstOrDefault());
        }

        [HttpGet("ObterLogradouros/{idCliente}")]
        public async Task<IActionResult> ObterLogradouros(Guid idCliente)
        {
            var clientes = await _logradouroService.Get(x => x.IdCliente == idCliente);
            return Ok(clientes);
        }

        [HttpDelete("RemoverLogradouro/{id}")]
        public async Task<IActionResult> RemoverLogradouro(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var resultado = await _logradouroService.Remove(id);
            return Ok();
        }

        [HttpGet("RetornaLogradouroPorId/{id}")]
        public async Task<IActionResult> RetornaLogradouroPorId(Guid id)
        {
            var clientes = await _logradouroService.Get(x => x.Id == id);

            return Ok(clientes.FirstOrDefault());
        }


        [HttpPost("EditarLogradouro")]
        public async Task<IActionResult> EditarLogradouro([FromBody] LogradouroModel logradouro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (logradouro == null)
                return BadRequest();

            var resp = await _logradouroService.Update(logradouro);

            if (resp <= 0)
            {
                return StatusCode(500, "Erro ao adicionar Candidato!");
            }


            return Ok(resp);
        }

    }
}

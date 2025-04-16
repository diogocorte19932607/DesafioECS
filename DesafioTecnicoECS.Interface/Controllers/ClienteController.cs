using DesafioTecnicoECS.Interface.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;



namespace DesafioTecnicoECS.Interface.Controllers
{
    public class ClienteController : BaseController
    {
        private readonly ILogger<ClienteController> _logger;
        public ClienteController(ILogger<ClienteController> logger, IConfiguration configuration, IHttpClientFactory httpClient) : base(configuration, httpClient)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> Editar(Guid id)
        {
            await GetToken();
            var getCandidato = await _httpClient.GetAsync("api/Cliente/RetornaPorId/" + id.ToString());
            if (!getCandidato.IsSuccessStatusCode)
            {
                throw new HttpRequestException(getCandidato.ToString());
            }
            var resultCliente = JsonConvert.DeserializeObject<DtoCliente>(await getCandidato.Content.ReadAsStringAsync());
            return View(resultCliente);
        }

        [HttpGet]
        public async Task<IActionResult> ListarClientes()
        {
            await GetToken();
            var getCandidato = await _httpClient.GetAsync("api/Cliente/Listar");
            if (!getCandidato.IsSuccessStatusCode)
            {
                throw new HttpRequestException(getCandidato.ToString());
            }
            var resultCliente = JsonConvert.DeserializeObject<List<DtoCliente>>(await getCandidato.Content.ReadAsStringAsync());
            return Json(resultCliente);
        }

        [HttpPost]
        public async Task<IActionResult> CriarCliente([FromBody] DtoClienteCreate cliente)
        {
            try
            {
                await GetToken();

                var jsonContent = JsonConvert.SerializeObject(cliente);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_configuration["ApiURL"] + "api/Cliente/Adicionar", contentString);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { sucesso = true, ret = await response.Content.ReadAsStringAsync() });
                }
                else
                {
                    return Json(new { sucesso = false, ret = await response.Content.ReadAsStringAsync() });
                }
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        [HttpPost]
        public async Task<bool> EditarCliente([FromBody] DtoClienteCreate cliente)
        {
            await GetToken();

            var jsonContent = JsonConvert.SerializeObject(cliente);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_configuration["ApiURL"] + "api/Cliente/Atualizar", contentString);

            return response.IsSuccessStatusCode;
        }

        [HttpDelete("DeletarCliente/{id}")]
        public async Task<bool> DeletarCliente(string id)
        {
            await GetToken();

            var response = await _httpClient.DeleteAsync(_configuration["ApiURL"] + "api/Cliente/Deletar/" + id);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        [HttpPost]
        public async Task<bool> AdicionarLogradouro([FromBody] DtoLogradouroCreate logradouro)
        {
            try
            {
                await GetToken();

                var jsonContent = JsonConvert.SerializeObject(logradouro);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_configuration["ApiURL"] + "api/Cliente/AdicionarLogradouro", contentString);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        public async Task<IActionResult> ListarLogradouros(Guid id)
        {
            await GetToken();
            var getCandidato = await _httpClient.GetAsync("api/Cliente/ObterLogradouros/" + id);
            if (!getCandidato.IsSuccessStatusCode)
            {
                throw new HttpRequestException(getCandidato.ToString());
            }
            var resultCliente = JsonConvert.DeserializeObject<List<DtoLogradouro>>(await getCandidato.Content.ReadAsStringAsync());
            return Json(resultCliente);
        }

        [HttpDelete("RemoverLogradouro/{id}")]
        public async Task<bool> RemoverLogradouro(string id)
        {
            await GetToken();

            var response = await _httpClient.DeleteAsync(_configuration["ApiURL"] + "api/Cliente/RemoverLogradouro/" + id);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public async Task<IActionResult> EditarLogradouro(Guid id)
        {
            await GetToken();
            var getCandidato = await _httpClient.GetAsync("api/Cliente/RetornaLogradouroPorId/" + id.ToString());
            if (!getCandidato.IsSuccessStatusCode)
            {
                throw new HttpRequestException(getCandidato.ToString());
            }
            var resultCliente = JsonConvert.DeserializeObject<DtoLogradouro>(await getCandidato.Content.ReadAsStringAsync());
            return Json(resultCliente);
        }

        [HttpPost]
        public async Task<bool> EditarLogradouro([FromBody] DtoLogradouro logradouro)
        {
            try
            {
                await GetToken();

                var jsonContent = JsonConvert.SerializeObject(logradouro);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_configuration["ApiURL"] + "api/Cliente/EditarLogradouro", contentString);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }





        //public async Task<IActionResult> CadastrarCandidato()
        //{

        //    List<DtoCandidato> ListaCandidato = new List<DtoCandidato>();

        //    using (var client = new HttpClient())
        //    {
        //        var getCandidato = await client.GetAsync(_configuration["BaseUrl"] + "api/Candidato/Listar");
        //        if (!getCandidato.IsSuccessStatusCode)
        //        {
        //            throw new HttpRequestException(getCandidato.ToString());
        //        }
        //        var resultCandidato = JsonConvert.DeserializeObject<List<DtoCandidato>>(await getCandidato.Content.ReadAsStringAsync());
        //        if (resultCandidato != null)
        //        {


        //            foreach (var item in resultCandidato)
        //            {
        //                ListaCandidato.Add(item);
        //            }
        //        }
        //    }
        //    List<DtoLegenda> ListaLegenda = new List<DtoLegenda>();

        //    var DtoLegenda1 = new DtoLegenda(13, "PT");
        //    var DtoLegenda2 = new DtoLegenda(45, "PSDB");
        //    var DtoLegenda3 = new DtoLegenda(33, "PSOL");
        //    var DtoLegenda4 = new DtoLegenda(22, "PSL");
        //    var DtoLegenda5 = new DtoLegenda(15, "P2");
        //    var DtoLegenda6 = new DtoLegenda(18, "P1");

        //    ListaLegenda.Add(DtoLegenda1);
        //    ListaLegenda.Add(DtoLegenda2);
        //    ListaLegenda.Add(DtoLegenda3);
        //    ListaLegenda.Add(DtoLegenda4);
        //    ListaLegenda.Add(DtoLegenda5);
        //    ListaLegenda.Add(DtoLegenda6);


        //    List<SelectListItem> legendas;
        //    legendas = ListaLegenda.Select(c => new SelectListItem()
        //    {
        //        Text = c.NomeLegenda.ToUpper(),
        //        Value = c.IdLegenda.ToString()
        //    }).ToList();

        //    ViewBag.ListaCandidato = ListaCandidato;
        //    ViewBag.ListaLegenda = legendas;

        //    return View();
        //}

        //[HttpPost("CadastrarCandidato")]
        //public async Task<bool> CadastrarCandidato([FromBody] DtoCandidato candidato)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Add("Authorization", "bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2OTE5NTcxOTAsImV4cCI6MTY5MTk2NDM5MCwiaWF0IjoxNjkxOTU3MTkwLCJpc3MiOiJNZXVTaXN0ZW1hIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3QifQ.xg3oI8StGBwXnA4SgFjHe_zq2K0ufhZ6_Bvm99QiDs4");
        //        var response1 = await client.GetAsync("https://localhost:7295/WeatherForecast/Listar");

        //        var test = response1;

        //        var getLegenda = await client.GetAsync(_configuration["BaseUrl"] + "api/Candidato/legenda/" + candidato.Legenda); ;
        //        if (!getLegenda.IsSuccessStatusCode)
        //        {
        //            throw new HttpRequestException(getLegenda.ToString());
        //        }
        //        var legenda = JsonConvert.DeserializeObject<DtoCandidato>(await getLegenda.Content.ReadAsStringAsync());
        //        if (legenda != null)
        //        {
        //            return false;
        //        }

        //        var jsonContent = JsonConvert.SerializeObject(candidato);
        //        var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        //        var response = await client.PostAsync(_configuration["BaseUrl"] + "api/Candidato/Adicionar", contentString);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}




        //[HttpPost("BuscarCandidato/{legenda}")]
        //public async Task<DtoCandidato> BuscarCandidato(int legenda)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        var response = await client.GetAsync(_configuration["BaseUrl"] + "api/Candidato/legenda/" + legenda);
        //        var jsonString = response.Content.ReadAsStringAsync();
        //        var candidato = JsonConvert.DeserializeObject<DtoCandidato>(jsonString.Result);

        //        return candidato;
        //    }
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

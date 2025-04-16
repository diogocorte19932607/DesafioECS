using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using DesafioTecnicoECS.Interface.Model;



namespace DesafioTecnicoECS.Interface.Controllers
{
    public class ProductController : BaseController
    {
        private readonly ILogger<ProductController> _logger;
        public ProductController(ILogger<ProductController> logger, IConfiguration configuration, IHttpClientFactory httpClient) : base(configuration, httpClient)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            await GetToken();

            var response = await _httpClient.GetAsync("api/Product/RetornaTodos");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Erro ao buscar produtos: " + response.StatusCode);
                ViewBag.Erro = "Erro ao carregar produtos.";
                return View(new List<DtoProduto>());
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var produtos = JsonConvert.DeserializeObject<List<DtoProduto>>(jsonString);

            return View(produtos);
        }

        [HttpPost]
        public async Task<IActionResult> CriarProdutos([FromBody] DtoProdutoiCreate produto)
        {
            try
            {
                await GetToken();

                var jsonContent = JsonConvert.SerializeObject(produto);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_configuration["ApiURL"] + "api/Product/Adicionar", contentString);

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

        [HttpGet]
        public async Task<IActionResult> ListarProdutos()
        {
            await GetToken();
            var getProduto = await _httpClient.GetAsync("api/Product/RetornaTodos");
            if (!getProduto.IsSuccessStatusCode)
            {
                throw new HttpRequestException(getProduto.ToString());
            }
            var produto = JsonConvert.DeserializeObject<List<DtoProduto>>(await getProduto.Content.ReadAsStringAsync());
            return Json(produto);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
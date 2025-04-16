using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DesafioTecnicoECS.Domain.Domain;
using DesafioTecnicoECS.Domain.Service;
using DesafioTecnicoECS.Infra.Entity;

namespace DesafioTecnicoECS.Api.Controllers
{
    [Route("api/Product")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ProductService<ProductModel, Product> _product;

        public ProductController(ProductService<ProductModel, Product> productService)
        {
            _product = productService;
        }

        [HttpPost("Adicionar")]
        public async Task<IActionResult> Adicionar([FromBody] ProductCreateModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (product == null)
                return BadRequest();

            var response = await _product.AdicionarProduct(product);

            if (response == null)
            {
                return StatusCode(500, "Erro ao adicionar produto!");
            }
            if (response.ExibicaoMensagem != null)
            {
                return StatusCode(response.ExibicaoMensagem.StatusCode, response);
            }

            return Ok(response);

        }


        [HttpGet("RetornaTodos")]
        public async Task<IActionResult> RetornaTodos()
        {
            var products = await _product.ListarProducts();
            return Ok(products);
        }

            }
}

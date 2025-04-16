using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;
using System.Net.Http;
using System;

using DesafioTecnicoECS.Interface.Model;
using DesafioTecnicoECS.Interface.Services;
using System.Threading.Tasks;

namespace DesafioTecnicoECS.Interface.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        protected readonly HttpClient _httpClient;

        public BaseController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("Client");
            _httpClient.BaseAddress = new Uri(_configuration["ApiURL"]);
        }

        protected async Task GetToken()
        {
            if (HttpClientAuthorizationDelegatingHandler.TokenDto == null || !HttpClientAuthorizationDelegatingHandler.CheckTokenIsValid(HttpClientAuthorizationDelegatingHandler.TokenDto.AccessToken))
            {

                var token = await _httpClient.GetAsync("api/Authorize/GetToken");
                if (!token.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(token.ToString());
                }
                HttpClientAuthorizationDelegatingHandler.TokenDto = JsonConvert.DeserializeObject<TokenDto>(await token.Content.ReadAsStringAsync());

            }
        }
    }
}

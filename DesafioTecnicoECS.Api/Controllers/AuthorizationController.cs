using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using DesafioTecnicoECS.Domain.Configuration;

namespace DesafioTecnicoECS.Api.Controllers
{
    [ApiController]
    [Route("api/Authorize")]
    public class AuthorizeController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        public AuthorizeController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        [HttpGet("GetToken")]
        public IActionResult GerarToken()
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var claims = new Dictionary<string, object>();
            claims.Add("Perfil", "Teste");
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Claims = claims
            });
            var encodedToken = tokenHandler.WriteToken(token);

            var response = new
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
            };

            return Ok(response);
        }
    }
}

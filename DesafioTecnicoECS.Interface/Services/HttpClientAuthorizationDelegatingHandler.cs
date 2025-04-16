using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using DesafioTecnicoECS.Interface.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace DesafioTecnicoECS.Interface.Services
{
    public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
    {
        public static TokenDto TokenDto;
        public HttpClientAuthorizationDelegatingHandler()
        {
        }

        private static long GetTokenExpirationTime(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
            var ticks = long.Parse(tokenExp);
            return ticks;
        }

        public static bool CheckTokenIsValid(string token)
        {
            var tokenTicks = GetTokenExpirationTime(token);
            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;

            var now = DateTime.Now.ToUniversalTime();

            var valid = tokenDate >= now;

            return valid;
        }

     
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (TokenDto != null && CheckTokenIsValid(TokenDto.AccessToken))
            {
                request.Headers.Add("Authorization", new List<string>() { "bearer "+ TokenDto.AccessToken });
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}

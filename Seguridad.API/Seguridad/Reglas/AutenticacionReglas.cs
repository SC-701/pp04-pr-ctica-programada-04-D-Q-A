using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Reglas
{
    public static class Autenticacion
    {
        // Genera el hash SHA256 de una cadena de texto
        public static string GenerarHash(string texto)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        // Lee un JWT y retorna el valor de un claim específico
        public static string ObtenerHash(string token, string claimType)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims
                           .FirstOrDefault(c => c.Type == claimType)?.Value ?? string.Empty;
        }

        // Convierte el JWT en un ClaimsIdentity (para la cookie de sesión del WEB)
        public static ClaimsIdentity GenerarClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims.ToList();

            // Agregar el token completo como claim para enviarlo después al API
            claims.Add(new Claim("AccessToken", token));

            return new ClaimsIdentity(
                claims,
                Microsoft.AspNetCore.Authentication.Cookies
                          .CookieAuthenticationDefaults.AuthenticationScheme);
        }

        // Lee directamente el JWT sin desencriptar (Base64)
        public static string leerToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.ToString();
        }
    }
}
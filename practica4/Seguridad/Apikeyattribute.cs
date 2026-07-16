using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace practica4.Seguridad
{
    public class Apikeyattribute : Attribute, IAuthorizationFilter
    {
        private const string APIKEY = "X-API-KEY";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(APIKEY, out var extractedApiKey))
            {
                context.Result = new ObjectResult("Por favor, proporciona una API Key") {
                    StatusCode = 401 };
                return;
            }

            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apivalida = appSettings["ApiKey"];

            if (string.IsNullOrEmpty(apivalida) || !apivalida.Equals(extractedApiKey))
            {
                context.Result = new ObjectResult("API Key no válida") {
                    StatusCode = 401 };
                return;
            }
        }
    }
}

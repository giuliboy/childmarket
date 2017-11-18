using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Market.WebAPI.Utils
{
    /// <summary>
    /// eigene middleware die HTTP requests auf die HTTPS redirected
    /// </summary>
    public class EnforceHttpsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _sslPort;

        public EnforceHttpsMiddleware( RequestDelegate next, int sslPort )
        {
            _next = next;
            _sslPort = sslPort;
        }

        public async Task Invoke( HttpContext context )
        {
            if ( context.Request.IsHttps )
            {
                await _next( context );
            }
            else
            {
                string sslPortStr = string.Empty;
                if ( _sslPort != 0 && _sslPort != 443 )
                {
                    sslPortStr = $":{_sslPort}";
                }
                string httpsUrl = $"https://{context.Request.Host.Host}{sslPortStr}{context.Request.Path}";
                context.Response.Redirect( httpsUrl );
            }
        }
    }
}

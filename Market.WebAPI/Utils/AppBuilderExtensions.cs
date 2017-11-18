using Microsoft.AspNetCore.Builder;
using System;

namespace Market.WebAPI.Utils
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseHttpsEnforcement( this IApplicationBuilder app, int sslPort )
        {
            if ( app == null )
            {
                throw new ArgumentNullException( nameof( app ) );
            }
            return app.UseMiddleware<EnforceHttpsMiddleware>( sslPort );
        }
    }
}

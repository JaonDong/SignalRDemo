using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(SignalRConsole.Startup))]

namespace SignalRConsole
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //设置跨域
            app.UseCors(CorsOptions.AllowAll)
               .MapSignalR();
        }
    }
}
